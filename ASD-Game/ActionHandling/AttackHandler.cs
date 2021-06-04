using System;
using System.Linq;
using ActionHandling.DTO;
using DatabaseHandler;
using DatabaseHandler.POCO;
using DatabaseHandler.Repository;
using DatabaseHandler.Services;
using Items;
using Network;
using Network.DTO;
using Newtonsoft.Json;
using WorldGeneration;

namespace ActionHandling
{
    public class AttackHandler : IAttackHandler, IPacketHandler
    {
        private readonly IClientController _clientController;
        private string _playerGuid;
        private readonly IWorldService _worldService;
        private const int ATTACK_STAMINA = 10;
        private readonly IDeadHandler _deadHandler;
        private readonly IDatabaseService<PlayerPOCO> _playerDatabaseService;
        private readonly IDatabaseService<PlayerItemPOCO> _playerItemDatabaseService;
        private readonly IDatabaseService<CreaturePOCO> _creatureDatabaseService;


        public AttackHandler(IClientController clientController, IWorldService worldService, IDeadHandler deadHandler, IDatabaseService<PlayerPOCO> playerDatabaseService, IDatabaseService<PlayerItemPOCO> playerItemDatabaseService, IDatabaseService<CreaturePOCO> creatureDatabaseService)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.Attack);
            _worldService = worldService;
            _deadHandler = deadHandler;
            _playerDatabaseService = playerDatabaseService;
            _playerItemDatabaseService = playerItemDatabaseService;
            _creatureDatabaseService = creatureDatabaseService;
        }

        public void SendAttack(string direction)
        {
            if (_worldService.isDead(_worldService.GetCurrentPlayer()))
            {
                Console.WriteLine("You can't attack, you're dead!");
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

            if (_clientController.IsHost() && packet.Header.Target.Equals("host"))
            {
                var allPlayers = _worldService.getAllPlayers();
                var PlayerResult =
                    allPlayers.Where(x =>
                        x.XPosition == attackDto.XPosition && x.YPosition == attackDto.YPosition);
                if (PlayerResult.FirstOrDefault() != null)
                {
                    if (PlayerResult.FirstOrDefault().Health <= 0)
                    {
                        if (_clientController.GetOriginId().Equals(attackDto.PlayerGuid))
                        {
                            Console.WriteLine("You can't attack this enemy, he is already dead.");
                            return new HandlerResponseDTO(SendAction.Ignore, null);
                        }
                    }
                }

                
                //var CreatureResult =
                //    allCreatures.Where(x =>
                //        x.XPosition == attackDto.XPosition && x.YPosition == attackDto.YPosition &&
                //        x.GameGuid == _clientController.SessionId);

                InsertStaminaToDatabase(attackDto);

                if (PlayerResult.Any())
                {
                    attackDto.AttackedPlayerGuid = PlayerResult.FirstOrDefault().Id;
                    if (attackDto.Stamina >= ATTACK_STAMINA)
                    {
                        InsertDamageToDatabase(attackDto, true);
                        packet.Payload = JsonConvert.SerializeObject(attackDto);
                    }
                }
                //else if (CreatureResult.Any())
                //{
                //    attackDto.AttackedPlayerGuid = CreatureResult.FirstOrDefault().CreatureGuid;
                //    InsertDamageToDatabase(attackDto, false);
                //    packet.Payload = JsonConvert.SerializeObject(attackDto);
                //}
                else
                {
                    if (_clientController.GetOriginId().Equals(attackDto.PlayerGuid))
                    {
                        HandleAttack(attackDto);
                        Console.WriteLine("There is no enemy to attack");
                    }

                    return new HandlerResponseDTO(SendAction.ReturnToSender,
                        "There is no enemy to attack");
                }
            }
            else if (packet.Header.Target.Equals(_clientController.GetOriginId()))
            {
                Console.WriteLine(packet.HandlerResponse.ResultMessage);
            }

            HandleAttack(attackDto);
            return new HandlerResponseDTO(SendAction.SendToClients, null);
        }

        private void InsertStaminaToDatabase(AttackDTO attackDto)
        {
            var player = _playerDatabaseService.GetAllAsync().Result
                .FirstOrDefault(player => player.PlayerGuid == attackDto.PlayerGuid);
            player.Stamina -= ATTACK_STAMINA;
            _playerDatabaseService.UpdateAsync(player);
        }

        private void InsertDamageToDatabase(AttackDTO attackDto, Boolean isPlayer) // both armor and health damage
        {
            int totalDamage = attackDto.Damage;
            if (isPlayer)
            {
                var attackedPlayer = _playerDatabaseService.GetAllAsync().Result
                    .FirstOrDefault(attackedPlayer => attackedPlayer.PlayerGuid == attackDto.AttackedPlayerGuid);
                
                var attackedPlayerInArray =
                    _worldService.getAllPlayers().Where(player => player.Id == attackDto.PlayerGuid).FirstOrDefault();

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
                    attackedPlayer.Health -= totalDamage;
                    _playerDatabaseService.UpdateAsync(attackedPlayer);
                }
            }
            else
            {
                var attackedCreature = _creatureDatabaseService.GetAllAsync().Result
                    .FirstOrDefault(attackedCreature => attackedCreature.CreatureGuid == attackDto.AttackedPlayerGuid);
                attackedCreature.Health -= attackDto.Damage;
                _creatureDatabaseService.UpdateAsync(attackedCreature);

                if (attackedCreature.Health <= 0)
                {
                    Console.WriteLine("RIP"); //TODO implement death of creature
                }
            }
        }

        private void HandleAttack(AttackDTO attackDto)
        {
            if (_clientController.GetOriginId().Equals(attackDto.PlayerGuid))
            {
                if (_worldService.GetCurrentPlayer().Stamina < ATTACK_STAMINA)
                {
                    Console.WriteLine("You're out of stamina, you can't attack.");
                }
                else
                {
                    Console.WriteLine("Your stamina got lowered with " + ATTACK_STAMINA + ".");
                    _worldService.GetCurrentPlayer().Stamina -= ATTACK_STAMINA;
                    Console.WriteLine("stamina: " + _worldService.GetCurrentPlayer().Stamina);    
                }
            }

            if (_clientController.GetOriginId().Equals(attackDto.AttackedPlayerGuid))
            {
                Console.WriteLine("You took a total of " + attackDto.Damage + " damage.");
                int ArmorPoints = 0;
                int HelmetPoints = 0;
                if (_worldService.GetCurrentPlayer().Inventory.Armor != null)
                {
                    ArmorPoints = _worldService.GetCurrentPlayer().Inventory.Armor.ArmorProtectionPoints;
                }

                if (_worldService.GetCurrentPlayer().Inventory.Helmet != null)
                {
                    HelmetPoints = _worldService.GetCurrentPlayer().Inventory.Helmet.ArmorProtectionPoints;
                }

                //Eerst wordt Damage van de helm afgehaald, vervolgens van de body armor en tot slot van de speler.
                if (HelmetPoints - attackDto.Damage <= 0 && HelmetPoints != 0)
                {
                    Console.WriteLine("Your helmet has been destroyed!");
                    attackDto.Damage -= HelmetPoints;
                    _worldService.GetCurrentPlayer().Inventory.Helmet = null;
                }
                else if (HelmetPoints != 0)
                {
                    Console.WriteLine("Your Helmet took " + attackDto.Damage + " damage.");
                    attackDto.Damage = 0;
                    _worldService.GetCurrentPlayer().Inventory.Helmet.ArmorProtectionPoints -= attackDto.Damage;
                }

                if (ArmorPoints - attackDto.Damage <= 0 && ArmorPoints != 0)
                {
                    Console.WriteLine("Your armor piece has been destroyed!");
                    attackDto.Damage -= ArmorPoints;
                    _worldService.GetCurrentPlayer().Inventory.Armor = null;
                    _worldService.GetCurrentPlayer().Health -= attackDto.Damage;
                    Console.WriteLine("Your health took " + attackDto.Damage + " damage.");
                }
                else if (ArmorPoints != 0)
                {
                    Console.WriteLine("Your armor took " + attackDto.Damage + " damage.");
                    _worldService.GetCurrentPlayer().Inventory.Armor.ArmorProtectionPoints -= attackDto.Damage;
                }
                else
                {
                    Console.WriteLine("Your health took " + attackDto.Damage + " damage.");
                    _worldService.GetCurrentPlayer().Health -= attackDto.Damage;
                }

                if (_worldService.GetCurrentPlayer().Health <= 0)
                {
                    _deadHandler.SendDead(_worldService.GetCurrentPlayer());
                }
            }
        }
    }
}