using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ASD_Game.Items;
using ASD_Game.World.Models.Interfaces;

namespace ASD_Game.World.Models.LootableTiles
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
    }
}