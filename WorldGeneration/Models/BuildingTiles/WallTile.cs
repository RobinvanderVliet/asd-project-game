using System;
using WorldGeneration.Models.Interfaces;

namespace WorldGeneration.Models.BuildingTiles
{
    public class WallTile : IBuildingTile
    {
        public bool IsAccessible { get; set; }
        public string Symbol { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public WallTile()
        {
            Symbol = TileSymbol.WALL;
            IsAccessible = false;
        }

        public void DrawBuilding()
        {
            throw new NotImplementedException();
        }
    }
}