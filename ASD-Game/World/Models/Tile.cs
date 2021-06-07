using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ASD_Game.Items;
using ASD_Game.World.Models.Interfaces;

namespace ASD_Game.World.Models
{
    [ExcludeFromCodeCoverage]
    public abstract class Tile : ITile
    {
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public string Symbol { get; set; }
        public bool IsAccessible { get; set; }
        public int StaminaCost { get; set; }
        public List<Item> ItemsOnTile { get; set; }
    }
}