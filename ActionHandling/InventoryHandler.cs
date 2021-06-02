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
<<<<<<< HEAD
            Player player = _worldService.GetPlayer(inventoryDTO.UserId);

            Item item;

            switch (inventoryDTO.Index)
            {
                case 0:
                    item = player.Inventory.Helmet;
                    _worldService.GetItemsOnCurrentTile().Add(item);
                    player.Inventory.Helmet = null;
                    Console.WriteLine("You dropped your helmet");
                    break;
                case 1:
                    item = player.Inventory.Armor;
                    _worldService.GetItemsOnCurrentTile().Add(item);
                    player.Inventory.Armor = null;
                    Console.WriteLine("You dropped your armor");
                    break;
                case 2:
                    item = player.Inventory.Weapon;
                    _worldService.GetItemsOnCurrentTile().Add(item);
                    player.Inventory.Weapon = null;
                    Console.WriteLine("You dropped your weapon");
                    break;
                case 3:
                    item = player.Inventory.ConsumableItemList[0];
                    _worldService.GetItemsOnCurrentTile().Add(item);
                    player.Inventory.ConsumableItemList[0] = null;
                    Console.WriteLine("You dropped your item in slot 1");
                    break;
                case 4:
                    item = player.Inventory.ConsumableItemList[1];
                    _worldService.GetItemsOnCurrentTile().Add(item);
                    player.Inventory.ConsumableItemList[1] = null;
                    Console.WriteLine("You dropped your item in slot 2");
                    break;
                case 5:
                    item = player.Inventory.ConsumableItemList[2];
                    _worldService.GetItemsOnCurrentTile().Add(item);
                    player.Inventory.ConsumableItemList[2] = null;
                    Console.WriteLine("You dropped your item in slot 3");
                    break;
                default:
                    Console.WriteLine("Unknown item slot");
                    return new HandlerResponseDTO(SendAction.ReturnToSender, "This is not an item you can drop!");
                    break;
            }
            return new HandlerResponseDTO(SendAction.SendToClients, null);
=======
            throw new NotImplementedException();
>>>>>>> parent of a6db6d52 (Merge pull request #153 in VDFZEH/asd-project-game from sub-task/VDFZEH-498-coderen-uitvoeren-commando-pickup-item to feature/VDFZEH-483-coderen-commando-use-n)
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

<<<<<<< HEAD
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
                    index = 99;
                    Console.WriteLine("Unknown item slot");
                    break;
            }

            InventoryDTO inventoryDTO =
                new InventoryDTO(_clientController.GetOriginId(), InventoryType.Drop, index);
            SendInventoryDTO(inventoryDTO);
=======
        public void DropItem(int index)
        {
            throw new NotImplementedException();
>>>>>>> parent of a6db6d52 (Merge pull request #153 in VDFZEH/asd-project-game from sub-task/VDFZEH-498-coderen-uitvoeren-commando-pickup-item to feature/VDFZEH-483-coderen-commando-use-n)
        }
    }
}
