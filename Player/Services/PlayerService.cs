using System;
using Player.ActionHandlers;
using Chat;
using DataTransfer.DTO.Character;
using Network;
using Player.Exceptions;
using Player.Model;
using WorldGeneration;

namespace Player.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerModel _currentPlayer;
        private readonly IMoveHandler _moveHandler;
        private readonly IChatHandler _chatHandler;
        private readonly IWorldService _worldService;
        private readonly IClientController _clientController;

        //random default values for health&stamina for now
        private const int MINIMUM_HEALTH = 0;
        private const int HEALTHCAP = 100;
        private const int MINIMUM_STAMINA = 0;
        private const int STAMINACAP = 10;

        public PlayerService(IPlayerModel currentPlayer
            , IChatHandler chatHandler
            , IMoveHandler moveHandler
            , IClientController clientController
            , IWorldService worldService)
        {
            _chatHandler = chatHandler;
            _currentPlayer = currentPlayer;
            _moveHandler = moveHandler;
            _clientController = clientController;
            currentPlayer.PlayerGuid = _clientController.GetOriginId();
            _worldService = worldService;
        }

        public void Attack(string direction)
        {
            Console.WriteLine("Attacked in " + direction + " direction.");
        }

        public void ExitCurrentGame()
        {
            //code for removing player from lobby
            Console.WriteLine("Game left.");
        }

        public void Pause()
        {
            //code for pause game (only for host)
            Console.WriteLine("Game paused.");
        }

        public void Resume()
        {
            //code for resume game (only for host)
            Console.WriteLine("Game resumed.");
        }

        public void ReplaceByAgent()
        {
            //code for replacing player by agent
            Console.WriteLine("Replaced by agent");
        }

        public void Say(string messageValue)
        {
            _chatHandler.SendSay(messageValue);
        }

        public void Shout(string messageValue)
        {
            _chatHandler.SendShout(messageValue);
        }

        public void AddHealth(int amount)
        {
            if (_currentPlayer.Health + amount >= HEALTHCAP)
            {
                _currentPlayer.Health = HEALTHCAP;
            }
            else
            {
                _currentPlayer.Health += amount;
            }
        }

        public void RemoveHealth(int amount)
        {
            if (_currentPlayer.Health - amount <= MINIMUM_HEALTH)
            {
                _currentPlayer.Health = MINIMUM_HEALTH;
                //extra code for when a player dies goes here
            }
            else
            {
                _currentPlayer.Health -= amount;
            }
        }

        public void AddStamina(int amount)
        {
            if (_currentPlayer.Stamina + amount >= STAMINACAP)
            {
                _currentPlayer.Stamina = STAMINACAP;
            }
            else
            {
                _currentPlayer.Stamina += amount;
            }
        }

        public void RemoveStamina(int amount)
        {
            if (_currentPlayer.Stamina - amount <= MINIMUM_STAMINA)
            {
                _currentPlayer.Stamina = MINIMUM_STAMINA;
            }
            else
            {
                _currentPlayer.Stamina -= amount;
            }
        }

        public IItem GetItem(string itemName)
        {
            return _currentPlayer.Inventory.GetItem(itemName);
        }

        public void AddInventoryItem(IItem item)
        {
            _currentPlayer.Inventory.AddItem(item);
        }

        public void RemoveInventoryItem(IItem item)
        {
            _currentPlayer.Inventory.RemoveItem(item);
        }

        public void EmptyInventory()
        {
            _currentPlayer.Inventory.EmptyInventory();
        }

        public void AddBitcoins(int amount)
        {
            _currentPlayer.Bitcoins.AddAmount(amount);
        }

        public void RemoveBitcoins(int amount)
        {
            _currentPlayer.Bitcoins.RemoveAmount(amount);
        }

        public int GetAttackDamage()
        {
            int dmg = 5 + GetItemDamage();
            return dmg;
        }
        
        private int GetItemDamage()
        {
            //things like passive damage items go here]
            return 0;
        }

        public void PickupItem()
        {
            //Item item = currentTile.pickupItem();
            // _currentPlayer.Inventory.AddItem(item);
            Console.WriteLine("Item opgepakt!");
        }

        public void DropItem(string itemName)
        {
            IItem item = _currentPlayer.Inventory.GetItem(itemName);
            if (item != null)
            {
                RemoveInventoryItem(item);
                Console.WriteLine(item.ItemName + " laten vallen.");
            }
            else
            {
                throw new ItemException("Je hebt geen " + itemName + " item in je inventory!");
            }
        }

        public void HandleDirection(string directionValue, int stepsValue)
        {
            int x = 0;
            int y = 0;
            switch (directionValue)
            {
                case "right":
                case "east":
                    x = stepsValue;
                    break;
                case "left":
                case "west":
                    x = -stepsValue;
                    break;
                case "forward":
                case "up":
                case "north":
                    y = +stepsValue;
                    break;
                case "backward":
                case "down":
                case "south":
                    y = -stepsValue;
                    break;
            }

            var mapCharacterDTO = new MapCharacterDTO((_worldService.getCurrentCharacterPositions().XPosition) + x, 
                (_worldService.getCurrentCharacterPositions().YPosition) + y, 
                _currentPlayer.PlayerGuid, 
                _worldService.getCurrentCharacterPositions().GameGuid, 
                _currentPlayer.Symbol);
            
            _moveHandler.SendMove(mapCharacterDTO);
            
            MapCharacterDTO currentCharacter =  _worldService.getCurrentCharacterPositions();
            _currentPlayer.XPosition = currentCharacter.XPosition;
            _currentPlayer.YPosition = currentCharacter.YPosition;
        }
    }
}