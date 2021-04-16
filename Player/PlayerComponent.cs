using System;
using System.Collections;
using System.Collections.Generic;

namespace Player
{
    public class PlayerComponent
    {
        //private Tile currentTile;
        //private List<Item> _inventory = new List<Item>();

        //public PlayerComponent(Tile tile)
        //{
        //    currentTile = tile;
        //}

        public void attack(String direction)
        {
            //
            Console.WriteLine("Attacked in " + direction + " direction.");
        }

        public void pickupItem()
        {
            //Item item = currentTile.pickupItem();
            //inventory.Add(item);
            Console.WriteLine("Item opgepakt!");
        }

        public void dropItem(String item)
        {
            //inventory.Remove(item);
            Console.WriteLine(item + " laten vallen.");
        }

        public void exitCurrentGame()
        {
            //
            Console.WriteLine("Spel geleaved.");
        }
    }
}
