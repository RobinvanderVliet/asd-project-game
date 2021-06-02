using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using ActionHandling.DTO;
using DatabaseHandler;
using DatabaseHandler.POCO;
using DatabaseHandler.Repository;
using DatabaseHandler.Services;
using Network.DTO;
using Newtonsoft.Json;
using WorldGeneration;
using WorldGeneration.Models.Interfaces;


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
                
                var allLocations = servicePlayer.GetAllAsync();

                allLocations.Wait();

                var newPosPlayerX = moveDTO.XPosition;
                var newPosPlayerY = moveDTO.YPosition;
                
                var oldPosPlayerX =
                    allLocations.Result.Where(x =>
                        x.PlayerGuid == moveDTO.UserId
                    ).Select(x => x.XPosition).FirstOrDefault();
                var oldPosPlayerY =
                    allLocations.Result.Where(x =>
                        x.PlayerGuid == moveDTO.UserId
                    ).Select(x => x.YPosition).FirstOrDefault();

                _worldService.LoadArea(newPosPlayerX, newPosPlayerY, 10);

                var stamina = GetStaminaCostForTiles(
                    GetTilesBetweenPositions(oldPosPlayerX, oldPosPlayerY, newPosPlayerX, newPosPlayerY));

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
                    if (packet.Header.OriginID == null)
                    {
                        Console.WriteLine("Can't move to new position something is in the way");
                    }

                    return new HandlerResponseDTO(SendAction.ReturnToSender, "Can't move to new position something is in the way");
                }
                else if (resultStamina < stamina)
                {
                    if (packet.Header.OriginID == null)
                    {
                        Console.WriteLine("You do not have enough stamina to move!");
                    }

                    return new HandlerResponseDTO(SendAction.ReturnToSender, "You do not have enough stamina to move!");
                }
                else
                {
                    moveDTO.Stamina = resultStamina - stamina;
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

        private List<ITile> GetTilesBetweenPositions(int x1, int y1, int x2, int y2)
        {
            var tiles = new List<ITile>();
 
            if (x1 == x2)
            {
                for (var i = Math.Min(y1, y2); i < Math.Max(y1, y2); i++)
                {
                    tiles.Add(_worldService.GetTile(x1, i));
                }
            }
            else
            {
                for (var i = Math.Min(x1, x2); i < Math.Max(x1, x2); i++)
                {
                    tiles.Add(_worldService.GetTile(i, y1));
                }
            }

            return tiles;
        }

        private int GetStaminaCostForTiles(List<ITile> tiles)
        {
            var staminaCosts = 0;

            foreach (var tile in tiles)
            {
                staminaCosts += tile.StaminaCost;
            }

            return staminaCosts;
        }
    }
}