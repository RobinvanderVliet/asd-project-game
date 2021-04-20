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
        public List<Item> getInventory();

        public void setInventory(List<Item> newInventory);

        public Item getItem(string itemName);

        public void addItem(Item item);

        public void removeItem(Item item);

        public void emptyInventory();
    }
}
