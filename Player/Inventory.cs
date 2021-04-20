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
        private List<Item> _itemList = new List<Item>();

        public Inventory()
        {

        }

        public List<Item> getInventory()
        {
            return _itemList;
        }

        public void setInventory(List<Item> newInventory)
        {
            _itemList = newInventory;
        }

        public Item getItem(string itemName)
        {
            foreach (var item in _itemList)
            {
                if (item.getItemName() == itemName)
                {
                    return item;
                }
            }
            return null;
        }

        public void addItem(Item item)
        {
            _itemList.Add(item);
        }

        public void removeItem(Item item)
        {
            _itemList.Remove(item);
        }

        public void emptyInventory()
        {
            _itemList.Clear();
        }
    }
}
