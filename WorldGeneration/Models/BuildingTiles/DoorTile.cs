using System;
using WorldGeneration.Models.Interfaces;

namespace WorldGeneration.Models.BuildingTiles
{
    public class DoorTile : BuildingTile, ITile
    {
        public DoorTile()
        {
            Symbol = TileSymbol.Door;
            IsAccessible = true;
        }

        public override void DrawBuilding()
        {
            throw new NotImplementedException();
        }
    }
}