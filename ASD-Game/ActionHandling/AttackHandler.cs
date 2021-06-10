using System.Linq;
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
using DatabaseHandler.POCO;
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
        }

        public void SendAttack(string direction)
        {
            if (_worldService.IsDead(_worldService.GetCurrentPlayer()))
            {
                _messageService.AddMessage("You can't attack, you're dead!");
                return;
            }

            Weapon weapon = _worldService.GetCurrentPlayer().Inventory.Weapon;
            int weaponDistance = (int) weapon.Distance;
            int x = 0;
            int y = 0;
            switch (direction)
            {
                case "right":
                case "east":
                    x = weaponDistance;
                    break;
                case "left":
                case "west":
                    x = -weaponDistance;
                    break;
                case "forward":
                case "up":
                case "north":
                    y = weaponDistance;
                    break;
                case "backward":
                case "down":
                case "south":
                    y = -weaponDistance;
                    break;
            }

            var currentPlayer = _worldService.GetCurrentPlayer();
            AttackDTO attackDto = new AttackDTO();
            attackDto.XPosition = currentPlayer.XPosition + x;
            attackDto.YPosition = currentPlayer.YPosition + y;
            attackDto.Damage = (int) weapon.Damage;
            attackDto.Stamina = currentPlayer.Stamina;
            attackDto.PlayerGuid = _clientController.GetOriginId();
            SendAttackDTO(attackDto);
        }

        public void SendAttackDTO(AttackDTO attackDto)
        {
            var payload = JsonConvert.SerializeObject(attackDto);
            _clientController.SendPayload(payload, PacketType.Attack);
        }

        public HandlerResponseDTO HandlePacket(PacketDTO packet)
        {
            AttackDTO attackDto = JsonConvert.DeserializeObject<AttackDTO>(packet.Payload);

            if (_worldService.GetPlayer(attackDto.PlayerGuid) != null &&
                _worldService.GetPlayer(attackDto.PlayerGuid).Stamina < 10)
            {
                if (_clientController.GetOriginId().Equals(attackDto.PlayerGuid))
                {
                    _messageService.AddMessage("You did not have enough stamina to attack.");
                }
                return new HandlerResponseDTO(SendAction.ReturnToSender, "You did not have enough stamina to attack.");
            }
            LowerStamina(attackDto.PlayerGuid);
            
            if (_clientController.IsHost() && packet.Header.Target.Equals("host") || _clientController.IsBackupHost)
            {
                InsertStaminaToDatabase(attackDto);
                var allCharacters = _worldService.GetAllCharacters();
                var characterToAttack =
                    allCharacters.Find(x =>
                        x.XPosition == attackDto.XPosition && x.YPosition == attackDto.YPosition);
                if (characterToAttack != null)
                {
                    if (characterToAttack.Health <= 0)
                    {
                        if (_clientController.GetOriginId().Equals(attackDto.PlayerGuid))
                        {
                            _messageService.AddMessage("You can't attack this enemy, he is already dead.");
                            return new HandlerResponseDTO(SendAction.Ignore, null);
                        }
                    }
                }

                if (characterToAttack != null)
                {
                    if(_worldService.GetPlayer(characterToAttack.Id) != null)
                    {
                        attackDto.AttackedPlayerGuid = characterToAttack.Id;
                        InsertDamageToDatabase(attackDto, true);
                        packet.Payload = JsonConvert.SerializeObject(attackDto);
                    } else if (_worldService.GetAI(characterToAttack.Id) != null)
                    {
                        attackDto.AttackedPlayerGuid = characterToAttack.Id;
                        packet.Payload = JsonConvert.SerializeObject(attackDto);
                    }
                }
                else
                {
                    if (_clientController.GetOriginId().Equals(attackDto.PlayerGuid))
                    {
                        _messageService.AddMessage("There is no enemy to attack");
                    }
                    return new HandlerResponseDTO(SendAction.ReturnToSender,
                        "There is no enemy to attack");
                }
            }
            else if (packet.Header.Target.Equals(_clientController.GetOriginId()))
            {
                _messageService.AddMessage(packet.HandlerResponse.ResultMessage);
            }

            HandleAttack(attackDto);
            return new HandlerResponseDTO(SendAction.SendToClients, null);
        }

        private void InsertStaminaToDatabase(AttackDTO attackDto)
        {
            var player = _playerDatabaseService.GetAllAsync().Result
                .FirstOrDefault(player =>
                    player.PlayerGUID == attackDto.PlayerGuid && player.GameGUID == _clientController.SessionId);
            player.Stamina -= ATTACK_STAMINA;
            _playerDatabaseService.UpdateAsync(player);
        }

        private void InsertDamageToDatabase(AttackDTO attackDto, bool isPlayer) // both armor and health damage
        {
            int totalDamage = attackDto.Damage;
            if (isPlayer)
            {
                var attackedPlayer = _playerDatabaseService.GetAllAsync().Result
                    .FirstOrDefault(attackedPlayer => attackedPlayer.PlayerGUID == attackDto.AttackedPlayerGuid);

                var attackedPlayerInArray =
                    _worldService.GetAllPlayers().Where(player => player.Id == attackDto.PlayerGuid).FirstOrDefault();

                if (attackedPlayerInArray.Inventory.Helmet != null)
                {
                    var attackedPlayerHelmet = attackedPlayerInArray.Inventory.Helmet;
                    var helmetPoints = attackedPlayerHelmet.ArmorProtectionPoints;

                    var attackedPlayerItem = _playerItemDatabaseService.GetAllAsync();
                    attackedPlayerItem.Wait();
                    var results = attackedPlayerItem.Result.OrderByDescending(a => a.ArmorPoints)
                        .First(playerItem =>
                            playerItem.PlayerGUID == attackDto.PlayerGuid &&
                            playerItem.ItemName ==
                            attackedPlayerHelmet.ItemName &&
                            playerItem.GameGUID == _clientController.SessionId);

                    if (helmetPoints - totalDamage <= 0 && helmetPoints != 0)
                    {
                        totalDamage -= results.ArmorPoints;
                        results.ArmorPoints = 0;
                        _playerItemDatabaseService.UpdateAsync(results);
                        // TODO: Delete playerItemPoco form database (function DeleteAsync)
                    }
                    else
                    {
                        results.ArmorPoints -= totalDamage;
                        _playerItemDatabaseService.UpdateAsync(results);
                    }
                }

                if (attackedPlayerInArray.Inventory.Armor != null)
                {
                    var attackedPlayerBodyArmor = attackedPlayerInArray.Inventory.Armor;
                    var bodyArmorPoints = attackedPlayerBodyArmor.ArmorProtectionPoints;
                    var attackedPlayerItem = _playerItemDatabaseService.GetAllAsync();
                    attackedPlayerItem.Wait();
                    var results = attackedPlayerItem.Result.OrderByDescending(a => a.ArmorPoints)
                        .First(playerItem => playerItem.PlayerGUID == attackDto.PlayerGuid &&
                                             playerItem.ItemName ==
                                             attackedPlayerBodyArmor.ItemName &&
                                             playerItem.GameGUID == _clientController.SessionId);
                    if (bodyArmorPoints - totalDamage <= 0 && bodyArmorPoints != 0)
                    {
                        totalDamage -= results.ArmorPoints;
                        results.ArmorPoints = 0;
                        // TODO: Delete playerItemPoco form database (function DeleteAsync)
                    }
                    else
                    {
                        results.ArmorPoints -= totalDamage;
                        _playerItemDatabaseService.UpdateAsync(results);

                        attackedPlayer.Health -= totalDamage;
                        _playerDatabaseService.UpdateAsync(attackedPlayer);
                    }
                }
                else
                {
                    if (attackedPlayer.Health - totalDamage >= 0)
                    {
                        attackedPlayer.Health -= totalDamage;
                        _playerDatabaseService.UpdateAsync(attackedPlayer);
                    }
                    else
                    {
                        attackedPlayer.Health = 0;
                        _playerDatabaseService.UpdateAsync(attackedPlayer);
                    }
                }
            }
        }

        private void LowerStamina(string playerId)
        {
            var player = _worldService.GetPlayer(playerId);
            bool printAttackMessage = _clientController.GetOriginId().Equals(player.Id);
            if (player.Stamina >= ATTACK_STAMINA && printAttackMessage)
            {
                player.Stamina -= ATTACK_STAMINA;
            }
        }

        private void HandleAttack(AttackDTO attackDto)
        {
            var creature = _worldService.GetAI(attackDto.AttackedPlayerGuid);
            var player = _worldService.GetPlayer(attackDto.PlayerGuid);
            bool printAttackMessage = _clientController.GetOriginId().Equals(player.Id);
            
            if (printAttackMessage)
            {
                _messageService.AddMessage("You attacked an enemy.");
            }

            if (creature == null)
            {
                var attackedPlayer = _worldService.GetPlayer(attackDto.AttackedPlayerGuid);
                bool printAttackedMessage = _clientController.GetOriginId().Equals(attackedPlayer.Id);
                if (attackDto.AttackedPlayerGuid != null && attackedPlayer.Health != 0)
                {
                    if (printAttackedMessage)
                    {
                        _messageService.AddMessage(
                            "You've been attacked! You took a total of: " + attackDto.Damage + " damage.");
                    }

                    int ArmorPoints = 0;
                    int HelmetPoints = 0;
                    if (attackedPlayer.Inventory.Armor != null)
                    {
                        ArmorPoints = attackedPlayer.Inventory.Armor.ArmorProtectionPoints;
                    }

                    if (attackedPlayer.Inventory.Helmet != null)
                    {
                        HelmetPoints = attackedPlayer.Inventory.Helmet.ArmorProtectionPoints;
                    }
                    
                    if (HelmetPoints - attackDto.Damage <= 0 && HelmetPoints != 0)
                    {
                        if (printAttackedMessage)
                        {
                            _messageService.AddMessage("Your helmet has been destroyed!");
                        }

                        attackDto.Damage -= HelmetPoints;
                        attackedPlayer.Inventory.Helmet = null;
                        _worldService.DisplayStats();
                    }
                    else if (HelmetPoints != 0)
                    {
                        attackDto.Damage = 0;
                        attackedPlayer.Inventory.Helmet.ArmorProtectionPoints -= attackDto.Damage;
                        _worldService.DisplayStats();
                    }

                    if (ArmorPoints - attackDto.Damage <= 0 && ArmorPoints != 0)
                    {
                        if (printAttackedMessage)
                        {
                            _messageService.AddMessage("Your armor piece has been destroyed!");
                        }

                        attackDto.Damage -= ArmorPoints;
                        attackedPlayer.Inventory.Armor = null;
                        attackedPlayer.Health -= attackDto.Damage;
                        _worldService.DisplayStats();
                    }
                    else if (ArmorPoints != 0)
                    {
                        attackedPlayer.Inventory.Armor.ArmorProtectionPoints -= attackDto.Damage;
                        _worldService.DisplayStats();
                    }
                    else
                    {
                        if (attackedPlayer.Health - attackDto.Damage >= 0)
                        {
                            attackedPlayer.Health -= attackDto.Damage;
                        }
                        else
                        {
                            if (printAttackedMessage)
                            {
                                _messageService.AddMessage("You died");
                            }

                            attackedPlayer.Health = 0;
                        }
                    }
                }
            }
            else
            {
                creature.Health -= attackDto.Damage;
            }

            _worldService.DisplayStats();
            _worldService.DisplayWorld();
        }
    }
}