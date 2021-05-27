using Player.Model.Armor;
using System.Collections.Generic;
using Weapon;

namespace Player.Model
{
    public class Inventory : IInventory
    {
        private List<IItem> _consumableItems;
        public List<IItem> ConsumableItemList { get => _consumableItems; set => _consumableItems = value; }

        private IItem _armor;
        public IItem Armor { get => _armor; set => _armor = value; }

        private IItem _helmet;
        public IItem Helmet { get => _helmet; set => _helmet = value; }

        private IItem _weapon;
        public IItem Weapon { get => _weapon; set => _weapon = value; }

        public Inventory()
        {
            _consumableItems = new List<IItem>();
            _helmet = new Bandana();
            _weapon = new Knife();
        }

        public IItem GetConsumableItem(string itemName)
        {
            foreach (var item in _consumableItems)
            {
                if (item.ItemName == itemName)
                {
                    return item;
                }
            }
            return null;
        }

        public void AddConsumableItem(IItem item)
        {
            if(_consumableItems.Count <= 3)
            {
                _consumableItems.Add(item);
            } else
            {
                System.Console.WriteLine("You already have 3 consumable items in your inventory!");
            }
        }

        public void RemoveConsumableItem(IItem item)
        {
            _consumableItems.Remove(item);
        }

        public void EmptyConsumableItemList()
        {
            _consumableItems.Clear();
        }
    }
}
