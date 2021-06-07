using System.Linq;
using ASD_project.ActionHandling.DTO;
using ASD_project.DatabaseHandler.POCO;
using ASD_project.DatabaseHandler.Services;
using ASD_project.Network;
using ASD_project.Network.DTO;
using ASD_project.Network.Enum;
using ASD_project.World.Services;
using Messages;
using Newtonsoft.Json;

namespace ASD_project.ActionHandling
{
    public class MoveHandler : IMoveHandler, IPacketHandler
    {
        private IClientController _clientController;
        private IWorldService _worldService;
        private IMessageService _messageService;
        private IDatabaseService<PlayerPOCO> _playerServicesDb;

        public MoveHandler(IClientController clientController, IWorldService worldService, IDatabaseService<PlayerPOCO> playerServicesDb, IMessageService messageService)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.Move);
            _worldService = worldService;
            _messageService = messageService;
            _playerServicesDb = playerServicesDb;
        }

        public void SendMove(string directionValue, int stepsValue)
        {
            int x = 0;
            int y = 0;
            switch (directionValue)
            {
                case "right":
                case "east":
                    x = stepsValue;
                    break;
                case "left":
                case "west":
                    x = -stepsValue;
                    break;
                case "forward":
                case "up":
                case "north":
                    y = +stepsValue;
                    break;
                case "backward":
                case "down":
                case "south":
                    y = -stepsValue;
                    break;
            }

            var currentPlayer = _worldService.GetCurrentPlayer();

            MoveDTO moveDTO = new(currentPlayer.Id, currentPlayer.XPosition + x, currentPlayer.YPosition + y);

            SendMoveDTO(moveDTO);
        }

        private void SendMoveDTO(MoveDTO moveDTO)
        {
            var payload = JsonConvert.SerializeObject(moveDTO);
            _clientController.SendPayload(payload, PacketType.Move);
        }

        public HandlerResponseDTO HandlePacket(PacketDTO packet)
        {
            var moveDTO = JsonConvert.DeserializeObject<MoveDTO>(packet.Payload);

            //check for backup host like comments below
            //(_clientController.IsHost() && packet.Header.Target.Equals("host")) || _clientController.IsBackupHost)
            if (_clientController.IsHost() && packet.Header.Target.Equals("host"))
            {
                var allLocations = _playerServicesDb.GetAllAsync();

                allLocations.Wait();

                int newPosPlayerX = moveDTO.XPosition;
                int newPosPlayerY = moveDTO.YPosition;

                var result =
                    allLocations.Result.Where(x =>
                        x.XPosition == newPosPlayerX && x.YPosition == newPosPlayerY &&
                        x.GameGUID == _clientController.SessionId);

                if (result.Any())
                {
                    return new HandlerResponseDTO(SendAction.ReturnToSender, "Can't move to new position something is in the way");
                }
                else
                {
                    InsertToDatabase(moveDTO);
                    HandleMove(moveDTO);
                }
            }
            else if (packet.Header.Target.Equals(_clientController.GetOriginId()))
            {
                _messageService.AddMessage(packet.HandlerResponse.ResultMessage);
            }
            else
            {
                HandleMove(moveDTO);
            }

            return new HandlerResponseDTO(SendAction.SendToClients, null);
        }

        private void InsertToDatabase(MoveDTO moveDTO)
        {            
            var player = _playerServicesDb.GetAllAsync().Result.FirstOrDefault(player => player.PlayerGUID == moveDTO.UserId && player.GameGUID == _clientController.SessionId);

            player.XPosition = moveDTO.XPosition;
            player.YPosition = moveDTO.YPosition;
            _playerServicesDb.UpdateAsync(player);
        }

        private void HandleMove(MoveDTO moveDTO)
        {
            _worldService.UpdateCharacterPosition(moveDTO.UserId, moveDTO.XPosition, moveDTO.YPosition);
        }
    }
}