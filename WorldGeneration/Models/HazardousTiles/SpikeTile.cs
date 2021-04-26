using System;
using WorldGeneration.Models.Interfaces;

namespace WorldGeneration.Models.HazardousTiles
{
    public class SpikeTile : IHazardousTile
    {
        public bool IsAccessible { get; set; }
        public string Symbol { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public SpikeTile()
        {
            Symbol = TileSymbol.SPIKE;
            IsAccessible = true;
            Damage = new Random().Next(2, 11);
        }

        public int Damage { get; set; }

        public int GetDamage(int time)
        {
            return Damage;
        }
    }
}