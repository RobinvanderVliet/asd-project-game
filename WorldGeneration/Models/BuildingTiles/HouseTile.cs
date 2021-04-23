using System;

namespace WorldGeneration.Models.BuildingTiles
{
    public class HouseTile : BuildingTile
    {
        public HouseTile()
        {
            Symbol = TileSymbol.HOUSE;
            IsAccessible = true;
        }

        public override void DrawBuilding()
        {
            throw new NotImplementedException();
        }
    }
}