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
        private IClientController _clientController;
        private string _playerGuid;
        private IWorldService _worldService;
        const int STAMINA = 10;

        public AttackHandler(IClientController clientController, IWorldService worldService)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.Attack);
            _worldService = worldService;
        }

        public void SendAttack(string direction)
        {
            Weapon weapon = _worldService.getCurrentPlayer().Inventory.Weapon;
            int x = 0;
            int y = 0;
            switch (direction)
            {
                case "right":
                case "east":
                    x = weapon.GetWeaponDistance();
                    break;
                case "left":
                case "west":
                    x = -weapon.GetWeaponDistance();
                    break;
                case "forward":
                case "up":
                case "north":
                    y = +weapon.GetWeaponDistance();
                    break;
                case "backward":
                case "down":
                case "south":
                    y = -weapon.GetWeaponDistance();
                    break;
            }

            var currentPlayer = _worldService.getCurrentPlayer();
            AttackDTO attackDto = new AttackDTO();
            attackDto.XPosition = currentPlayer.XPosition + x;
            attackDto.YPosition = currentPlayer.YPosition + y;
            attackDto.Damage = weapon.GetWeaponDamage();
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
                var dbConnection = new DbConnection();

                var playerRepository = new Repository<PlayerPOCO>(dbConnection);
                var servicePlayer = new ServicesDb<PlayerPOCO>(playerRepository);

                var allPlayers = servicePlayer.GetAllAsync();

                allPlayers.Wait();

                var result =
                    allPlayers.Result.Where(x =>
                        x.XPosition == attackDto.XPosition && x.YPosition == attackDto.YPosition &&
                        x.GameGuid == _clientController.SessionId);

                InsertStaminaToDatabase(attackDto);

                if (result.Any())
                {
                    attackDto.AttackedPlayerGuid = result.FirstOrDefault().PlayerGuid;
                    InsertDamageToDatabase(attackDto);
                    packet.Payload = JsonConvert.SerializeObject(attackDto);
                }
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
            var dbConnection = new DbConnection();
            var playerRepository = new Repository<PlayerPOCO>(dbConnection);
            var player = playerRepository.GetAllAsync().Result
                .FirstOrDefault(player => player.PlayerGuid == attackDto.PlayerGuid);
            player.Stamina -= STAMINA;
            playerRepository.UpdateAsync(player);
        }

        private void InsertDamageToDatabase(AttackDTO attackDto)
        {
            var dbConnection = new DbConnection();

            var attackedPlayerRepository = new Repository<PlayerPOCO>(dbConnection);
            var attackedPlayer = attackedPlayerRepository.GetAllAsync().Result
                .FirstOrDefault(attackedPlayer => attackedPlayer.PlayerGuid == attackDto.AttackedPlayerGuid);
            attackedPlayer.Health -= attackDto.Damage;
            attackedPlayerRepository.UpdateAsync(attackedPlayer);
        }

        private void HandleAttack(AttackDTO attackDto)
        {
            if (_clientController.GetOriginId().Equals(attackDto.PlayerGuid))
            {
                Console.WriteLine("Your stamina got lowered with " + STAMINA + ".");
                _worldService.getCurrentPlayer().Stamina -= STAMINA;
            }

            if (_clientController.GetOriginId().Equals(attackDto.AttackedPlayerGuid))
            {
                Console.WriteLine("You took " + attackDto.Damage + " damage.");
                _worldService.getCurrentPlayer().Health -= attackDto.Damage;
            }
        }
    }
}