using Network;
using Network.DTO;
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

        public void Search()
        {
            string searchResult = _worldService.SearchCurrentTile();
        }

        public void DropItem(string inventorySlot)
        {
            switch (inventorySlot)
            {
                case "armor":
                    Console.WriteLine("Case Armor");
                    _worldService.DropItemOnTile(_worldService.getCurrentPlayer().Inventory.Armor);
                    _worldService.getCurrentPlayer().Inventory.Armor = null;
                    break;
                case "helmet":
                    Console.WriteLine("Case Helmet");
                    _worldService.DropItemOnTile(_worldService.getCurrentPlayer().Inventory.Helmet);
                    _worldService.getCurrentPlayer().Inventory.Helmet = null;
                    break;
                case "weapon":
                    Console.WriteLine("Case Weapon");
                    _worldService.DropItemOnTile(_worldService.getCurrentPlayer().Inventory.Weapon);
                    _worldService.getCurrentPlayer().Inventory.Weapon = null;
                    break;
                case "item":
                    Console.WriteLine("Case Consumable Item");
                    _worldService.DropItemOnTile(_worldService.getCurrentPlayer().Inventory.GetConsumableItem(inventorySlot));
                    _worldService.getCurrentPlayer().Inventory.RemoveConsumableItem(_worldService.getCurrentPlayer().Inventory.GetConsumableItem(inventorySlot));
                    break;
                default:
                    Console.WriteLine("Unknown inventory slot");
                    break;
            }
        }



        public HandlerResponseDTO HandlePacket(PacketDTO packet)
        {
            throw new NotImplementedException();
        }
    }
}
