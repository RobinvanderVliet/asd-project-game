/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD-Project.
 
    This file is created by team: 2.
     
    Goal of this file: Currently for managing player attacks.
     
*/

using System;

namespace Player
{
    public class PlayerComponent
    {
        public void attack(String direction)
        {
            //Player1.getTile();
            //check with the gameboard whether or not there's a player in the given direction from this tile
            //if yes {
            //  random value
            //dmg = player1.getAttackDamage();
            //Player1.removeStamina(1);
            //Player2.removeHealth(dmg)
            //} else {
            //  Console.WriteLine("You swung at nothing!");
            //Player1.removeStamina(1);
            //}

            Console.WriteLine("Attacked in " + direction + " direction.");
        }
    }
}
