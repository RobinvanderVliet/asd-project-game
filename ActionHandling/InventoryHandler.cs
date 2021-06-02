using ActionHandling.DTO;
using DatabaseHandler;
using DatabaseHandler.POCO;
using DatabaseHandler.Repository;
using DatabaseHandler.Services;
using Items.Consumables;
using Messages;
using Network;
using Network.DTO;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Items;
using WorldGeneration;

namespace ActionHandling
{
    public class InventoryHandler : IPacketHandler, IInventoryHandler
    {
        private readonly IClientController _clientController;
        private readonly IWorldService _worldService;
        private readonly IMessageService _messageService;
        private readonly IServicesDb<PlayerPOCO> _playerServicesDB;
        private readonly IServicesDb<PlayerItemPOCO> _playerItemServicesDB;


        public InventoryHandler(IClientController clientController, IWorldService worldService, IServicesDb<PlayerPOCO> playerServicesDB, IServicesDb<PlayerItemPOCO> playerItemServicesDB, IMessageService messageService)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.Inventory);
            _worldService = worldService;
            _messageService = messageService;
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
            _messageService.AddMessage(searchResult);
        }

        private void SendInventoryDTO(InventoryDTO inventoryDTO)
        {
            var payload = JsonConvert.SerializeObject(inventoryDTO);
            _clientController.SendPayload(payload, PacketType.Inventory);
        }

        public void InspectItem(string slot)
        {
            var inventory = _worldService.GetCurrentPlayer().Inventory;
            string output = "No item in this inventory slot";

            try
            {
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
                    output = inventoryItem.ToString();
                }
            }
            catch (ArgumentOutOfRangeException e) {}
            
            _messageService.AddMessage(output);
        }

        public HandlerResponseDTO HandlePacket(PacketDTO packet)
        {
            var inventoryDTO = JsonConvert.DeserializeObject<InventoryDTO>(packet.Payload);
            bool handleInDatabase = (_clientController.IsHost() && packet.Header.Target.Equals("host")) || _clientController.IsBackupHost;


            if (packet.Header.Target == "host" || packet.Header.Target == "client")
            {
                switch (inventoryDTO.InventoryType)
                {
                    case InventoryType.Use:
                        return HandleUse(inventoryDTO, handleInDatabase);
                    case InventoryType.Drop:
                        return HandleDrop(inventoryDTO, handleInDatabase);
                    case InventoryType.Pickup:
                        return HandlePickup(inventoryDTO, handleInDatabase);
                }
            }
            else
            {
                _messageService.AddMessage(packet.HandlerResponse.ResultMessage);
            }
            return new(SendAction.Ignore, null);
        }

        private HandlerResponseDTO HandlePickup(InventoryDTO inventoryDTO, bool handleInDatabase)
        {
            throw new NotImplementedException();
        }

        private HandlerResponseDTO HandleDrop(InventoryDTO inventoryDTO, bool handleInDatabase)
        {
            Player player = _worldService.GetPlayer(inventoryDTO.UserId);

            switch (inventoryDTO.Index)
            {
                case 0:
                    //Add helmet to current tile
                    player.Inventory.Helmet = null;
                    break;
                case 1:
                    //Add armor to current tile
                    player.Inventory.Armor = null;
                    break;
                case 2:
                    //Add weapon to current tile
                    player.Inventory.Weapon = null;
                    break;
                case 3:
                    //Add slot 1 item to current tile
                    player.Inventory.ConsumableItemList[0] = null;
                    break;
                case 4:
                    //Add slot 2 item to current tile
                    player.Inventory.ConsumableItemList[1] = null;
                    break;
                case 5:
                    //Add slot 3 item to current tile
                    player.Inventory.ConsumableItemList[2] = null;
                    break;
                default:
                    Console.WriteLine("Unknown slot");
                    break;
            }

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
                    _messageService.AddMessage("Could not find item");
                }
                return new HandlerResponseDTO(SendAction.ReturnToSender, "Could not find item");
            }
        }

        public void PickupItem(int index)
        {
            throw new NotImplementedException();
        }

        public void DropItem(string inventorySlot)
        {
            int index = 0;
            switch (inventorySlot)
            {
                case "helmet":
                    index = 0;
                    break;
                case "armor":
                    index = 1;
                    break;
                case "weapon":
                    index = 2;
                    break;
                case "slot 1":
                    index = 3;
                    break;
                case "slot 2":
                    index = 4;
                    break;
                case "slot 3":
                    index = 5;
                    break;
                default:
                    Console.WriteLine("Unknown slot");
                    break;
            }

            InventoryDTO inventoryDTO =
                new InventoryDTO(_clientController.GetOriginId(), InventoryType.Drop, index);
            SendInventoryDTO(inventoryDTO);
        }
    }
}
