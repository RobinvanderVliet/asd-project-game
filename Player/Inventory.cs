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
