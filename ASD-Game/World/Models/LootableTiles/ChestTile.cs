using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ASD_project.World.Models.Interfaces;
using Items;

namespace ASD_project.World.Models.LootableTiles
{
    [ExcludeFromCodeCoverage]
    public class ChestTile : ILootAbleTile
    {
        public bool IsAccessible { get; set; }
        public string Symbol { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public List<Item> ItemsOnTile { get; set; }

        public ChestTile()
        {
            Symbol = TileSymbol.CHEST;
            IsAccessible = true;
            ItemsOnTile = new List<Item>();
        }

        public int GenerateLoot()
        {
            throw new NotImplementedException();
        }

        public void LootItem(int item)
        {
            throw new NotImplementedException();
        }
    }
}