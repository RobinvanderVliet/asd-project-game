using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Timers;
using ActionHandling;
using ActionHandling.DTO;
using ASD_Game.DatabaseHandler.POCO;
using ASD_Game.DatabaseHandler.Services;
using ASD_Game.Items;
using ASD_Game.Messages;
using ASD_Game.Network;
using ASD_Game.Network.DTO;
using ASD_Game.Network.Enum;
using ASD_Game.World.Models.Characters;
using ASD_Game.World.Services;
using Newtonsoft.Json;

namespace ASD_Game.ActionHandling
{
    public class AttackHandler : IAttackHandler, IPacketHandler
    {
        private readonly IClientController _clientController;
        private string _playerGuid;
        private readonly IWorldService _worldService;
        private const int ATTACK_STAMINA = 10;
        private readonly IDatabaseService<PlayerPOCO> _playerDatabaseService;
        private readonly IDatabaseService<PlayerItemPOCO> _playerItemDatabaseService;
        private readonly IDatabaseService<CreaturePOCO> _creatureDatabaseService;
        private readonly IMessageService _messageService;

        private Timer AIUpdateTimer;
        private int _updateTime = 2000;

        public AttackHandler(IClientController clientController, IWorldService worldService,
            IDatabaseService<PlayerPOCO> playerDatabaseService,
            IDatabaseService<PlayerItemPOCO> playerItemDatabaseService,
            IDatabaseService<CreaturePOCO> creatureDatabaseService, IMessageService messageService)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.Attack);
            _worldService = worldService;
            _playerDatabaseService = playerDatabaseService;
            _playerItemDatabaseService = playerItemDatabaseService;
            _creatureDatabaseService = creatureDatabaseService;
            _messageService = messageService;
            CheckAITimer();
        }

        public void SendAttack(string direction)
        {
            if (_worldService.IsDead(_worldService.GetCurrentPlayer()))
            {
                _messageService.AddMessage("You can't attack, you're dead!");
                return;
            }
            
            AttackDTO attackDTO = new AttackDTO();
            attackDTO.Direction = direction;
            attackDTO.PlayerGuid = _worldService.GetCurrentPlayer().Id;
            SendAttackDTO(attackDTO);
        }

        public void SendAttackDTO(AttackDTO attackDto)
        {
            var payload = JsonConvert.SerializeObject(attackDto);
            _clientController.SendPayload(payload, PacketType.Attack);
        }

        public HandlerResponseDTO HandlePacket(PacketDTO packet)
        {
            AttackDTO attackDTO = JsonConvert.DeserializeObject<AttackDTO>(packet.Payload);
            
            var handleInDatabase = (_clientController.IsHost() && packet.Header.Target.Equals("host")) || _clientController.IsBackupHost;
            
            //check stamina
            var attackingCharacter = _worldService.GetCharacter(attackDTO.PlayerGuid);
            if (attackingCharacter is Player)
            {
                var attackingPlayer = (Player) attackingCharacter;
                if (attackingPlayer.Stamina <= ATTACK_STAMINA)
                {
                    if (isMe(attackingPlayer.Id))
                    {
                        _messageService.AddMessage("You don't have enough stamina to attack.");
                    }
                    return new HandlerResponseDTO(SendAction.ReturnToSender, "You don't have enough stamina to attack.");
                }
                else
                {
                    LowerStamina(attackingPlayer, handleInDatabase);
                }
            }

            var attackedCharacter = GetCharacterToAttack(attackingCharacter, attackDTO.Direction);
            
            if (attackedCharacter == null)
            {
                if (isMe(attackingCharacter.Id))
                {
                    _messageService.AddMessage("There is no enemy to attack.");
                }

                return new HandlerResponseDTO(SendAction.ReturnToSender, "There is no enemy to attack.");
            }
            else if (attackedCharacter.Health <= 0)
            {
                if (isMe(attackingCharacter.Id))
                {
                    _messageService.AddMessage("You can't attack this enemy, he is already dead.");
                }

                return new HandlerResponseDTO(SendAction.ReturnToSender, "You can't attack this enemy, he is already dead.");
            }
            
            if (isMe(attackingCharacter.Id))
            {
                _messageService.AddMessage("You attacked an enemy.");
                _worldService.DisplayStats();
            }
            
            AttackCharacter(attackedCharacter, attackingCharacter.GetDamage(), handleInDatabase);
            
            return new(SendAction.SendToClients, null);
        }

        private void AttackCharacter(Character attackedCharacter, int damage, bool handleInDatabase)
        {
            if (attackedCharacter is Player)
            {
                var attackedPlayer = _worldService.GetPlayer(attackedCharacter.Id);
                
                if (isMe(attackedPlayer.Id))
                {
                    _messageService.AddMessage("You've been attacked! You took a total of: " + damage + " damage.");
                }

                if (damage > 0 && attackedPlayer.Inventory.Helmet != null)
                {
                    if (damage >= attackedPlayer.Inventory.Helmet.ArmorProtectionPoints)
                    {
                        if (isMe(attackedPlayer.Id))
                        {
                            _messageService.AddMessage("Your helmet has been destroyed!");
                        }

                        damage -= attackedPlayer.Inventory.Helmet.ArmorProtectionPoints;

                        DeleteHelmet(attackedPlayer, handleInDatabase);

                    }
                    else
                    {
                        UpdateHelmet(attackedPlayer, damage, handleInDatabase);
                        damage = 0;
                    }
                }
                if (damage > 0 && attackedPlayer.Inventory.Armor != null)
                {
                    if (damage >= attackedPlayer.Inventory.Armor.ArmorProtectionPoints)
                    {
                        if (isMe(attackedPlayer.Id))
                        {
                            _messageService.AddMessage("Your armor has been destroyed!");
                        }
                        
                        damage -= attackedPlayer.Inventory.Armor.ArmorProtectionPoints;

                        DeleteArmor(attackedPlayer, handleInDatabase);
                    }
                    else
                    {
                        UpdateArmor(attackedPlayer, damage, handleInDatabase);
                        damage = 0;
                    }
                }
                if (damage > 0)
                {
                    TakeDamage(attackedPlayer, damage, handleInDatabase);

                    if (isMe(attackedCharacter.Id))
                    {
                        _worldService.DisplayStats();
                    }
                }
            }
            else
            {
                attackedCharacter.Health -= damage;
            }
        }

        private void TakeDamage(Player attackedPlayer, int damage, bool handleInDatabase)
        {
            attackedPlayer.AddHealth(-damage);
            if (handleInDatabase)
            {
                var playerPOCO = _playerDatabaseService.GetAllAsync().Result.FirstOrDefault(playerPOCO => playerPOCO.PlayerGUID == attackedPlayer.Id && playerPOCO.GameGUID == _clientController.SessionId);
                playerPOCO.Health = attackedPlayer.Health;
                _playerDatabaseService.UpdateAsync(playerPOCO);
            }

            if (attackedPlayer.Health <= 0)
            {
                if (isMe(attackedPlayer.Id))
                {
                    _messageService.AddMessage("You died.");
                    _worldService.DisplayWorld();
                }
            }
        }

        private bool isMe(string id)
        {
            return id.Equals(_clientController.GetOriginId());
        }

        private Character GetCharacterToAttack(Character attackingCharacter, string direction)
        {
            int range = attackingCharacter.GetRange();
            Character character = null;
            
            if(direction.Equals("right") || direction.Equals("east"))
            {
                for(int x = 1; x <= range && character is null; x++)
                {
                    character = _worldService.GetCharacterOnTile(attackingCharacter.XPosition + x, attackingCharacter.YPosition);
                }
            }
            else if (direction.Equals("left") || direction.Equals("west"))
            {
                for (int x = 1; x <= range && character is null; x++)
                {
                    character = _worldService.GetCharacterOnTile(attackingCharacter.XPosition - x, attackingCharacter.YPosition);
                }
            }
            else if (direction.Equals("up") || direction.Equals("north") || direction.Equals("forward"))
            {
                for (int y = 1; y <= range && character is null; y++)
                {
                    character = _worldService.GetCharacterOnTile(attackingCharacter.XPosition, attackingCharacter.YPosition + y);
                }
            }
            else if (direction.Equals("down") || direction.Equals("south") || direction.Equals("backward"))
            {
                for (int y = 1; y <= range && character is null; y++)
                {
                    character = _worldService.GetCharacterOnTile(attackingCharacter.XPosition, attackingCharacter.YPosition - y);
                }
            }
            return character;
        }

        private void LowerStamina(Player attackingPlayer, bool handleInDatabase)
        {
            attackingPlayer.AddStamina(-ATTACK_STAMINA);
            if (handleInDatabase)
            {
                var playerPOCO = _playerDatabaseService.GetAllAsync().Result.FirstOrDefault(playerPOCO => playerPOCO.PlayerGUID == attackingPlayer.Id && playerPOCO.GameGUID == _clientController.SessionId);
                playerPOCO.Stamina = attackingPlayer.Stamina;
                _playerDatabaseService.UpdateAsync(playerPOCO);
            }
        }

        private void DeleteHelmet(Player attackedPlayer, bool handleInDatabase)
        {
            if (handleInDatabase)
            {
                var playerItemPOCO = _playerItemDatabaseService.GetAllAsync().Result.FirstOrDefault(playerItemPOCO => playerItemPOCO.PlayerGUID == attackedPlayer.Id && playerItemPOCO.GameGUID == _clientController.SessionId && playerItemPOCO.ItemName == attackedPlayer.Inventory.Helmet.ItemName);
                _playerItemDatabaseService.DeleteAsync(playerItemPOCO);
            }
            attackedPlayer.Inventory.Helmet = null;
                                    
        }

        private void UpdateHelmet(Player attackedPlayer, int damage, bool handleInDatabase)
        {
            attackedPlayer.Inventory.Helmet.ArmorProtectionPoints -= damage;
            if (handleInDatabase)
            {
                var playerItemPOCO = _playerItemDatabaseService.GetAllAsync().Result.FirstOrDefault(playerItemPOCO => playerItemPOCO.PlayerGUID == attackedPlayer.Id && playerItemPOCO.GameGUID == _clientController.SessionId && playerItemPOCO.ItemName == attackedPlayer.Inventory.Helmet.ItemName);
                playerItemPOCO.ArmorPoints = attackedPlayer.Inventory.Helmet.ArmorProtectionPoints;
                _playerItemDatabaseService.UpdateAsync(playerItemPOCO);
            }
        }

        private void DeleteArmor(Player attackedPlayer, bool handleInDatabase)
        {
            if (handleInDatabase)
            {
                var playerItemPOCO = _playerItemDatabaseService.GetAllAsync().Result.FirstOrDefault(playerItemPOCO => playerItemPOCO.PlayerGUID == attackedPlayer.Id && playerItemPOCO.GameGUID == _clientController.SessionId && playerItemPOCO.ItemName == attackedPlayer.Inventory.Armor.ItemName);
                _playerItemDatabaseService.DeleteAsync(playerItemPOCO);
            }
            attackedPlayer.Inventory.Armor = null;
                                    
        }

        private void UpdateArmor(Player attackedPlayer, int damage, bool handleInDatabase)
        {
            attackedPlayer.Inventory.Armor.ArmorProtectionPoints -= damage;
            if (handleInDatabase)
            {
                var playerItemPOCO = _playerItemDatabaseService.GetAllAsync().Result.FirstOrDefault(playerItemPOCO => playerItemPOCO.PlayerGUID == attackedPlayer.Id && playerItemPOCO.GameGUID == _clientController.SessionId && playerItemPOCO.ItemName == attackedPlayer.Inventory.Armor.ItemName);
                playerItemPOCO.ArmorPoints = attackedPlayer.Inventory.Armor.ArmorProtectionPoints;
                _playerItemDatabaseService.UpdateAsync(playerItemPOCO);
            }
        }

        public void GetAIMoves()
        {
            AIAttack(_worldService.GetCreatureMoves("Attack"));
        }
        
        public void AIAttack(List<Character> creatureMoves)
        {
            List<AttackDTO> attackDTOs = new List<AttackDTO>();
            if (creatureMoves != null)
            {
                foreach (Character move in creatureMoves)
                {
                    if (move is SmartMonster smartMonster)
                    {
                        if (smartMonster.MoveType == "Attack")
                        {
                            AttackDTO attackDTO = new();
                            
                            if (smartMonster.XPosition < smartMonster.Destination.X)
                            {
                                attackDTO.Direction = "right";
                            }
                            else if (smartMonster.XPosition > smartMonster.Destination.X)
                            {
                                attackDTO.Direction = "left";
                            }
                            else if (smartMonster.YPosition < smartMonster.Destination.Y)
                            {
                                attackDTO.Direction = "up";
                            }
                            else if (smartMonster.YPosition > smartMonster.Destination.Y)
                            {
                                attackDTO.Direction = "down";
                            }
                            
                            attackDTO.PlayerGuid = smartMonster.Id;
                            attackDTOs.Add(attackDTO);
                        }
                    }
                }
                foreach (AttackDTO attack in attackDTOs)
                {
                    SendAttackDTO(attack);
                }
            }
        }

        [ExcludeFromCodeCoverage]
        private void CheckAITimer()
        {
            AIUpdateTimer = new Timer(_updateTime)
            {
                AutoReset = true
            };
            AIUpdateTimer.Elapsed += CheckAITimerEvent;
            AIUpdateTimer.Start();
        }

        [ExcludeFromCodeCoverage]
        private void CheckAITimerEvent(object sender, ElapsedEventArgs e)
        {
            AIUpdateTimer.Stop();
            GetAIMoves();
            AIUpdateTimer.Start();
        }
    }
}