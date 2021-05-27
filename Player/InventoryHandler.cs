using Network;
using Network.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player
{
    public class InventoryHandler : IPacketHandler, IInventoryHandler
    {
        private IClientController _clientController;

        public InventoryHandler(IClientController clientController)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.Inventory);
        }

        public void UseItem(int index)
        {
            InventoryDTO inventoryDTO = new(InventoryType.Use, index);
            SendInventoryDTO(inventoryDTO);
        }

        private void SendInventoryDTO(InventoryDTO inventoryDTO)
        {
            var payload = JsonConvert.SerializeObject(inventoryDTO);
            _clientController.SendPayload(payload, PacketType.Inventory);
        }

        public HandlerResponseDTO HandlePacket(PacketDTO packet)
        {
            var inventoryDTO = JsonConvert.DeserializeObject<InventoryDTO>(packet.Payload);

            switch (inventoryDTO.InventoryType)
            {
                case InventoryType.Use:
                    return HandleUse(inventoryDTO);
                case InventoryType.Drop:
                    return HandleDrop(inventoryDTO);
                case InventoryType.Pickup:
                    return HandlePickup(inventoryDTO);
            }
            return new(SendAction.Ignore, null);
        }

        private HandlerResponseDTO HandlePickup(InventoryDTO inventoryDTO)
        {
            throw new NotImplementedException();
        }

        private HandlerResponseDTO HandleDrop(InventoryDTO inventoryDTO)
        {
            throw new NotImplementedException();
        }

        private HandlerResponseDTO HandleUse(InventoryDTO inventoryDTO)
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
