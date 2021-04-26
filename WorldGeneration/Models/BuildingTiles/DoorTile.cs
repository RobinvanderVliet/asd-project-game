using System;
using WorldGeneration.Models.Interfaces;

namespace WorldGeneration.Models.BuildingTiles
{
    public class DoorTile : IBuildingTile
    {
        public bool IsAccessible { get; set; }
        public string Symbol { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public DoorTile()
        {
            Symbol = TileSymbol.DOOR;
            IsAccessible = true;
        }

        public void DrawBuilding()
        {
            throw new NotImplementedException();
        }
    }
}