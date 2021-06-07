using Network;
using System.Linq;
using ActionHandling.DTO;
using Messages;
using Network.DTO;
using Newtonsoft.Json;
using WorldGeneration;


namespace ActionHandling
{
    public class DeadHandler : IDeadHandler, IPacketHandler
    {
        private readonly IClientController _clientController;
        private readonly IWorldService _worldService;
        private readonly IMessageService _messageService;

        public DeadHandler(IClientController clientController, IWorldService worldService, IMessageService messageService)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.Dead);
            _worldService = worldService;
            _messageService = messageService;
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
            _worldService.PlayerDied(deadDTO.DeadPlayer);
            _worldService.GetAllPlayers().Where(player => player.Id == deadDTO.DeadPlayer.Id).FirstOrDefault().Symbol =
                deadDTO.DeadPlayer.Symbol;
            _worldService.DisplayWorld();
            if (deadDTO.DeadPlayer.Id == _clientController.GetOriginId())
            {
                _messageService.AddMessage("Your health reached 0, you died!");
                _worldService.GetCurrentPlayer().Health = 0;
                _worldService.DisplayStats();
            }
            return new HandlerResponseDTO(SendAction.SendToClients, null);
        }
    }
}