using System;
using Player.Exceptions;
using WorldGeneration.Models;

namespace Player.Model
{
    public class PlayerModel : IPlayerModel
    {
        private string _name;
        public string Name { get => _name; set => _name = value; }

        private string _PlayerGuid;
        public string PlayerGuid { get => _PlayerGuid; set => _PlayerGuid = value; }
        
        private int _health;
        public int Health { get => _health; set => _health = value; }
        private int _stamina;
        public int Stamina { get => _stamina; set => _stamina = value; }

        // private Tile _currentTile;
        // public Tile CurrentTile { get => _currentTile; set => _currentTile = value; }
        private IInventory _inventory;
        public IInventory Inventory { get => _inventory; set => _inventory = value; }
        private IBitcoin _bitcoins;
        public IBitcoin Bitcoins { get => _bitcoins; set => _bitcoins = value; }
        private int _radiationLevel;
        public int RadiationLevel { get => _radiationLevel; set => _radiationLevel = value; }

        private int _xPosition;
        private int _yPosition;
        public int XPosition { get => _xPosition; set => _xPosition = value; }
        public int YPosition { get => _yPosition; set => _yPosition = value; }
        
        public string Symbol { get => _symbol; set => _symbol = value; }
        public ConsoleColor Color { get; set; }
        public int Team { get; set; }
        private string _symbol = CharacterSymbol.CURRENT_PLAYER;

        //random default values for health&stamina for now
        private const int HEALTHCAP = 100;
        private const int STAMINACAP = 10;
        private const int RADIATIONLEVELCAP = 10;


        public PlayerModel(string name, IInventory inventory, IBitcoin bitcoins, IRadiationLevel radiationLevel
            //, Tile tile
        )
        {
            _name = name;
            _health = HEALTHCAP;
            _stamina = STAMINACAP;
            _inventory = inventory;
            _bitcoins = bitcoins;
            _radiationLevel = RADIATIONLEVELCAP;
        }

        public void AddHealth(int amount)
        {
            if (_health + amount >= HEALTHCAP)
            {
                _health = HEALTHCAP;
            }
            else
            {
                _health += amount;
            }
        }

        public void RemoveHealth(int amount)
        {
            if (_health - amount <= 0)
            {
                _health = 0;
                //extra code for when a player dies goes here
            }
            else
            {
                _health -= amount;
            }
        }

        public void AddStamina(int amount)
        {
            if (_stamina + amount >= STAMINACAP)
            {
                _stamina = STAMINACAP;
            }
            else
            {
                _stamina += amount;
            }
        }

        public void RemoveStamina(int amount)
        {
            if (_stamina - amount <= 0)
            {
                _stamina = 0;
            }
            else
            {
                _stamina -= amount;
            }
        }

        public IItem GetItem(string itemName)
        {
            return _inventory.GetItem(itemName);
        }

        public void AddInventoryItem(IItem item)
        {
            _inventory.AddItem(item);
        }

        public void RemoveInventoryItem(IItem item)
        {
            _inventory.RemoveItem(item);
        }

        public void EmptyInventory()
        {
            _inventory.EmptyInventory();
        }

        public void AddBitcoins(int amount)
        {
            _bitcoins.AddAmount(amount);
        }

        public void RemoveBitcoins(int amount)
        {
            _bitcoins.RemoveAmount(amount);
        }

        public int GetAttackDamage()
        {
            //random default attack damage for now
            int dmg = 5 + GetItemDamage();
            return dmg;
        }

        private int GetItemDamage()
        {
            //things like passive damage items go here
            return 0;
        }

        public void PickupItem()
        {
            //Item item = currentTile.pickupItem();
            //addInventoryItem(item);
            Console.WriteLine("Item opgepakt!");
        }

        public void DropItem(string itemName)
        {
            IItem item = _inventory.GetItem(itemName);
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
        
    }
}