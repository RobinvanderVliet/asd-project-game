using System.Diagnostics.Eventing.Reader;
using DataTransfer.DTO.Character;
using DataTransfer.DTO.Player;
using Network;
﻿using System;
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
using WorldGeneration;
using Player.Model;
using Player.Services;
using Session.DTO;

namespace Player.ActionHandlers
{
    public class MoveHandler : IMoveHandler, IPacketHandler
    {
        private IClientController _clientController;
        private IPlayerModel _currentPlayer;
        private string Game;
        private string playerGuid; 
        private Dictionary<string, int[]> _PlayerLocations  {get; set;}
        private IWorldService _worldService;

        public MoveHandler(IClientController clientController)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.Move);
            // _worldService = worldService;
        }

        // public void SendMove(MapCharacterDTO player, IWorldService worldService)
        // {
        //     _worldService = worldService;
        //     //_currentPlayer = player;
        //     //var playerPostionDTO = new PlayerPositionDTO(player.XPosition, player.YPosition, player.Name, player.Team);
        //     var moveDTO = new MoveDTO(player);
        //     SendMoveDTO(moveDTO);
        // } 
       
        private void SendMoveDTO(MoveDTO moveDTO)
        {
            var payload = JsonConvert.SerializeObject(moveDTO);
            _clientController.SendPayload(payload, PacketType.Move);
        }
        
        public HandlerResponseDTO HandlePacket(PacketDTO packet)
        {
            var moveDTO = JsonConvert.DeserializeObject<MoveDTO>(packet.Payload);
            HandleMove(moveDTO.PlayerPosition);
            
            if (packet.Header.PacketType == PacketType.Move)
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
        }

        private void InsertToDatabase(MoveDTO moveDto)
        {
           //Get database
           //Check database
           //Insert Database
           //if inserted then send to all a message
           
            
            
        }
                
       private void HandleMove(MapCharacterDTO playerPosition)
       {
           _worldService.UpdateCharacterPosition(playerPosition);
           
           
           // worldService.updateArraylistposition(player, x, y);
           
           // aanroepen daadwerkelijke functie voor aanpassen x en y in wereld (dus in arraylist)
           //_player.ChangePositionOfAPlayer(player);
           // als host dan in globale db aanpassen voor die speler (hostcontoller (HandlePacket))
           
         
       }
        
    }
}