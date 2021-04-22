/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD-Project.
 
    This file is created by team: 2.
     
    Goal of this file: Outside services for player.
     
*/

using System;
using Player.Model;

namespace Player.Services
{
    public class PlayerService
    {
        public void Attack(IPlayer player1, IPlayer player2, string direction)
        {
            //player1.getTile();
            //check with the gameboard whether or not there's a player in the given direction from this tile
            //if yes {
            ////random value
            //int dmg = player1.GetAttackDamage();
            //player1.RemoveStamina(1);
            // player2.RemoveHealth(dmg);
            //} else {  
            //  Console.WriteLine("You swung at nothing!");
            // player1.RemoveStamina(1);
            //}
            Console.WriteLine("Attacked in " + direction + " direction.");
        }
        
        public void ExitCurrentGame(IPlayer player)
        {
            //code for removing player from lobby
            Console.WriteLine("Spel geleaved.");
        }
    }
}
