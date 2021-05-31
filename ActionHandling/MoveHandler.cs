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
    public class MoveHandler : IMoveHandler, IPacketHandler
    {
        private IClientController _clientController;
        private IWorldService _worldService;

        public MoveHandler(IClientController clientController, IWorldService worldService)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.Move);
            _worldService = worldService;
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

            var currentPlayer = _worldService.getCurrentPlayer();
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
                var dbConnection = new DbConnection();

                var playerRepository = new Repository<PlayerPOCO>(dbConnection);
                var servicePlayer = new ServicesDb<PlayerPOCO>(playerRepository);
                
                // Thread.Sleep(1500);
                
                var allLocations = servicePlayer.GetAllAsync();

                allLocations.Wait();

                int newPosPlayerX = moveDTO.XPosition;
                int newPosPlayerY = moveDTO.YPosition;
                
                int oldPosPlayerX =
                    allLocations.Result.Where(x =>
                        x.PlayerGuid == moveDTO.UserId
                    ).Select(x => x.XPosition).FirstOrDefault();
                int oldPosPlayerY =
                    allLocations.Result.Where(x =>
                        x.PlayerGuid == moveDTO.UserId
                    ).Select(x => x.YPosition).FirstOrDefault();

                var steps = Math.Abs(newPosPlayerX - oldPosPlayerX) + Math.Abs(newPosPlayerY - oldPosPlayerY);

                var result =
                    allLocations.Result.Where(x =>
                        x.XPosition == newPosPlayerX && x.YPosition == newPosPlayerY &&
                        x.GameGuid == _clientController.SessionId);

                var resultStamina =
                    allLocations.Result.Where(x =>
                        x.PlayerGuid == moveDTO.UserId
                    ).Select(x => x.Stamina).FirstOrDefault();

                if (result.Any())
                {
                    Console.WriteLine("Can't move to new position something is in the way");
                    return new HandlerResponseDTO(SendAction.ReturnToSender, "Can't move to new position something is in the way");
                }
                else if (resultStamina < steps)
                {
                    Console.WriteLine("You do not have enough stamina to move!");
                    return new HandlerResponseDTO(SendAction.ReturnToSender, "You do not have enough stamina to move!");
                }
                else
                {
                    moveDTO.Stamina = resultStamina - steps;
                    InsertToDatabase(moveDTO);
                    HandleMove(moveDTO);
                    packet.Payload = JsonConvert.SerializeObject(moveDTO);
                }
            }
            else if (packet.Header.Target.Equals(_clientController.GetOriginId()))
            {
                Console.WriteLine(packet.HandlerResponse.ResultMessage);
            }
            else
            {
                HandleMove(moveDTO);
            }
            
            return new HandlerResponseDTO(SendAction.SendToClients, null);
        }

        private void InsertToDatabase(MoveDTO moveDTO)
        {
            var dbConnection = new DbConnection();

            var playerRepository = new Repository<PlayerPOCO>(dbConnection);
            var player = playerRepository.GetAllAsync().Result.FirstOrDefault(player => player.PlayerGuid == moveDTO.UserId);

            player.XPosition = moveDTO.XPosition;
            player.YPosition = moveDTO.YPosition;
            player.Stamina = moveDTO.Stamina;
            playerRepository.UpdateAsync(player);
        }

        private void HandleMove(MoveDTO moveDTO)
        {
            _worldService.UpdateCharacterPosition(moveDTO.UserId, moveDTO.XPosition, moveDTO.YPosition);
            var player = _worldService.GetPlayer(moveDTO.UserId);
            player.Stamina = moveDTO.Stamina;
            _worldService.DisplayWorld();
        }
    }
}