using System.Collections.Generic;

namespace Player.Model
{
    public class Inventory : IInventory
    {
        public List<Item> ItemList { get; set; }

        public Inventory()
        {
            ItemList = new List<Item>();
        }

        public Item GetItem(string itemName)
        {
            foreach (var item in ItemList)
            {
                if (item.ItemName == itemName)
                {
                    return item;
                }
            }
            return null;
        }

        public void AddItem(Item item)
        {
            ItemList.Add(item);
        }

        public void RemoveItem(Item item)
        {
            ItemList.Remove(item);
        }

        public void EmptyInventory()
        {
            ItemList.Clear();
        }
    }
}
