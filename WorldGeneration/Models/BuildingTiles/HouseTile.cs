using System;
using WorldGeneration.Models.Interfaces;

namespace WorldGeneration.Models.BuildingTiles
{
    public class HouseTile : IBuildingTile
    {
        public bool IsAccessible { get; set; }
        public string Symbol { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public HouseTile()
        {
            Symbol = TileSymbol.HOUSE;
            IsAccessible = true;
        }

        public void DrawBuilding()
        {
            throw new NotImplementedException();
        }
    }
}