using System.Collections.Generic;

namespace Player.Model
{
    public class Inventory : IInventory
    {
        private List<IItem> _itemList;
        public List<IItem> ItemList { get => _itemList; set => _itemList = value; }

        public Inventory()
        {
            _itemList = new List<IItem>();
        }

        public IItem GetItem(string itemName)
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

        public void AddItem(IItem item)
        {
            _itemList.Add(item);
        }

        public void RemoveItem(IItem item)
        {
            _itemList.Remove(item);
        }

        public void EmptyInventory()
        {
            _itemList.Clear();
        }
    }
}
