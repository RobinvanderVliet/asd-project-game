using System;
using System.Linq;
using ActionHandling;
using ActionHandling.DTO;
using ASD_project.DatabaseHandler.POCO;
using ASD_project.DatabaseHandler.Services;
using ASD_project.Items;
using ASD_project.Items.Consumables;
using ASD_project.Network;
using ASD_project.Network.DTO;
using ASD_project.Network.Enum;
using ASD_project.World.Models.Characters;
using ASD_project.World.Services;
using Messages;
using Newtonsoft.Json;
using WorldGeneration.Exceptions;

namespace ASD_project.ActionHandling
{
    public class InventoryHandler : IPacketHandler, IInventoryHandler
    {
        private readonly IClientController _clientController;
        private readonly IWorldService _worldService;
        private readonly IMessageService _messageService;
        private readonly IDatabaseService<PlayerPOCO> _playerServicesDB;
        private readonly IDatabaseService<PlayerItemPOCO> _playerItemServicesDB;
        private readonly IDatabaseService<WorldItemPOCO> _worldItemServicesDB;


        public InventoryHandler(IClientController clientController, IWorldService worldService, IDatabaseService<PlayerPOCO> playerServicesDB, IDatabaseService<PlayerItemPOCO> playerItemServicesDB, IDatabaseService<WorldItemPOCO> worldItemServicesDB, IMessageService messageService)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.Inventory);
            _worldService = worldService;
            _messageService = messageService;
            _playerServicesDB = playerServicesDB;
            _playerItemServicesDB = playerItemServicesDB;
            _worldItemServicesDB = worldItemServicesDB;
        }

        public void UseItem(int index)
        {
            InventoryDTO inventoryDTO = new(_clientController.GetOriginId(), InventoryType.Use, index);
            SendInventoryDTO(inventoryDTO);
        }

        public void Search()
        {
            _messageService.AddMessage(_worldService.SearchCurrentTile());
        }

        public void PickupItem(int index)
        {
            // Compensate for index starting at 0.
            index -= 1;

            InventoryDTO inventoryDTO =
                new InventoryDTO(_clientController.GetOriginId(), InventoryType.Pickup, index);
            SendInventoryDTO(inventoryDTO);
        }
        public void DropItem(string inventorySlot)
        {
            int index = inventorySlot switch
            {
                "helmet" => 0,
                "armor" => 1,
                "weapon" => 2,
                "slot 1" => 3,
                "slot 2" => 4,
                "slot 3" => 5,
                _ => 99
            };

            if (index == 99)
            {
                _messageService.AddMessage("Unknown item slot");
            } 
            else
            {
                InventoryDTO inventoryDTO = new (_clientController.GetOriginId(), InventoryType.Drop, index);
                SendInventoryDTO(inventoryDTO);
            }   
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
            Player player = _worldService.GetPlayer(inventoryDTO.UserId);
            Item item;

            try
            {
                item = _worldService.GetItemsOnCurrentTile(player).ElementAt(inventoryDTO.Index);
            }
            catch (ArgumentOutOfRangeException)
            {
                if (inventoryDTO.UserId == _clientController.GetOriginId())
                {
                    _messageService.AddMessage("Number is not in search list!");
                }
                return new HandlerResponseDTO(SendAction.ReturnToSender, "Number is not in search list!");
            }

            try
            {
                player.Inventory.AddItem(item);

                _worldService.GetItemsOnCurrentTile(player).RemoveAt(inventoryDTO.Index);
                _worldService.DisplayStats();

                if (handleInDatabase)
                {
                    PlayerItemPOCO playerItemPOCO = new PlayerItemPOCO
                    {
                        PlayerGUID = inventoryDTO.UserId,
                        GameGUID = _clientController.SessionId,
                        ItemName = item.ItemName,
                    };
                    
                    if (item is Armor armor)
                    {
                        playerItemPOCO.ArmorPoints = armor.ArmorProtectionPoints;
                    }

                    _playerItemServicesDB.CreateAsync(playerItemPOCO);
                }

                return new HandlerResponseDTO(SendAction.SendToClients, null);
            }
            catch (InventoryFullException e)
            {
                if (inventoryDTO.UserId == _clientController.GetOriginId())
                {
                    _messageService.AddMessage(e.Message);
                }
                return new HandlerResponseDTO(SendAction.ReturnToSender, e.Message);
            }
        }

        private HandlerResponseDTO HandleDrop(InventoryDTO inventoryDTO, bool handleInDatabase)
        {
            Player player = _worldService.GetPlayer(inventoryDTO.UserId);

            Item item = null;

            switch (inventoryDTO.Index)
            {
                case 0:
                    item = player.Inventory.Helmet;
                    player.Inventory.Helmet = null;
                    break;
                case 1:
                    item = player.Inventory.Armor;
                    player.Inventory.Armor = null;
                    break;
                case 2:
                    item = player.Inventory.Weapon;
                    player.Inventory.Weapon = null;
                    break;
                case 3:
                    if(player.Inventory.ConsumableItemList.Count >= 1)
                    {
                        item = player.Inventory.ConsumableItemList.ElementAt(0);
                        player.Inventory.ConsumableItemList.RemoveAt(0);
                    }              
                    break;
                case 4:
                    if (player.Inventory.ConsumableItemList.Count >= 2)
                    {
                        item = player.Inventory.ConsumableItemList.ElementAt(1);
                        player.Inventory.ConsumableItemList.RemoveAt(1);
                    }
                    break;
                case 5:
                    if (player.Inventory.ConsumableItemList.Count >= 3)
                    {
                        item = player.Inventory.ConsumableItemList.ElementAt(2);
                        player.Inventory.ConsumableItemList.RemoveAt(2);
                    }
                    break;
                default:
                    break;
            }

            if (item != null)
            {
                _worldService.DisplayStats();
                if (handleInDatabase)
                {
                    PlayerItemPOCO playerItemPOCO = _playerItemServicesDB.GetAllAsync().Result.FirstOrDefault(playerItem => playerItem.GameGUID == _clientController.SessionId && playerItem.ItemName == item.ItemName && playerItem.PlayerGUID == player.Id);
                    _ = _playerItemServicesDB.DeleteAsync(playerItemPOCO);
                }

                if (inventoryDTO.UserId == _clientController.GetOriginId())
                {
                    _messageService.AddMessage("You dropped " + item.ItemName);
                }

                return new HandlerResponseDTO(SendAction.SendToClients, null);
            } 
            else
            {
                if(inventoryDTO.UserId == _clientController.GetOriginId())
                {
                    _messageService.AddMessage("This is not an item you can drop!");

                }
                return new HandlerResponseDTO(SendAction.ReturnToSender, "This is not an item you can drop!");
            }
        }

        private HandlerResponseDTO HandleUse(InventoryDTO inventoryDTO, bool handleInDatabase)
        {
            var player = _worldService.GetPlayer(inventoryDTO.UserId);
            if(player.Inventory.ConsumableItemList.Count >= inventoryDTO.Index)
            {
                Consumable itemToUse = player.Inventory.ConsumableItemList.ElementAt(inventoryDTO.Index-1);
                player.Inventory.ConsumableItemList.RemoveAt(inventoryDTO.Index-1);
                player.UseConsumable(itemToUse);
                _worldService.DisplayStats();

                if (handleInDatabase)
                {
                    PlayerItemPOCO playerItemPOCO = _playerItemServicesDB.GetAllAsync().Result.FirstOrDefault(playerItem => playerItem.GameGUID == _clientController.SessionId && playerItem.ItemName == itemToUse.ItemName && playerItem.PlayerGUID == player.Id);
                    _ = _playerItemServicesDB.DeleteAsync(playerItemPOCO);

                    var result = _playerServicesDB.GetAllAsync().Result;
                    PlayerPOCO playerPOCO = result.FirstOrDefault(player => player.PlayerGUID == inventoryDTO.UserId && player.GameGUID == _clientController.SessionId );

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
    }
}
