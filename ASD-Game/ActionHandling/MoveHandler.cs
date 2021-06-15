using System;
using System.Collections.Generic;
using System.Linq;
using ASD_Game.ActionHandling.DTO;
using ASD_Game.DatabaseHandler.POCO;
using ASD_Game.Network;
using ASD_Game.Network.DTO;
using ASD_Game.Network.Enum;
using ASD_Game.World.Models.Characters;
using ASD_Game.World.Models.Interfaces;
using ASD_Game.World.Services;
using Newtonsoft.Json;
using System.Timers;
using ASD_Game.DatabaseHandler.Services;
using ASD_Game.Messages;
using System;
using System.Diagnostics.CodeAnalysis;

namespace ASD_Game.ActionHandling
{
    public class MoveHandler : IMoveHandler, IPacketHandler
    {
        private readonly IClientController _clientController;
        private readonly IWorldService _worldService;
        private readonly IDatabaseService<PlayerPOCO> _playerDatabaseService;
        private readonly IMessageService _messageService;
        private Timer AIUpdateTimer;
        private int _updateTime = 10000; // Smartmonster timer

        private List<MoveDTO> _AIMoves = new List<MoveDTO>();

        public MoveHandler(IClientController clientController, IWorldService worldService, IDatabaseService<PlayerPOCO> playerDatabaseService, IMessageService messageService)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.Move);
            _worldService = worldService;
            _playerDatabaseService = playerDatabaseService;
            _messageService = messageService;
        }

        public void SendMove(string directionValue, int stepsValue)
        {
            if (_worldService.IsDead(_worldService.GetCurrentPlayer()))
            {
                _messageService.AddMessage("You can't move, you're dead!");
                return;
            }

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
            var handleInDatabase = (_clientController.IsHost() && packet.Header.Target.Equals("host")) || _clientController.IsBackupHost;
            
            if (moveDTO.UserId.StartsWith("monst"))
            {
                return ChangeAIPosition(moveDTO);
            } 
            if (packet.Header.Target == "host" || packet.Header.Target == "client")
            {
                return HandleMove(moveDTO, handleInDatabase);
            }
            _messageService.AddMessage(packet.HandlerResponse.ResultMessage);
            
            return new(SendAction.Ignore, null);
        }

        private HandlerResponseDTO HandleMove(MoveDTO moveDTO, bool handleInDatabase)
        {
            if (_worldService.GetPlayer(moveDTO.UserId) != null)
            {
                var player = _worldService.GetPlayer(moveDTO.UserId);
                var newPosPlayerX = moveDTO.XPosition;
                var newPosPlayerY = moveDTO.YPosition;
                var oldPosPlayerX = player.XPosition;
                var oldPosPlayerY = player.YPosition;
                var playerStamina = player.Stamina;

                _worldService.LoadArea(newPosPlayerX, newPosPlayerY, 10);
                var allTiles = GetTilesForPositions(oldPosPlayerX, oldPosPlayerY, newPosPlayerX, newPosPlayerY);
                var accessibleTiles = inAccessibleTileExists(allTiles);
                var movableTiles = GetMovableTiles(allTiles, accessibleTiles);
                var staminaCost = GetStaminaCostForTiles(movableTiles);

                if (staminaCost > playerStamina)
                {
                    moveDTO = ChangeMoveDTOToNewLocation(moveDTO, movableTiles, player);

                    if (moveDTO.UserId == _clientController.GetOriginId())
                    {
                        _messageService.AddMessage("You do not have enough stamina to move!");
                    }

                    return new HandlerResponseDTO(SendAction.ReturnToSender, "You do not have enough stamina to move!");
                }
                
                string resultMessage = null;
                moveDTO.Stamina = playerStamina - staminaCost;
                //allTiles.Count-1 because the maximum count movableTiles always has 1 less tile, since it doesn't contain the first tile
                if (movableTiles.Count < allTiles.Count - 1)
                {
                    moveDTO = ChangeMoveDTOToNewLocation(moveDTO, movableTiles, player);
                    resultMessage = "You could not move to the next tile.";

                    if (moveDTO.UserId == _clientController.GetOriginId())
                    {
                        _messageService.AddMessage(resultMessage);
                    }
                }

                ChangePlayerPosition(moveDTO);

                if (handleInDatabase)
                {
                    InsertToDatabase(moveDTO);
                }

                return new HandlerResponseDTO(SendAction.SendToClients, resultMessage);
            }
            _AIMoves.Add(moveDTO);
            return new HandlerResponseDTO(SendAction.SendToClients, null);
        }

        private void InsertToDatabase(MoveDTO moveDTO)
        {
            var player = _playerDatabaseService.GetAllAsync().Result.FirstOrDefault(player => player.PlayerGUID == moveDTO.UserId && player.GameGUID == _clientController.SessionId);
            player.XPosition = moveDTO.XPosition;
            player.YPosition = moveDTO.YPosition;
            player.Stamina = moveDTO.Stamina;
            _playerDatabaseService.UpdateAsync(player);
        }

        private MoveDTO ChangeMoveDTOToNewLocation(MoveDTO moveDTO, List<ITile> movableTiles, Player player)
        {
            if (movableTiles.Any())
            {
                moveDTO.XPosition = movableTiles.Last().XPosition;
                moveDTO.YPosition = movableTiles.Last().YPosition;
            }
            else
            {
                moveDTO.XPosition = player.XPosition;
                moveDTO.YPosition = player.YPosition;
            }

            return moveDTO;
        }

        private void ChangePlayerPosition(MoveDTO moveDTO)
        {
            var player = _worldService.GetPlayer(moveDTO.UserId);
            player.Stamina = moveDTO.Stamina;
            player.XPosition = moveDTO.XPosition;
            player.YPosition = moveDTO.YPosition;
            _worldService.DisplayStats();
            _worldService.DisplayWorld();
        }
        
        private HandlerResponseDTO ChangeAIPosition(MoveDTO moveDTO)
        {
            var character = _worldService.GetAI(moveDTO.UserId);
            if (character != null)
            {
                character.XPosition = moveDTO.XPosition;
                character.YPosition = moveDTO.YPosition;
            }
            else
            {
                return new HandlerResponseDTO(SendAction.ReturnToSender, "Character doesn't exist");
            }
            _worldService.DisplayWorld();
            return new HandlerResponseDTO(SendAction.SendToClients, null);
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

        private List<bool> inAccessibleTileExists(List<ITile> tiles)
        {
            var accessibleTiles = new List<bool>();
            //i=1 because we want to skip the first tile, since the player is standing on that tile and it doesn't matter if that tile is accessible.
            for (int i = 1; i < tiles.Count; i++)
            {
                accessibleTiles.Add(tiles[i].IsAccessible);
            }
            return accessibleTiles;
        }

        private List<ITile> GetMovableTiles(List<ITile> tiles, List<bool> accessible)
        {
            var movableTiles = new List<ITile>();
            for (int i = 0; i < accessible.Count; i++)
            {
                //tiles[i+1] since we don't want to check the first tile, since the player is standing on that tile and the accessible list has 1 tile less than list tiles
                if (accessible[i] && !_worldService.CheckIfCharacterOnTile(tiles[i + 1]))
                {
                    movableTiles.Add(tiles[i + 1]);
                }
                else
                {
                    break;
                }
            }
            return movableTiles;
        }

        private void MoveAIs(List<Character> creatureMoves)
        {
            var _creatureMoves = creatureMoves;
            if (creatureMoves != null)
            {
                foreach (Character move in _creatureMoves)
                {
                    if (move is SmartMonster smartMonster)
                    {
                        if (smartMonster.MoveType == "Move")
                        {
                            MoveDTO moveDTO = new(smartMonster.Id, (int)smartMonster.Destination.X, (int)smartMonster.Destination.Y);
                            SendMoveDTO(moveDTO);
                        }
                    }
                }
            }
        }

        private void GetAIMoves()
        {
            MoveAIs(_worldService.GetCreatureMoves("Move"));
        }

        [ExcludeFromCodeCoverage]
        public void CheckAITimer()
        {
            AIUpdateTimer = new Timer(_updateTime);
            AIUpdateTimer.AutoReset = true;
            AIUpdateTimer.Elapsed += CheckAITimerEvent;
            AIUpdateTimer.Start();
        }

        [ExcludeFromCodeCoverage]
        private void CheckAITimerEvent(object sender, ElapsedEventArgs e)
        {
            AIUpdateTimer.Stop();
            GetAIMoves();
            AIUpdateTimer.Start();
        }

        public void SearchNearestPlayer()
        {
            var currentPlayer = _worldService.GetCurrentPlayer();

            if (_worldService.IsDead(currentPlayer))
            {
                _messageService.AddMessage("You can't look for another player, you're dead!");
                return;
            }

            int minDistance = 0;
            Player closestPlayer = null;

            foreach (var player in _worldService.GetAllPlayers())
            {
                if (player.Id == currentPlayer.Id || player.Health == 0)
                {
                    continue;
                }

                int distance = Math.Abs(currentPlayer.XPosition - player.XPosition) + Math.Abs(currentPlayer.YPosition - player.YPosition);

                if (minDistance == 0 || distance < minDistance)
                {
                    minDistance = distance;
                    closestPlayer = player;
                }
            }

            if (closestPlayer != null)
            {
                int x = currentPlayer.XPosition - closestPlayer.XPosition;
                int y = currentPlayer.YPosition - closestPlayer.YPosition;

                var xDirection = x > 0 ? "left" : "right";
                var yDirection = y > 0 ? "down" : "up";

                x = Math.Abs(x);
                y = Math.Abs(y);

                var xTiles = $"{x} tile{(x != 1 ? "s" : "")} {xDirection}";
                var yTiles = $"{y} tile{(y != 1 ? "s" : "")} {yDirection}";
                
                if (x == 0)
                {
                    _messageService.AddMessage($"The closest player is {yTiles}.");
                }
                else if (y == 0)
                {
                    _messageService.AddMessage($"The closest player is {xTiles}.");
                }
                else
                {
                    _messageService.AddMessage($"The closest player is {xTiles} and {yTiles}.");
                }
            }
            else
            {
                _messageService.AddMessage("That is strange, there are no other living players left...");
            }
        }
    }
}