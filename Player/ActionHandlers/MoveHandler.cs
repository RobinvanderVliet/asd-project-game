using DataTransfer.DTO.Character;
using Network;
﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DatabaseHandler;
using DatabaseHandler.POCO;
using DatabaseHandler.Repository;
using DatabaseHandler.Services;
using Network;
using Network.DTO;
using Newtonsoft.Json;
using Player.DTO;
using WorldGeneration;
using Player.Model;


namespace Player.ActionHandlers
{
    public class MoveHandler : IMoveHandler, IPacketHandler
    {
        private IClientController _clientController;
        private IPlayerModel _currentPlayer;
        private string _game;
        private string _playerGuid;
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

            //check for backup host like comments below
            //(_clientController.IsHost() && packet.Header.Target.Equals("host")) || _clientController.IsBackupHost)
            if (_clientController.IsHost() && packet.Header.Target.Equals("host"))
            {
                var dbConnection = new DbConnection();

                var playerRepository = new Repository<PlayerPOCO>(dbConnection);
                var servicePlayer = new ServicesDb<PlayerPOCO>(playerRepository);

                var allLocations = servicePlayer.GetAllAsync();

                allLocations.Wait();

                int newPosPlayerX = moveDTO.PlayerPosition.XPosition;
                int newPosPlayerY = moveDTO.PlayerPosition.YPosition;

                var result =
                    allLocations.Result.Where(x =>
                        x.XPosition == newPosPlayerX && x.YPosition == newPosPlayerY &&
                        x.GameGuid == moveDTO.PlayerPosition.GameGuid);

                var resultStamina =
                    allLocations.Result.Where(x =>
                        x.GameGuid == moveDTO.PlayerPosition.GameGuid &&
                        x.PlayerGuid == moveDTO.PlayerPosition.PlayerGuid
                    ).Select(x => x.Stamina).FirstOrDefault();
                
                if (result.Any())
                {
                    return new HandlerResponseDTO(SendAction.ReturnToSender, "Can't move to new position something is in the way");
                }
                else if (resultStamina < moveDTO.PlayerPosition.Steps)
                {
                    return new HandlerResponseDTO(SendAction.ReturnToSender, "You do not have enough stamina to move!");
                }
                else
                {
                    moveDTO.PlayerPosition.Stamina = resultStamina - moveDTO.PlayerPosition.Steps;
                    
                    InsertToDatabase(moveDTO);
                    HandleMove(moveDTO.PlayerPosition);
                }
            }
            else if (packet.Header.Target.Equals(_clientController.GetOriginId()))
            {
                Console.WriteLine(packet.HandlerResponse.ResultMessage);
            }
            else
            {
                HandleMove(moveDTO.PlayerPosition);
            }
            
            return new HandlerResponseDTO(SendAction.SendToClients, null);
        }

        private void InsertToDatabase(MoveDTO moveDTO)
        {
            var dbConnection = new DbConnection();

            var playerRepository = new Repository<PlayerPOCO>(dbConnection);
            var servicePlayer = new ServicesDb<PlayerPOCO>(playerRepository);

            var destination = _mapper.Map<PlayerPOCO>(moveDTO.PlayerPosition);

            if (servicePlayer.UpdateAsync(destination).Result == 1)
            {
                //TODO: check if successful or not
            }
        }

        private void HandleMove(MapCharacterDTO playerPosition)
        {
            _worldService.UpdateCharacterPosition(playerPosition);
            _worldService.DisplayWorld();
        }
    }
}