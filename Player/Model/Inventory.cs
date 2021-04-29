using System.Collections.Generic;

namespace Player.Model
{
    public class Inventory : IInventory
    {
        private List<Item> _itemList;
        public List<Item> ItemList { get => _itemList; set => _itemList = value; }

        public Inventory()
        {
            _itemList = new List<Item>();
        }

        public Item GetItem(string itemName)
        {
            foreach (var item in _itemList)
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
            _itemList.Add(item);
        }

        public void RemoveItem(Item item)
        {
            _itemList.Remove(item);
        }

        public void EmptyInventory()
        {
            _itemList.Clear();
        }
    }
}
