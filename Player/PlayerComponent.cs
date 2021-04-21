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
        public void Attack(string direction)
        {
            //Player1.getTile();
            //check with the gameboard whether or not there's a player in the given direction from this tile
            //if yes {
            //  random value
            //dmg = player1.GetAttackDamage();
            //Player1.RemoveStamina(1);
            //Player2.RemoveHealth(dmg)
            //} else {
            //  Console.WriteLine("You swung at nothing!");
            //Player1.RemoveStamina(1);
            //}

            Console.WriteLine("Attacked in " + direction + " direction.");
        }
    }
}
