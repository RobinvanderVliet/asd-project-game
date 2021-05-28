using System;
using System.Linq;
using AutoMapper;
using DatabaseHandler;
using DatabaseHandler.POCO;
using DatabaseHandler.Repository;
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
        private IMapper _mapper;

        public AttackHandler(IMapper mapper, IClientController clientController, IWorldService worldService)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.Attack);
            _worldService = worldService;
            _mapper = mapper;
        }

        public void SendAttack(string direction)
        {
            Weapon weapon = _worldService...
            int x = 0;
            int y = 0;
            switch (direction)
            {
                case "right":
                case "east":
                    x = attackRange;
                    break;
                case "left":
                case "west":
                    x = -attackRange;
                    break;
                case "forward":
                case "up":
                case "north":
                    y = +attackRange;
                    break;
                case "backward":
                case "down":
                case "south":
                    y = -attackRange;
                    break;
            }

            var currentPlayer = _worldService.getCurrentPlayer();
            AttackDTO attackDto = new AttackDTO();
            attackDto.XPosition = currentPlayer.XPosition + x;
            attackDto.YPosition = currentPlayer.YPosition + x;
            attackDto.Damage = damage;
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
                var playerToAttackGuid = playerRepository.GetAllAsync().Result.FirstOrDefault(player =>
                    player.XPosition == attackDto.XPosition && player.YPosition == attackDto.YPosition).PlayerGuid; // check wat er uit komt als er geen speler staat
                
                if (playerToAttackGuid.Equals(""))
                {
                    return new HandlerResponseDTO(SendAction.ReturnToSender, "There is no enemy to attack");
                }
                else
                {
                    attackDto.AttackedPlayerGuid = playerToAttackGuid;

                    var player = playerRepository.GetAllAsync().Result
                        .FirstOrDefault(player => player.PlayerGuid == attackDto.AttackedPlayerGuid);
                
                    player.Health -= attackDto.Damage;
                    playerRepository.UpdateAsync(player);
                }
            }
            else if (packet.Header.Target.Equals(_clientController.GetOriginId()))
            {
                Console.WriteLine(packet.HandlerResponse.ResultMessage);
                // stamina verlagen
            }
            HandleAttack(attackDto);
            return new HandlerResponseDTO(SendAction.SendToClients, null);
        }

        private void HandleAttack(AttackDTO attackDto)
        {
            if (_clientController.GetOriginId().Equals(attackDto.PlayerGuid))
            {
                // stamina verlagen
                
            }

            if (_clientController.GetOriginId().Equals(attackDto.AttackedPlayerGuid))
            {
                // verlagen health met damage uit attackDto
            }
        }
    }
}