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
        private String _itemName;
        private String _description;

        public Item(String itemName, String decription)
        {
            _itemName = itemName;
            _description = decription;
        }

        public String getItemName()
        {
            return _itemName;
        }

        public void setItemName(String itemName)
        {
            _itemName = itemName;
        }

        public String getDescription()
        {
            return _description;
        }

        public void setDescription(String descriptionName)
        {
            _description = descriptionName;
        }
    }
}
