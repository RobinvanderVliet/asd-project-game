using System;
using DataTransfer.DTO.Character;
using Network;
using Network.DTO;
using Newtonsoft.Json;
using Session.DTO;
using WorldGeneration;

namespace Session
{
    
    public class GameSessionHandler : IPacketHandler, IGameSessionHandler
    {
        private IClientController _clientController;
        private ISessionHandler _sessionHandler;
        private IWorldService _worldService;
        public GameSessionHandler(IClientController clientController, IWorldService worldService)
        {
            _worldService = worldService;
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.GameSession);
        }
        
        public void SendGameSession(string messageValue, ISessionHandler sessionHandler)
        {
            _sessionHandler = sessionHandler;
            Console.WriteLine(messageValue);
            var dto =  _sessionHandler.SetupGameHost();
            SendGameSessionDTO(dto);
            
        }
        
        private void SendGameSessionDTO(StartGameDto startGameDto)
        {
            var payload = JsonConvert.SerializeObject(startGameDto);
            _clientController.SendPayload(payload, PacketType.GameSession);
        }
        public HandlerResponseDTO HandlePacket(PacketDTO packet)
        {
            var startGameDTO = JsonConvert.DeserializeObject<StartGameDto>(packet.Payload);
            HandleStartGameSession(startGameDTO);
            return new HandlerResponseDTO(SendAction.SendToClients, null);
        }

        private void HandleStartGameSession(StartGameDto startGameDto)
        {
            if (_clientController.IsHost())
            {
                Console.WriteLine("Ik ben de host, ga iets doen met de database");
            }

            // if (_clientController.GetOriginId() == startGameDto.id)
            // {
            //     // doe iets met eigen database
                    // eigen model?
            // }

            foreach (var player in startGameDto.PlayerLocations)
            {
                MapCharacterDTO mapCharacterDto = new MapCharacterDTO(player.Value[0], player.Value[1], player.Key);
                _worldService.AddCharacterToWorld(mapCharacterDto);
            }
            
        }
    }
}