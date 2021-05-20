using System;
using DataTransfer.POCO.World.Interfaces;

namespace DataTransfer.POCO.World.BuildingTiles
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