using System.Collections.Generic;

namespace Player.Model
{
    public interface IInventory
    {
        public List<Item> ItemList { get; set; }

        public Item GetItem(string itemName);

        public void AddItem(Item item);

        public void RemoveItem(Item item);

        public void EmptyInventory();
    }
}
