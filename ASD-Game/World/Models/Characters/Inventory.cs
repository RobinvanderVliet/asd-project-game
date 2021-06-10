using System;
using System.Collections.Generic;
using ASD_Game.Items;
using ASD_Game.Items.ArmorStats;
using ASD_Game.Items.Consumables;
using ASD_Game.World.Models.Characters.Exceptions;

namespace ASD_Game.World.Models.Characters
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
            if (_consumableItems.Count < 3)
            {
                _consumableItems.Add(consumable);
            }
            else
            {
                throw new InventoryFullException("You already have 3 consumable items in your inventory!");
            }
        }

        public void AddItem(Item item)
        {
            if (item is Weapon weapon)
            {
                if (_weapon != null) throw new InventoryFullException("You already have a weapon!");
                _weapon = weapon;
                return;
            }

            if (item is Armor armor)
            {
                if (armor.ArmorPartType == ArmorPartType.Body)
                {
                    if (_armor != null) throw new InventoryFullException("You already have body armor!");
                    _armor = armor;
                    return;
                }

                if (armor.ArmorPartType == ArmorPartType.Helmet)
                {
                    if (_helmet != null) throw new InventoryFullException("You already have a helmet!");
                    _helmet = armor;
                    return;
                }
            }

            if (item is Consumable consumable)
            {
                AddConsumableItem(consumable);
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

        public Consumable GetConsumableAtIndex(int i)
        {
            try
            {
                return ConsumableItemList[i];
            }
            catch(Exception e)
            {
                return null;
            }
        }
    }
}