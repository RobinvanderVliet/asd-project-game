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
        private String _name;
        private int _health;
        private int _stamina;

        //private Tile _currentTile;
        private IInventory _inventory;
        private IBitcoin _bitcoins;
        private IRadiationLevel _radiationLevel;

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

        public String getName()
        {
            return _name;
        }

        public void setName(String newName)
        {
            _name = newName;
        }

        public int getHealthAmount()
        {
            return _health;
        }

        public void setHealthAmount(int amount)
        {
            _health = amount;
        }

        public void addHealth(int amount)
        {
            if (_health + amount >= HEALTHCAP)
            {
                _health = HEALTHCAP;
            } else
            {
                _health += amount;
            }
        }

        public void removeHealth(int amount)
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

        public int getStaminaAmount()
        {
            return _stamina;
        }

        public void setStaminaAmount(int amount)
        {
            _stamina = amount;
        }

        public void addStamina(int amount)
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

        public void removeStamina(int amount)
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

        //public Tile getTile()
        //{
        //    return _currentTile;
        //}

        //public void setTile(Tile newTile)
        //{
        //    _currentTile = newTile;
        //}

        public List<Item> getInventory()
        {
            return _inventory.getInventory();
        }

        public void setInventory(List<Item> newInventory)
        {
            _inventory.setInventory(newInventory);
        }

        public void addInventoryItem(Item item)
        {
            _inventory.addItem(item);
        }

        public void removeInventoryItem(Item item)
        {
            _inventory.removeItem(item);
        }

        public void emptyInventory()
        {
            _inventory.emptyInventory();
        }

        public int getBitcoinAmount()
        {
            return _bitcoins.getAmount();
        }

        public void setBitcoinAmount(int amount)
        {
            _bitcoins.setAmount(amount);
        }

        public void addBitcoins(int amount)
        { 
            _bitcoins.addAmount(amount);
        }

        public void removeBitcoins(int amount)
        {
            _bitcoins.removeAmount(amount);
        }

        public int getRadiationLevel()
        {
            return _radiationLevel.getRadiationLevel();
        }

        public void setRadiationLevel(int amount)
        {
            _radiationLevel.setRadiationLevel(amount);
        }

        public int getAttackDamage()
        {
            //random default attack damage for now
            int dmg = 5 + getItemDamage();
            return dmg;
        }

        private int getItemDamage()
        {
            //things like passive damage items go here
            return 0;
        }

        public void pickupItem()
        {
            //Item item = currentTile.pickupItem();
            //addInventoryItem(item);
            Console.WriteLine("Item opgepakt!");
        }

        public void dropItem(String itemName)
        {
            Item item = _inventory.getItem(itemName);
            if (item != null)
            {
                removeInventoryItem(item);
            }
            Console.WriteLine(item.getItemName() + " laten vallen.");
        }

        public void exitCurrentGame()
        {
            //code for removing player from lobby
            Console.WriteLine("Spel geleaved.");
        }
    }
}
