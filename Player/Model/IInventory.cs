using System.Collections.Generic;

namespace Player.Model
{
    public interface IInventory
    {
        public List<IItem> ItemList { get; set; }

        public IItem GetItem(string itemName);

        public void AddItem(IItem item);

        public void RemoveItem(IItem item);

        public void EmptyInventory();
    }
}
