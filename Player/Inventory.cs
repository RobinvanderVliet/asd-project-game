/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD-project.
 
    This file is created by team: 2.
     
    Goal of this file: Keeping track of the inventory.
     
*/

using System.Collections.Generic;

namespace Player
{
    public class Inventory : IInventory
    {
        public List<Item> _itemList { get; set; }

        public Inventory()
        {

        }

        public Item GetItem(string itemName)
        {
            foreach (var item in _itemList)
            {
                if (item._itemName == itemName)
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
