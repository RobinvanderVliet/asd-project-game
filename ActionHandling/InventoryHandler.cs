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

        public void InspectItem(string slot)
        {
            throw new NotImplementedException("TODO");
        }

        public HandlerResponseDTO HandlePacket(PacketDTO packet)
        {
            throw new NotImplementedException();
        }
    }
}
