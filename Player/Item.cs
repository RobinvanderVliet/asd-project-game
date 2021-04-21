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
        public String _itemName { get; set; }
        public String _description { get; set; }

        public Item(String itemName, String decription)
        {
            _itemName = itemName;
            _description = decription;
        }
    }
}
