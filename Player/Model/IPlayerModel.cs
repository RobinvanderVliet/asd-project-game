using System;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;

namespace Player.Model
{
    public interface IPlayerModel
    {
        public string Name { get; set; }
        
        public string PlayerGuid { get; set; }
        public int Health { get; set; }
        public int Stamina { get; set; }

        //public Tile _currentTile { get; set; }
        public IInventory Inventory { get; set; }
        public IBitcoin Bitcoins { get; set; }
        public IRadiationLevel RadiationLevel { get; set; }
        public int XPosition { get; set; }

        public int YPosition { get; set; }
        public string Symbol { get; set; }
        public ConsoleColor Color { get; set; }
        public int Team { get; set; }
        public void AddHealth(int amount);

        public void RemoveHealth(int amount);

        public void AddStamina(int amount);

        public void RemoveStamina(int amount);

        public IItem GetItem(string itemName);

        public void AddInventoryItem(IItem item);

        public void RemoveInventoryItem(IItem item);

        public void EmptyInventory();

        public void AddBitcoins(int amount);

        public void RemoveBitcoins(int amount);

        public int GetAttackDamage();
        
        public void PickupItem();

        public void DropItem(string itemName);

    }
}