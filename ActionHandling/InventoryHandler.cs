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
                    //Drop armor on current tile
                    _worldService.getCurrentPlayer().Inventory.Armor = null;
                    break;
                case "helmet":
                    Console.WriteLine("Case Helmet");
                    //Drop armor on current tile
                    _worldService.getCurrentPlayer().Inventory.Helmet = null;
                    break;
                case "weapon":
                    Console.WriteLine("Case Weapon");
                    //Drop armor on current tile
                    _worldService.getCurrentPlayer().Inventory.Weapon = null;
                    break;
                case "1":
                    Console.WriteLine("Case Item slot 1");
                    //Drop armor on current tile
                    _worldService.getCurrentPlayer().Inventory.RemoveConsumableItem(null);
                    break;
                case "2":
                    Console.WriteLine("Case Item slot 2");
                    //Drop armor on current tile
                    _worldService.getCurrentPlayer().Inventory.RemoveConsumableItem(null);
                    break;
                case "3":
                    Console.WriteLine("Case Item slot 3");
                    //Drop armor on current tile
                    _worldService.getCurrentPlayer().Inventory.RemoveConsumableItem(null);
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
