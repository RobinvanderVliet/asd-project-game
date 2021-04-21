using WorldGeneration.Models.Interfaces;

namespace WorldGeneration.Models
{
    public abstract class HazardousTile : Tile, IHazardousTile
    {
        public int Damage { get; set; }

        public abstract int GetDamage(int time);
    }
}
