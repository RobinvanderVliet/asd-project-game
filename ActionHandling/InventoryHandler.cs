using ActionHandling.DTO;
using Network;
using Network.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration;

namespace ActionHandling
{
    public class InventoryHandler : IPacketHandler, IInventoryHandler
    {
        private IClientController _clientController;
        private IWorldService _worldService;

        public InventoryHandler(IClientController clientController, IWorldService worldService)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.Inventory);
            _worldService = worldService;
        }

        public void UseItem(int index)
        {
            InventoryDTO inventoryDTO = new(_clientController.GetOriginId(), InventoryType.Use, index);
            SendInventoryDTO(inventoryDTO);
        }

        public void Search()
        {
            string searchResult = _worldService.SearchCurrentTile();
            Console.WriteLine(searchResult);
        }

        private void SendInventoryDTO(InventoryDTO inventoryDTO)
        {
            var payload = JsonConvert.SerializeObject(inventoryDTO);
            _clientController.SendPayload(payload, PacketType.Inventory);
        }

        public HandlerResponseDTO HandlePacket(PacketDTO packet)
        {
            var inventoryDTO = JsonConvert.DeserializeObject<InventoryDTO>(packet.Payload);
            bool handleInDatabase = (_clientController.IsHost() && packet.Header.Target.Equals("host")) || _clientController.IsBackupHost;


            switch (inventoryDTO.InventoryType)
            {
                case InventoryType.Use:
                    return HandleUse(inventoryDTO, handleInDatabase);
                case InventoryType.Drop:
                    return HandleDrop(inventoryDTO, handleInDatabase);
                case InventoryType.Pickup:
                    return HandlePickup(inventoryDTO, handleInDatabase);
            }
            return new(SendAction.Ignore, null);
        }

        private HandlerResponseDTO HandlePickup(InventoryDTO inventoryDTO, bool handleInDatabase)
        {
            throw new NotImplementedException();
        }

        private HandlerResponseDTO HandleDrop(InventoryDTO inventoryDTO, bool handleInDatabase)
        {
            throw new NotImplementedException();
        }

        private HandlerResponseDTO HandleUse(InventoryDTO inventoryDTO, bool handleInDatabase)
        {
            bool handleInWorld = true;
            if (handleInDatabase)
            {
                handleInWorld = HandleUseInDatabase(inventoryDTO.UserId, inventoryDTO.Index);
            }

            if (handleInWorld)
            {
                HandleUseInWorld(inventoryDTO.UserId, inventoryDTO.Index);
                return new HandlerResponseDTO(SendAction.SendToClients, null);
            }
            else
            {
                return new HandlerResponseDTO(SendAction.ReturnToSender, "Could not use item");
            }

        }

        private void HandleUseInWorld(string userId, int index)
        {
            throw new NotImplementedException();
        }

        private bool HandleUseInDatabase(string userId, int index)
        {
            throw new NotImplementedException();
        }

        public void PickupItem(int index)
        {
            throw new NotImplementedException();
        }

        public void DropItem(int index)
        {
            throw new NotImplementedException();
        }
    }
}
