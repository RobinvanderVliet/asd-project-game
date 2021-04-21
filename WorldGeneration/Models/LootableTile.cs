using WorldGeneration.Models.Interfaces;

namespace WorldGeneration.Models
{
    public abstract class LootableTile : Tile, ILootableTile
    {
        public abstract int GenerateLoot();

        public abstract void LootItem(int item);
    }
}
