/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD-project.
 
    This file is created by team: 2.
     
    Goal of this file: Implementing the player.
     
*/

using System;
using System.Collections.Generic;

namespace Player
{
    public class PlayerModel
    {
        public String _name { get; set; }
        public int _health { get; set; }
        public int _stamina { get; set; }

        //public Tile _currentTile { get; set; }
        public IInventory _inventory { get; set; }
        public IBitcoin _bitcoins { get; set; }
        public IRadiationLevel _radiationLevel { get; set; }

        //random default values for health&stamina for now
        private const int HEALTHCAP = 100;
        private const int STAMINACAP = 10;

        public PlayerModel(String name//, Tile tile
                                      )
        {
            _name = name;
            _health = HEALTHCAP;
            _stamina = STAMINACAP;
            //_currentTile = tile;
            _inventory = new Inventory();
            //random default value for now
            _bitcoins = new Bitcoin(20);
            _radiationLevel = new RadiationLevel(0);
        }

        public void AddHealth(int amount)
        {
            if (_health + amount >= HEALTHCAP)
            {
                _health = HEALTHCAP;
            } else
            {
                _health += amount;
            }
        }

        public void RemoveHealth(int amount)
        {
            if (_health - amount <= 0)
            {
                _health = 0;
                //extra code for when a player dies goes here
            }
            else
            {
                _health -= amount;
            }
        }

        public void AddStamina(int amount)
        {
            if (_stamina + amount >= STAMINACAP)
            {
                _stamina = STAMINACAP;
            }
            else
            {
                _stamina -= amount;
            }
        }

        public void RemoveStamina(int amount)
        {
            if (_stamina - amount <= 0)
            {
                _stamina = 0;
            }
            else
            {
                _stamina -= amount;
            }
        }

        public void AddInventoryItem(Item item)
        {
            _inventory.AddItem(item);
        }

        public void RemoveInventoryItem(Item item)
        {
            _inventory.RemoveItem(item);
        }

        public void EmptyInventory()
        {
            _inventory.EmptyInventory();
        }

        public void AddBitcoins(int amount)
        { 
            _bitcoins.AddAmount(amount);
        }

        public void RemoveBitcoins(int amount)
        {
            _bitcoins.RemoveAmount(amount);
        }

        public int GetAttackDamage()
        {
            //random default attack damage for now
            int dmg = 5 + GetItemDamage();
            return dmg;
        }

        private int GetItemDamage()
        {
            //things like passive damage items go here
            return 0;
        }

        public void PickupItem()
        {
            //Item item = currentTile.pickupItem();
            //addInventoryItem(item);
            Console.WriteLine("Item opgepakt!");
        }

        public void DropItem(String itemName)
        {
            Item item = _inventory.GetItem(itemName);
            if (item != null)
            {
                RemoveInventoryItem(item);
            }
            Console.WriteLine(item._itemName + " laten vallen.");
        }

        public void ExitCurrentGame()
        {
            //code for removing player from lobby
            Console.WriteLine("Spel geleaved.");
        }
    }
}
