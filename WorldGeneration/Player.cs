using System.Collections.Generic;
using WorldGeneration.Models;
using WorldGeneration.Models.Interfaces;

namespace WorldGeneration
{
   public class Player : IPlayer
   {
      public string Symbol { get; set; }
      public int[] CurrentPosition { get; set; }
      public string Name { get; set; }
      public int Health { get; set; }
      public int RadiationLevel { get; set; }
      public int Armor { get; set; }
      public int Bitcoins { get; set; }

      public Player(string name, int[] currentPosition)
      {
         Name = name;
         CurrentPosition = currentPosition;
         Symbol = CharacterSymbol.CURRENT_PLAYER;
      }
   }
}