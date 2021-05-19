using System;
using Network;
using Network.DTO;
using Newtonsoft.Json;
using Session.DTO;

namespace Session
{
    
    public class GameSessionHandler : IPacketHandler, IGameSessionHandler
    {
        private IClientController _clientController;
        public GameSessionHandler(IClientController clientController)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.GameSession);
        }
        
        // public void SendGameSession(StartGameDto startGameDto)
        // {
        //     ;
        // }
        
        // private void SendGameSessionDTO()
        // {
        //     var payload = JsonConvert.SerializeObject(moveDTO);
        //     _clientController.SendPayload(payload, PacketType.Move);
        // }
        public HandlerResponseDTO HandlePacket(PacketDTO packet)
        {
            var startGameDTO = JsonConvert.DeserializeObject<StartGameDto>(packet.Payload);
            HandleStartGameSession(startGameDTO);
            return new HandlerResponseDTO(SendAction.SendToClients, null);
        }

        private void HandleStartGameSession(StartGameDto startGameDto)
        {
            Console.WriteLine("ik ben er");
        }
    }
}