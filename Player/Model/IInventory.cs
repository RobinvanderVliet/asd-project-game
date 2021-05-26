using System.Collections.Generic;

namespace Player.Model
{
    public interface IInventory
    {
        public List<IItem> ConsumableItemList { get; set; }
        public IItem Armor { get; set; }

        public IItem Helmet { get; set; }

        public IItem MeleeWeapon { get; set; }

        public IItem RangedWeapon { get; set; }

        public IItem GetConsumableItem(string itemName);

        public void AddConsumableItem(IItem item);

        public void RemoveConsumableItem(IItem item);

        public void EmptyConsumableItemList();
    }
}
