using System;

namespace WorldGeneration.Models.BuildingTiles
{
    public class WallTile : BuildingTile
    {
        public WallTile()
        {
            Symbol = TileSymbol.Wall;
            IsAccessible = false;
        }

        public override void DrawBuilding()
        {
            throw new NotImplementedException();
        }
    }
}