using System;
using System.Linq;
using ActionHandling.DTO;
using DatabaseHandler.POCO;
using DatabaseHandler.Services;
using Items;
using Items.Consumables;
using Network;
using Network.DTO;
using Newtonsoft.Json;
using WorldGeneration;

namespace ActionHandling
{
    public class InventoryHandler : IPacketHandler, IInventoryHandler
    {
        private readonly IClientController _clientController;
        private readonly IWorldService _worldService;
        private readonly IServicesDb<PlayerPOCO> _playerServicesDB;
        private readonly IServicesDb<PlayerItemPOCO> _playerItemServicesDB;

        public InventoryHandler(IClientController clientController, IWorldService worldService, IServicesDb<PlayerPOCO> playerServicesDB, IServicesDb<PlayerItemPOCO> playerItemServicesDB)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.Inventory);
            _worldService = worldService;
            _playerServicesDB = playerServicesDB;
            _playerItemServicesDB = playerItemServicesDB;
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
        
        public void PickupItem(int index)
        {
            // Compensate for index starting at 0.
            index -= 1;

            InventoryDTO inventoryDTO =
                new InventoryDTO(_clientController.GetOriginId(), InventoryType.Pickup, index);
            SendInventoryDTO(inventoryDTO);
        }

        public void DropItem(int index)
        {
            throw new NotImplementedException();
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
            Player player = _worldService.GetPlayer(inventoryDTO.UserId);
            Item item;
            
            try
            {
                item = _worldService.GetItemsOnCurrentTile(player).ElementAt(inventoryDTO.Index);
            }
            catch (ArgumentOutOfRangeException)
            {
                return new HandlerResponseDTO(SendAction.ReturnToSender, "Number is not in search list!");
            }
            
            if (player.Inventory.AddItem(item))
            {
                _worldService.GetItemsOnCurrentTile(player).RemoveAt(inventoryDTO.Index);
                
                if (handleInDatabase)
                {
                    PlayerItemPOCO playerItemPOCO = new PlayerItemPOCO {PlayerGUID = inventoryDTO.UserId, ItemName = item.ItemName};
                    _playerItemServicesDB.CreateAsync(playerItemPOCO);
                }
                
                return new HandlerResponseDTO(SendAction.SendToClients, null);
            }
            else
            {
                return new HandlerResponseDTO(SendAction.ReturnToSender, "Could not pickup item");
            }
        }

        private HandlerResponseDTO HandleDrop(InventoryDTO inventoryDTO, bool handleInDatabase)
        {
            throw new NotImplementedException();
        }

        private HandlerResponseDTO HandleUse(InventoryDTO inventoryDTO, bool handleInDatabase)
        {
            var player = _worldService.GetPlayer(inventoryDTO.UserId);
            if(player.Inventory.ConsumableItemList.Count >= inventoryDTO.Index)
            {
                Consumable itemToUse = player.Inventory.ConsumableItemList.ElementAt(inventoryDTO.Index-1);
                player.Inventory.ConsumableItemList.RemoveAt(inventoryDTO.Index-1);
                player.UseConsumable(itemToUse);

                if (handleInDatabase)
                {
                    PlayerItemPOCO playerItemPOCO = new() {PlayerGUID = inventoryDTO.UserId, ItemName = itemToUse.ItemName, GameGuid = _clientController.SessionId };
                    _ = _playerItemServicesDB.DeleteAsync(playerItemPOCO);

                    var result = _playerServicesDB.GetAllAsync().Result;
                    PlayerPOCO playerPOCO = result.FirstOrDefault(player => player.PlayerGuid == inventoryDTO.UserId && player.GameGuid == _clientController.SessionId );
                    
                    playerPOCO.Health = player.Health;
                    //add stamina to playerPOCO
                    _ = _playerServicesDB.UpdateAsync(playerPOCO);
                }
                return new HandlerResponseDTO(SendAction.SendToClients, null);
            }
            else
            {
                if(inventoryDTO.UserId == _clientController.GetOriginId())
                {
                    Console.WriteLine("Could not find item");
                }
                return new HandlerResponseDTO(SendAction.ReturnToSender, "Could not find item");
            }
        }
    }
}
