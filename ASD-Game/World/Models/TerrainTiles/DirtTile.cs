using Items;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using WorldGeneration.Models.Interfaces;

namespace WorldGeneration.Models.TerrainTiles
{
    [ExcludeFromCodeCoverage]
    public class DirtTile : ITerrainTile
    {
        public bool IsAccessible { get; set; }
        public string Symbol { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public List<Item> ItemsOnTile { get; set; }

        public DirtTile(int x, int y)
        {
            ItemsOnTile = new();
            Symbol = TileSymbol.DIRT;
            IsAccessible = true;
            XPosition = x;
            YPosition = y;
            ItemsOnTile = new List<Item>();
        }
    }
}