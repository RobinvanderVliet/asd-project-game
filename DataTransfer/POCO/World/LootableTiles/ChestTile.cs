using System;
using WorldGeneration.Models.Interfaces;

namespace WorldGeneration.Models.LootableTiles
{
    public class ChestTile : ILootAbleTile
    {
        public bool IsAccessible { get; set; }
        public string Symbol { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public ChestTile()
        {
            Symbol = TileSymbol.CHEST;
            IsAccessible = true;
        }

        public int GenerateLoot()
        {
            throw new NotImplementedException();
        }

        public void LootItem(int item)
        {
            throw new NotImplementedException();
        }
    }
}