using System;
using Items;
using WorldGeneration.Models;
using WorldGeneration.Models.Interfaces;
using WorldGeneration.Models.LootableTiles;

namespace WorldGeneration
{
    public class ItemSpawner
    {

        public ITile PutItemOnTile(ITile tile, float noiseResult)
        {
            var item = RandomItemGenerator.GetRandomItem(noiseResult);
            if (item != null)
            {
                tile.ItemsOnTile.Add(item);
            }

            return tile;
        }
    }
}