using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ASD_project.Items;
using ASD_project.World.Models.Interfaces;

namespace ASD_project.World.Models.TerrainTiles
{
    [ExcludeFromCodeCoverage]
    public class WaterTile : ITerrainTile
    {
        public bool IsAccessible { get; set; }
        public string Symbol { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public List<Item> ItemsOnTile { get; set; }

        public WaterTile(int x, int y)
        {
            Symbol = TileSymbol.WATER;
            ItemsOnTile = new();
            IsAccessible = false;
            XPosition = x;
            YPosition = y;
            ItemsOnTile = new List<Item>();
        }
    }
}