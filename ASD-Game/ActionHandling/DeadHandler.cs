using Network;
using System;
using System.Linq;
using ActionHandling.DTO;
using DatabaseHandler;
using DatabaseHandler.POCO;
using DatabaseHandler.Repository;
using DatabaseHandler.Services;
using Network.DTO;
using Newtonsoft.Json;
using WorldGeneration;


namespace ActionHandling
{
    public class DeadHandler : IDeadHandler, IPacketHandler
    {
        private readonly IClientController _clientController;
        private readonly IWorldService _worldService;

        public DeadHandler(IClientController clientController, IWorldService worldService)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.Dead);
            _worldService = worldService;
        }

        public void SendDead(Player deadPlayer)
        {
            DeadDTO deadDto = new DeadDTO();
            deadDto.DeadPlayer = deadPlayer;
            SendDeadDTO(deadDto);
        }

        private void SendDeadDTO(DeadDTO deadDTO)
        {
            var payload = JsonConvert.SerializeObject(deadDTO);
            _clientController.SendPayload(payload, PacketType.Dead);
        }

        public HandlerResponseDTO HandlePacket(PacketDTO packet)
        {
            var deadDTO = JsonConvert.DeserializeObject<DeadDTO>(packet.Payload);
            _worldService.playerDied(deadDTO.DeadPlayer);
            _worldService.getAllPlayers().Where(player => player.Id == deadDTO.DeadPlayer.Id).FirstOrDefault().Symbol =
                deadDTO.DeadPlayer.Symbol;
            _worldService.DisplayWorld();
            if (deadDTO.DeadPlayer.Id == _clientController.GetOriginId())
            {
                Console.WriteLine("You died.");
            }
            return new HandlerResponseDTO(SendAction.SendToClients, null);
        }
    }
}