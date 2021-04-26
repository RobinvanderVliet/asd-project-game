using System;
using WorldGeneration.Models.Interfaces;

namespace WorldGeneration.Models.BuildingTiles
{
    public class WallTile : IBuildingTile
    {
        public bool IsAccessible { get; set; }
        public string Symbol { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
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