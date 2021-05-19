using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseHandler;
using DatabaseHandler.Poco;
using DatabaseHandler.Repository;
using DatabaseHandler.Services;
using Network;
using Network.DTO;
using Newtonsoft.Json;
using Player.DTO;
using Player.Services;
using Session.DTO;

namespace Player.ActionHandlers
{
    public class MoveHandler : IMoveHandler, IPacketHandler
    {
        private IClientController _clientController;
        private PlayerDTO _currentPlayer;
        private WorldService _worldService;
        private string Game;
        private string playerGuid; 
        private Dictionary<string, int[]> _PlayerLocations  {get; set;}
 

        public MoveHandler(IClientController clientController)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.Move);
        }
        
        public void SendMove(MapCharacterDTO player, WorldService worldService)
        {
            _worldService = worldService;
            //_currentPlayer = player;
            //var playerPostionDTO = new PlayerPositionDTO(player.XPosition, player.YPosition, player.Name, player.Team);
            // var moveDTO = new MoveDTO(player);
            _currentPlayer = player;
            // var playerDTO = new PlayerDTO(player.Name, player.XPosition, player.YPosition, _clientController.GetOriginId());
            var moveDTO = new MoveDTO(playerDTO);
            SendMoveDTO(moveDTO);
        }
        
        private void SendMoveDTO(MoveDTO moveDTO)
        {
            var payload = JsonConvert.SerializeObject(moveDTO);
            _clientController.SendPayload(payload, PacketType.Move);
        }
        
        public HandlerResponseDTO HandlePacket(PacketDTO packet)
        {
            if (packet.Header.PacketType == PacketType.StartGame)
            {
                Console.WriteLine("Game started in MoveHandler :)");
            } else if (packet.Header.PacketType == PacketType.Move)
            {
             Console.WriteLine("Moved in moveHandler :)");   
            }
            //
            // var packetDTO = JsonConvert.DeserializeObject<StartGameDto>(packet.Payload);
            // var moveDTO = JsonConvert.DeserializeObject<MoveDTO>(packet.Payload);
            //
            // if (packet.Header.Target.Equals("client"))
            // {
            //     if (packetDTO != null)
            //     {
            //         _PlayerLocations = packetDTO.PlayerLocations;
            //         Game = packetDTO.GameName; 
            //     }
            // }
            //
            // if (packet.Header.Target.Equals("host"))
            // {
            //     var tmp = new DbConnection();
            //     tmp.SetForeignKeys();
            //
            //     var playerRepository = new Repository<PlayerPoco>(tmp);
            //     var tmpServicePlayer = new ServicesDb<PlayerPoco>(playerRepository);
            //     var tmpGameRepostory = new Repository<GamePoco>(tmp);
            //     var tmpServiceGame = new ServicesDb<GamePoco>(tmpGameRepostory);
            //
            //     var allLocations = playerRepository.GetAllPoco();
            //
            //     if (moveDTO != null)
            //     {
            //        int newPosPlayerX =  moveDTO.Player.X;
            //        int newPosPlayerY = moveDTO.Player.Y;
            //
            //        var result =
            //            allLocations.Result.Where(x => x.PositionX == newPosPlayerX && x.PositionY == newPosPlayerY);
            //
            //        if (result.Any())
            //        {
            //            return new HandlerResponseDTO(SendAction.Ignore, "Can't move to new position already something in the way");
            //
            //        }
            //        else
            //        {
            //      //      PlayerPoco updatedPlayerPoco = new PlayerPoco(playerGuid = _clientController.GetOriginId().ToString());
            //       //     playerRepository.UpdateAsync()
            //        }
            //     }
            //
            //  
            //
            //     // foreach(var element in packetDTO.PlayerLocations)
            //     // {
            //     //     var positions = element.Key[1];
            //     //
            //     // }
            //
            //
            //
            //
            //
            //
            //     //  allLocations.Result.Where(x => x.PositionX
            //
            //
            // }
                            //adjust playerlist
                                
                                //moet op id vergeleken worden
                                // if (moveDTO.PlayerPosition. == _currentPlayer.)
                                // {
                                    //adjust actual player
                                    // _currentPlayer.ChangePositionOfAPlayer(moveDTO.Player);
                                    
                                    //change database
                                    
                                    // CreateAsync(moveDTO.Player);
                                // }
            //
            // //Check welk actie het is:
            //
            // //check if pakketje is host: Zo ja Controlleer of het kan zert dan in database en stuur naar alle clients een bericht
            //
            //
            //
            //
            // //check if pakketje is client: Zo ja voer het uit. 
            // HandleMove(moveDTO.Player);
            return new HandlerResponseDTO(SendAction.Ignore, null);
            // return null;
        }

        private HandlerResponseDTO HandleMove(MapCharacterDTO playerPosition)
        {
            // if (player.PlayerName.Equals(_currentPlayer.GetName()))
            // {
                _worldService.UpdateCharacterPosition(playerPosition);

                // _currentPlayer.SetX(player.X);
                // _currentPlayer.SetY(player.Y);
                // Make changes in database
            // }

            // _currentPlayer.ChangePositionOfAPlayer(player);
            return new HandlerResponseDTO(SendAction.Ignore, null);
        }
        
        private void InsertToDatabase(MoveDTO moveDto)
        {
           //Get database
           //Check database
           //Insert Database
           //if inserted then send to all a message
        }
    }
}