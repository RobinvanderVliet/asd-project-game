/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD-project.
 
    This file is created by team: 2.
     
    Goal of this file: Creating an interface for the inventory.
     
*/

using System.Collections.Generic;

namespace Player
{
    public interface IInventory
    {
        public List<Item> _itemList { get; set; }

        public Item GetItem(string itemName);

        public void AddItem(Item item);

        public void RemoveItem(Item item);

        public void EmptyInventory();
    }
}
