using System;

namespace WorldGeneration.Models.HazardousTiles
{
    public class SpikeTile : HazardousTile
    {
        public SpikeTile()
        {
            Symbol = TileSymbol.Spike;
            IsAccessible = true;
            Damage = new Random().Next(2, 11);
        }

        public override int GetDamage(int time)
        {
            return Damage;
        }
    }
}