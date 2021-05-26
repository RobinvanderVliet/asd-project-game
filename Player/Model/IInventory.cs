using System.Collections.Generic;

namespace Player.Model
{
    public interface IInventory
    {
        public List<IItem> ConsumableItemList { get; set; }

        public IItem GetConsumableItem(string itemName);

        public void AddConsumableItem(IItem item);

        public void RemoveConsumableItem(IItem item);

        public void EmptyConsumableItemList();
    }
}
