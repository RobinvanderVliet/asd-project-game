using System;
using Player.Model;

namespace Player.Services
{
    public class PlayerService
    {
        public void Attack(IPlayerModel player1, IPlayerModel player2, string direction)
        {
            //player1.getTile();
            //check with the gameboard whether or not there's a player in the given direction from this tile
            //if yes {
            //int dmg = player1.GetAttackDamage();
            //player1.RemoveStamina(1);
            // player2.RemoveHealth(dmg);
            //} else {
            //  Console.WriteLine("You swung at nothing!");
            // player1.RemoveStamina(1);
            //}
            Console.WriteLine("Attacked in " + direction + " direction.");
        }
        
        public void ExitCurrentGame(IPlayerModel playerModel)
        {
            //code for removing player from lobby
            Console.WriteLine("Spel geleaved.");
        }
    }
}
