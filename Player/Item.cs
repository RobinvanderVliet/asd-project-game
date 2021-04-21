/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD-project.
 
    This file is created by team: 2.
     
    Goal of this file: Keeping track of items.
     
*/

using System;

namespace Player
{
    public class Item
    {
        public string ItemName { get; set; }
        public string Description { get; set; }

        public Item(string itemName, string decription)
        {
            ItemName = itemName;
            Description = decription;
        }
    }
}
