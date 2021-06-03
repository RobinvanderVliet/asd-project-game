using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ASD_project.World.Models.Interfaces;
using Items;

namespace ASD_project.World.Models.TerrainTiles
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
            Symbol = TileSymbol.DIRT;
            IsAccessible = true;
            XPosition = x;
            YPosition = y;
            ItemsOnTile = new List<Item>();
        }
    }
}