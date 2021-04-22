using WorldGeneration.Models.Interfaces;

namespace WorldGeneration.Models
{
    public abstract class LootAbleTile : Tile, ILootAbleTile
    {
        public abstract int GenerateLoot();

        public abstract int LootItem(int item);
    }
}
