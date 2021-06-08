using Items;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using WorldGeneration.Models.Interfaces;

namespace WorldGeneration.Models.LootableTiles
{
    [ExcludeFromCodeCoverage]
    public class ChestTile : ILootAbleTile
    {
        public bool IsAccessible { get; set; }
        public string Symbol { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public int StaminaCost { get; set; }
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