using System;
using System.Collections.Generic;
using System.Linq;
using ActionHandling.DTO;
using DatabaseHandler;
using DatabaseHandler.POCO;
using DatabaseHandler.Repository;
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
        
        public void PickupItem(int index)
        {
            IList<Item> items = _worldService.GetItemsOnCurrentTile();

            try
            {
                Item item = items[index - 1];
                Console.Out.WriteLine($"Pickup index {index} {item.ItemName}");
                InventoryDTO inventoryDTO =
                    new InventoryDTO(_clientController.GetOriginId(), InventoryType.Pickup, index);
                SendInventoryDTO(inventoryDTO);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine("Number is not in search list!");
                throw;
            }
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
            // ✔ Get the player
            // ✔ Verify if the item can be picked up? Is already checked client side. So optimistic security. 
                // ✔ true => move item fom tile to inventory. 
                    // if handleDatabase => update the players inventory. update the database?
                // ✔ false => display message that item could not be picked up.
            
            Player player = _worldService.GetPlayer(inventoryDTO.UserId);
            Item item = _worldService.GetItemsOnCurrentTile(player).ElementAt(inventoryDTO.Index);
            
            if (player.Inventory.AddItem(item))
            {
                _worldService.GetItemsOnCurrentTile(player).RemoveAt(inventoryDTO.Index);
                
                if (handleInDatabase)
                {
                    
                }
                
                return new HandlerResponseDTO(SendAction.SendToClients, null);
            }
            else
            {
                return new HandlerResponseDTO(SendAction.ReturnToSender, "Could not pickup item");
            }
        }
        
        // Returns true if the given slot is occupied.
        private bool SlotOccupied(Item item)
        {
            return item != null;
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
                    var dbConnection = new DbConnection();

                    var playerRepository = new Repository<PlayerPOCO>(dbConnection);

                    var playerItemRepository = new Repository<PlayerItemPOCO>(dbConnection);

                    PlayerItemPOCO playerItemPOCO = new() {PlayerGUID = inventoryDTO.UserId, ItemName = itemToUse.ItemName };
                    playerItemRepository.DeleteAsync(playerItemPOCO);

                    PlayerPOCO playerPOCO = playerRepository.GetAllAsync().Result.FirstOrDefault(player => player.PlayerGuid == inventoryDTO.UserId);
                    playerPOCO.Health = player.Health;
                    //add stamina to playerPOCO
                    playerRepository.UpdateAsync(playerPOCO);
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
