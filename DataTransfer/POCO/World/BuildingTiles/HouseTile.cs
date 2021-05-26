﻿using System;
using DataTransfer.POCO.World.Interfaces;

namespace DataTransfer.POCO.World.BuildingTiles
{
    public class HouseTile : IBuildingTile
    {
        public bool IsAccessible { get; set; }
        public string Symbol { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
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