using System;
using System.Linq;
using Castle.Core.Internal;
using DatabaseHandler;
using DatabaseHandler.POCO;
using DatabaseHandler.Repository;
using DatabaseHandler.Services;
using Items;
using Network;
using Network.DTO;
using Newtonsoft.Json;
using Player.DTO;
using WorldGeneration;

namespace Player.ActionHandlers
{
    public class AttackHandler : IAttackHandler, IPacketHandler
    {
        private IClientController _clientController;
        private string _playerGuid;
        private IWorldService _worldService;

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
            Console.WriteLine("ik kom hier als client");
            AttackDTO attackDto = JsonConvert.DeserializeObject<AttackDTO>(packet.Payload);

            // if (_clientController.IsHost() && packet.Header.Target.Equals("host"))
            // {
            //     var dbConnection = new DbConnection();
            //
            //     var playerRepository = new Repository<PlayerPOCO>(dbConnection);
            //     try
            //     {
            //         var player = playerRepository.GetAllAsync().Result.FirstOrDefault(player =>
            //             player.XPosition == attackDto.XPosition && player.YPosition == attackDto.YPosition &&
            //             player.GameGuid.Equals(_clientController.SessionId));
            //         attackDto.AttackedPlayerGuid = player.PlayerGuid;
            //         player.Health -= attackDto.Damage;
            //         playerRepository.UpdateAsync(player);
            //         //HandleAttack(attackDto);
            //     }
            //     catch (Exception e)
            //     {
            //         HandleAttack(attackDto);
            //         return new HandlerResponseDTO(SendAction.ReturnToSender, "There is no enemy to attack");
            //     }
            // }
            // else if (packet.Header.Target.Equals(_clientController.GetOriginId()))
            // {
            //     Console.WriteLine(packet.HandlerResponse.ResultMessage);
            // }
            // else
            // {
            //     HandleAttack(attackDto);
            // }
            // return new HandlerResponseDTO(SendAction.SendToClients, null);

            if (_clientController.IsHost() && packet.Header.Target.Equals("host"))
            {
                var dbConnection = new DbConnection();

                var playerRepository = new Repository<PlayerPOCO>(dbConnection);
                var servicePlayer = new ServicesDb<PlayerPOCO>(playerRepository);

                var allLocations = servicePlayer.GetAllAsync();

                allLocations.Wait();

                var result =
                    allLocations.Result.Where(x =>
                        x.XPosition == attackDto.XPosition && x.YPosition == attackDto.YPosition &&
                        x.GameGuid == _clientController.SessionId);

                if (result.Any())
                {
                    attackDto.AttackedPlayerGuid = result.FirstOrDefault().PlayerGuid;
                    InsertToDatabase(attackDto);
                    HandleAttack(attackDto);
                    packet.Payload = JsonConvert.SerializeObject(attackDto);
                }
                else
                {
                    HandleAttack(attackDto);
                    return new HandlerResponseDTO(SendAction.ReturnToSender,
                        "There is no enemy to attack");
                }
            }
            else if (packet.Header.Target.Equals(_clientController.GetOriginId()))
            {
                Console.WriteLine(packet.HandlerResponse.ResultMessage);
            }
            else
            {
                HandleAttack(attackDto);
            }

            return new HandlerResponseDTO(SendAction.SendToClients, null);
        }

        private void InsertToDatabase(AttackDTO attackDto)
        {
            var dbConnection = new DbConnection();

            var playerRepository = new Repository<PlayerPOCO>(dbConnection);
            var player = playerRepository.GetAllAsync().Result
                .FirstOrDefault(player => player.PlayerGuid == attackDto.AttackedPlayerGuid);

            player.Health -= attackDto.Damage;
            playerRepository.UpdateAsync(player);
        }

        private void HandleAttack(AttackDTO attackDto)
        {
            Console.WriteLine(_clientController.GetOriginId());
            if (_clientController.GetOriginId().Equals(attackDto.PlayerGuid))
            {
                int stamina = 10;
                Console.WriteLine("Your stamina got lowered with "+stamina+".");
                _worldService.getCurrentPlayer().Stamina -= stamina; // later aanpassen met variabel stamina
            }

            if (_clientController.GetOriginId().Equals(attackDto.AttackedPlayerGuid))
            {
                Console.WriteLine("You took "+ attackDto.Damage +" damage.");
                _worldService.getCurrentPlayer().Health -= attackDto.Damage;
            }
        }
    }
}