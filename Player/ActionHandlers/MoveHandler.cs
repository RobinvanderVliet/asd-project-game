using System.Diagnostics.Eventing.Reader;
using DataTransfer.DTO.Character;
using DataTransfer.DTO.Player;
using Network;
﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
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
        private Dictionary<string, int[]> _PlayerLocations { get; set; }
        private IWorldService _worldService;
        private IMapper _mapper;

        public MoveHandler(IMapper mapper, IClientController clientController, IWorldService worldService)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.Move);
            _worldService = worldService;
            _mapper = mapper;
        }

        public void SendMove(MapCharacterDTO player)
        {
  
            // _currentPlayer = player;
            // var playerPostionDTO = new PlayerPositionDTO(player.XPosition, player.YPosition, player.Name, player.Team);
            var moveDTO = new MoveDTO(player);
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
          
            if (_clientController.IsHost())
            {
                var tmp = new DbConnection();

                var playerRepository = new Repository<PlayerPoco>(tmp);
                var tmpServicePlayer = new ServicesDb<PlayerPoco>(playerRepository);
                var tmpGameRepostory = new Repository<GamePoco>(tmp);
                var tmpServiceGame = new ServicesDb<GamePoco>(tmpGameRepostory);

                var allLocations = tmpServicePlayer.GetAllAsync();

                allLocations.Wait();
                
                int newPosPlayerX = moveDTO.PlayerPosition.XPosition;
                int newPosPlayerY = moveDTO.PlayerPosition.YPosition;

                var result =
                    allLocations.Result.Where(x => x.XPosition == newPosPlayerX && x.YPosition == newPosPlayerY && x.GameGuid == moveDTO.PlayerPosition.GameGuid);

                
                
                if (result.Any())
                {
                    return new HandlerResponseDTO(SendAction.Ignore,
                        "Can't move to new position something is in the way");
                }
                else
                {
                    InsertToDatabase(moveDTO);
                    HandleMove(moveDTO.PlayerPosition);
                }

            }
            
            HandleMove(moveDTO.PlayerPosition);

            return new HandlerResponseDTO(SendAction.SendToClients, null);
        }

        private void InsertToDatabase(MoveDTO moveDto)
        {
            var tmp = new DbConnection();

            var playerRepository = new Repository<PlayerPoco>(tmp);
            var tmpServicePlayer = new ServicesDb<PlayerPoco>(playerRepository);
            var tmpGameRepostory = new Repository<GamePoco>(tmp);
            var tmpServiceGame = new ServicesDb<GamePoco>(tmpGameRepostory);
            
            var destination = _mapper.Map<PlayerPoco>(moveDto.PlayerPosition);
              
            
          //  playerRepository.UpdateAsync(moveDto.PlayerPosition);
            if (playerRepository.UpdateAsync(destination).Result == 1)

            {
                var allPlayers = tmpServicePlayer.GetAllAsync();
                allPlayers.Wait();
                Console.WriteLine("Updated :)");
             //   SendMove(moveDto.PlayerPosition);
            }
        }
                
       private void HandleMove(MapCharacterDTO playerPosition) // hiervan move DTO maken
       {
           _worldService.UpdateCharacterPosition(playerPosition);
           _worldService.DisplayWorld();
           
           //_player.ChangePositionOfAPlayer(player);
           // als host dan in globale db aanpassen voor die speler (hostcontoller (HandlePacket))
           
         
       }
        
    }
}