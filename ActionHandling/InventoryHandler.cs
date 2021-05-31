using Network;
using Network.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Items;
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

        public void Search()
        {
            string searchResult = _worldService.SearchCurrentTile();
        }

        public void InspectItem(string slot)
        {
            var inventory = _worldService.getCurrentPlayer().Inventory;
            Item inventoryItem = null;
            
            switch (slot)
            {
                case "helmet":
                    inventoryItem = inventory.Helmet;
                    break;
                case "armor":
                    inventoryItem = inventory.Armor;
                    break;
                case "weapon":
                    inventoryItem = inventory.Weapon;
                    break;
                case "slot 1":
                    inventoryItem = inventory.ConsumableItemList[0];
                    break;
                case "slot 2":
                    inventoryItem = inventory.ConsumableItemList[1];
                    break;
                case "slot 3":
                    inventoryItem = inventory.ConsumableItemList[2];
                    break;
            }

            if (inventoryItem != null)
            {
                Console.WriteLine(inventoryItem.ToString());
            }
            else
            {
                Console.WriteLine("No item in this inventory slot");
            }
        }

        public HandlerResponseDTO HandlePacket(PacketDTO packet)
        {
            throw new NotImplementedException();
        }
    }
}
