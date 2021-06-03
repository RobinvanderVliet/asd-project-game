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
using WorldGeneration.Models.BuildingTiles;
using WorldGeneration.Models.Interfaces;


namespace ActionHandling
{
    public class MoveHandler : IMoveHandler, IPacketHandler
    {
        private readonly IClientController _clientController;
        private readonly IWorldService _worldService;

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
            bool handleInDatabase = (_clientController.IsHost() && packet.Header.Target.Equals("host")) || _clientController.IsBackupHost;
            
            if (handleInDatabase)
            {
                var player = _worldService.GetPlayer(moveDTO.UserId);
                
                var newPosPlayerX = moveDTO.XPosition;
                var newPosPlayerY = moveDTO.YPosition;
                var oldPosPlayerX = player.XPosition;
                var oldPosPlayerY = player.YPosition;
                var playerStamina = player.Stamina;
                
                _worldService.LoadArea(newPosPlayerX, newPosPlayerY, 10);
                Console.WriteLine("moveDTO: " + moveDTO.XPosition + " " + moveDTO.YPosition);
                var allTiles = GetTilesForPositions(oldPosPlayerX, oldPosPlayerY, newPosPlayerX, newPosPlayerY);

                Console.WriteLine(oldPosPlayerX + " " + oldPosPlayerY);
                foreach (var tile in allTiles)
                {
                    Console.WriteLine(tile + ": " + tile.XPosition + " " + tile.YPosition);
                }
                // List<ITile> allTilesMinusYours = allTiles;
                // allTilesMinusYours.RemoveAt(1);
                var accessibleTiles = inAccessibleTileExists(allTiles);
                var moveableTiles = GetMoveAbleTiles(allTiles, accessibleTiles);
                
                Console.WriteLine(accessibleTiles.Count);
                Console.WriteLine(moveableTiles.Count);
                for (int i = 0; i < moveableTiles.Count; i++)
                {
                    Console.WriteLine(moveableTiles[i] + ": " + moveableTiles[i].XPosition + " " + moveableTiles[i].YPosition + " Accessible: "+accessibleTiles[i]);
                }

                var staminaCost = GetStaminaCostForTiles(moveableTiles);
                
                //check if possible
                // if ()
                // {
                    // if (packet.Header.OriginID == null)
                    // {
                        // Console.WriteLine("Can't move to new position something is in the way");
                    // }

                    // return new HandlerResponseDTO(SendAction.ReturnToSender, "Can't move to new position something is in the way");
                // }
                //else
                if (staminaCost > playerStamina)
                {
                    if (packet.Header.OriginID == null)
                    {
                        Console.WriteLine("You do not have enough stamina to move!");
                    }

                    return new HandlerResponseDTO(SendAction.ReturnToSender, "You do not have enough stamina to move!");
                } 
                // else if (CheckIfPlayerOnTiles(oldPosPlayerX, oldPosPlayerY, newPosPlayerX, newPosPlayerY))
                // {
                //     if (packet.Header.OriginID == null)
                //     {
                //         Console.WriteLine("A player is on the tile");
                //     }
                //     
                //     var allTiles = GetTilesBetweenPositions(oldPosPlayerX, oldPosPlayerY, newPosPlayerX, newPosPlayerY);
                //     var moveableTiles = GetMoveAbleTiles(allTiles,inAccessibleTileExists(allTiles));
                //
                //     var staminaCost = GetStaminaCostForTiles(moveableTiles);
                //
                //     return new HandlerResponseDTO(SendAction.ReturnToSender, "A player is on the tile");
                // }
                else
                {
                    moveDTO.Stamina = playerStamina - staminaCost;
                    if (moveableTiles.Count < allTiles.Count-1)
                    {
                        Console.WriteLine("Ik kom erin");
                        if (moveableTiles.Count != 0)
                        {
                            moveDTO.XPosition = moveableTiles.Last().XPosition;
                            moveDTO.YPosition = moveableTiles.Last().YPosition;
                        }
                        else
                        {
                            moveDTO.XPosition = player.XPosition;
                            moveDTO.YPosition = player.YPosition;
                        }
                    }
                    
                    Console.WriteLine("stamina: " +moveDTO.Stamina);
                    InsertToDatabase(moveDTO);
                    HandleMove(moveDTO);
                    packet.Payload = JsonConvert.SerializeObject(moveDTO);
                    
                    return new HandlerResponseDTO(SendAction.SendToClients, null);
                }
            }
            else
            {
                HandleMove(moveDTO);
            }
            
            return new(SendAction.Ignore, null);
            
            // var moveDTO = JsonConvert.DeserializeObject<MoveDTO>(packet.Payload);

            //check for backup host like comments below
            //(_clientController.IsHost() && packet.Header.Target.Equals("host")) || _clientController.IsBackupHost)
            // if (_clientController.IsHost() && packet.Header.Target.Equals("host"))
            // {
            //     var dbConnection = new DbConnection();
            //
            //     var playerRepository = new Repository<PlayerPOCO>(dbConnection);
            //     var servicePlayer = new ServicesDb<PlayerPOCO>(playerRepository);
            //     
            //     var allLocations = servicePlayer.GetAllAsync();
            //
            //     allLocations.Wait();
            //
            //     var newPosPlayerX = moveDTO.XPosition;
            //     var newPosPlayerY = moveDTO.YPosition;
            //     
            //     var oldPosPlayerX =
            //         allLocations.Result.Where(x =>
            //             x.PlayerGuid == moveDTO.UserId
            //         ).Select(x => x.XPosition).FirstOrDefault();
            //     var oldPosPlayerY =
            //         allLocations.Result.Where(x =>
            //             x.PlayerGuid == moveDTO.UserId
            //         ).Select(x => x.YPosition).FirstOrDefault();
            //
            //     _worldService.LoadArea(newPosPlayerX, newPosPlayerY, 10);
            //
            //     var stamina = GetStaminaCostForTiles(
            //         GetTilesBetweenPositions(oldPosPlayerX, oldPosPlayerY, newPosPlayerX, newPosPlayerY));
            //
            //     var result =
            //         allLocations.Result.Where(x =>
            //             x.XPosition == newPosPlayerX && x.YPosition == newPosPlayerY &&
            //             x.GameGuid == _clientController.SessionId);
            //
            //     var resultStamina =
            //         allLocations.Result.Where(x =>
            //             x.PlayerGuid == moveDTO.UserId
            //         ).Select(x => x.Stamina).FirstOrDefault();
            //
            //     if (result.Any())
            //     {
            //         if (packet.Header.OriginID == null)
            //         {
            //             Console.WriteLine("Can't move to new position something is in the way");
            //         }
            //
            //         return new HandlerResponseDTO(SendAction.ReturnToSender, "Can't move to new position something is in the way");
            //     }
            //     else if (resultStamina < stamina)
            //     {
            //         if (packet.Header.OriginID == null)
            //         {
            //             Console.WriteLine("You do not have enough stamina to move!");
            //         }
            //
            //         return new HandlerResponseDTO(SendAction.ReturnToSender, "You do not have enough stamina to move!");
            //     }
            //     else
            //     {
            //         moveDTO.Stamina = resultStamina - stamina;
            //         InsertToDatabase(moveDTO);
            //         HandleMove(moveDTO);
            //         packet.Payload = JsonConvert.SerializeObject(moveDTO);
            //     }
            // }
            // else if (packet.Header.Target.Equals(_clientController.GetOriginId()))
            // {
            //     Console.WriteLine(packet.HandlerResponse.ResultMessage);
            // }
            // else
            // {
            //     HandleMove(moveDTO);
            // }
            //
            // return new HandlerResponseDTO(SendAction.SendToClients, null);
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
            // _worldService.UpdateCharacterPosition(moveDTO.UserId, moveDTO.XPosition, moveDTO.YPosition);
            var player = _worldService.GetPlayer(moveDTO.UserId);
            player.Stamina = moveDTO.Stamina;
            player.XPosition = moveDTO.XPosition;
            player.YPosition = moveDTO.YPosition;
            Console.WriteLine("NewPlayer: " + player.Stamina + " "+ player.XPosition + " "+ player.YPosition);
            _worldService.DisplayWorld();
        }

        private List<ITile> GetTilesForPositions(int x1, int y1, int x2, int y2)
        {
            var tiles = new List<ITile>();
 
            if (y1 != y2)
            {
                if (y2 > y1)
                {
                    for (int i = y1; i <= y2; i++)
                    {
                        tiles.Add(_worldService.GetTile(x1, i));
                    }
                }
                else
                {
                    for (int i = y1; i >= y2; i--)
                    {
                        tiles.Add(_worldService.GetTile(x1, i));
                    }
                }
            }
            else
            {
                if (x2 > x1)
                {
                    for (int i = x1; i <= x2; i++)
                    {
                        tiles.Add(_worldService.GetTile(i, y1));
                    }
                }
                else
                {
                    for (int i = x1; i >= x2; i--)
                    {
                        tiles.Add(_worldService.GetTile(i, y1));
                    }
                }
                
                // for (var i = Math.Min(x1, x2); i < Math.Max(x1, x2); i++)
                // {
                //     tiles.Add(_worldService.GetTile(i, y1+1));
                // }
            }
            return tiles;
        }
        
        // private bool CheckIfPlayerOnTiles(int x1, int y1, int x2, int y2) {
        //     var boolean = false;
        //
        //     if (x1 == x2)
        //     {
        //         for (var i = Math.Min(y1, y2); i <= Math.Max(y1, y2); i++)
        //         {
        //             if (i != y1)
        //             {
        //                 if (_worldService.CheckIfPlayerOnTile(x1, i))
        //                 {
        //                     boolean = true;
        //                 }
        //             }
        //         }
        //     }
        //     else
        //     {
        //         for (var i = Math.Min(x1, x2); i <= Math.Max(x1, x2); i++)
        //         {
        //             if (i != x1)
        //             {
        //                 if (_worldService.CheckIfPlayerOnTile(i, y1))
        //                 {
        //                     boolean = true;
        //                 }
        //             }
        //         }
        //     }
        //     
        //     return boolean;
        // }

        private int GetStaminaCostForTiles(List<ITile> tiles)
        {
            var staminaCosts = 0;
            
            foreach (var tile in tiles)
            {
                staminaCosts += tile.StaminaCost;
            }
            
            return staminaCosts;
        }

        private List<bool> inAccessibleTileExists(List<ITile> tiles)
        {
            var accessibleTiles = new List<bool>();
            for (int i = 1; i < tiles.Count; i++)
            {
                if (tiles[i].IsAccessible)
                {
                    accessibleTiles.Add(true);
                }
                else
                {
                    accessibleTiles.Add(false);
                }
            }
            return accessibleTiles;
        }

        private List<ITile> GetMoveAbleTiles(List<ITile> tiles, List<bool> accessible)
        {
            var moveableTiles = new List<ITile>();
            for (int i = 0; i < accessible.Count; i++)
            {
                Console.Write("Player: ");
                Console.WriteLine(_worldService.CheckIfPlayerOnTile(tiles[i+1]));
                if (accessible[i] && !_worldService.CheckIfPlayerOnTile(tiles[i+1]))
                {
                    moveableTiles.Add(tiles[i+1]);
                }
                else
                {
                    return moveableTiles;
                }
            }
            return moveableTiles;
        }
    }
}