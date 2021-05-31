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
            
            Item inventoryItem = slot switch
            {
                "helmet" => inventory.Helmet,
                "armor" => inventory.Armor,
                "weapon" => inventory.Weapon,
                "slot 1" => inventory.ConsumableItemList[0],
                "slot 2" => inventory.ConsumableItemList[1],
                "slot 3" => inventory.ConsumableItemList[2],
                _ => null
            };

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
