using System.Collections.Generic;
using Items;
using Items.Consumables;

namespace ASD_project.World.Models.Characters
{
    public class Inventory
    {
        private List<Consumable> _consumableItems;
        public List<Consumable> ConsumableItemList { get => _consumableItems; set => _consumableItems = value; }

        private Armor _armor;
        public Armor Armor { get => _armor; set => _armor = value; }

        private Armor _helmet;
        public Armor Helmet { get => _helmet; set => _helmet = value; }

        private Weapon _weapon;
        public Weapon Weapon { get => _weapon; set => _weapon = value; }

        public Inventory()
        {
            _consumableItems = new List<Consumable>();
            _helmet = ItemFactory.GetBandana();
            _weapon = ItemFactory.GetKnife();
        }

        public Item GetConsumableItem(string itemName)
        {
            return _consumableItems.Find(item => item.ItemName == itemName);
        }

        public void AddConsumableItem(Consumable consumable)
        {
            if (_consumableItems.Count <= 3)
            {
                _consumableItems.Add(consumable);
            }
            else
            {
                System.Console.WriteLine("You already have 3 consumable items in your inventory!");
            }
        }

        public void RemoveConsumableItem(Consumable consumable)
        {
            _consumableItems.Remove(consumable);
        }

        public void EmptyConsumableItemList()
        {
            _consumableItems.Clear();
        }
    }
}
