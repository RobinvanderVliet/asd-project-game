using Items;
using WorldGeneration.Models.LootableTiles;

namespace WorldGeneration
{
    public class ItemSpawner : ISpawner
    {
        public void Spawn()
        {
            var chestTile = new ChestTile();
            chestTile.ItemsOnTile.Add(ItemFactory.GetKnife());
            throw new System.NotImplementedException();
        }
    }
}