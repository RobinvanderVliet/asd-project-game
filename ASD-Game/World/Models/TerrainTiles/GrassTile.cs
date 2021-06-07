using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ASD_project.Items;
using ASD_project.World.Models.Interfaces;

namespace ASD_project.World.Models.TerrainTiles
{
    [ExcludeFromCodeCoverage]
    public class GrassTile : ITerrainTile
    {
        
        public bool IsAccessible { get; set; }
        public string Symbol { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public List<Item> ItemsOnTile { get; set; }

        public GrassTile(int x, int y)
        {
            ItemsOnTile = new();
            Symbol = TileSymbol.GRASS;
            IsAccessible = true;
            XPosition = x;
            YPosition = y;
            ItemsOnTile = new List<Item>();
        }
    }
}