using Network;
﻿using System;
using System.Collections.Generic;
using System.Linq;
using ActionHandling.DTO;
using DatabaseHandler;
using DatabaseHandler.POCO;
using DatabaseHandler.Repository;
using DatabaseHandler.Services;
using Items;
using Network.DTO;
using Newtonsoft.Json;
using WorldGeneration;


namespace ActionHandling
{
    public class SpawnHandler : ISpawnHandler, IPacketHandler
    {
        private IClientController _clientController;
        private IWorldService _worldService;

        public SpawnHandler(IClientController clientController, IWorldService worldService)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.Spawn);
            _worldService = worldService;
        }

        public void SendSpawn(int x, int y, Item item)
        {

            ItemSpawnDTO itemSpawnDto = new ItemSpawnDTO();
            itemSpawnDto.XPosition = x;
            itemSpawnDto.YPosition = y;
            itemSpawnDto.item = item;
            SendSpawnDTO(itemSpawnDto);
        }

        private void SendSpawnDTO(ItemSpawnDTO itemSpawnDto)
        {
            var payload = JsonConvert.SerializeObject(itemSpawnDto);
            _clientController.SendPayload(payload, PacketType.Spawn);
        }

        public HandlerResponseDTO HandlePacket(PacketDTO packet)
        {
            var itemSpawnDto = JsonConvert.DeserializeObject<ItemSpawnDTO>(packet.Payload);
            
            if (_clientController.IsHost() && packet.Header.Target.Equals("host"))
            {
                ItemSpawnDTO item = _worldService.getAllItems()
                    .FirstOrDefault(item => item.Equals(itemSpawnDto));
                if (item == null)
                {
                    InsertToDatabase(itemSpawnDto);
                }
                else
                {
                    return new HandlerResponseDTO(SendAction.Ignore, null);
                }
            }
            else
            {
                HandleItemSpawn(itemSpawnDto);
            }

            return new HandlerResponseDTO(SendAction.SendToClients, null);
        }

        private void InsertToDatabase(ItemSpawnDTO itemSpawnDto)
        {
            var ItemService = new DatabaseService<ItemPoco>();
            var item = new ItemPoco()
                {ItemName = itemSpawnDto.item.ItemName, Xposition = itemSpawnDto.XPosition, Yposition = itemSpawnDto.YPosition};
            ItemService.CreateAsync(item);
        }

        private void HandleItemSpawn(ItemSpawnDTO itemSpawnDto)
        {
            // _worldService.UpdateCharacterPosition(moveDTO.UserId, moveDTO.XPosition, moveDTO.YPosition);
            _worldService.DisplayWorld();
        }
    }
}