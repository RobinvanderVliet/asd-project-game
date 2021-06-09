using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ASD_Game.Items;
using ASD_Game.World.Models.Interfaces;

namespace ASD_Game.World.Models.TerrainTiles
{
    [ExcludeFromCodeCoverage]
    public class StreetTile : ITerrainTile
    {
        
        public bool IsAccessible { get; set; }
        public string Symbol { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public int StaminaCost { get; set; }
        public List<Item> ItemsOnTile { get; set; }

        

        public StreetTile(int x, int y)
        {
            ItemsOnTile = new();
            Symbol = TileSymbol.STREET;
            IsAccessible = true;
            XPosition = x;
            YPosition = y;
            StaminaCost = 1;
        }
    }
}