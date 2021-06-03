using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ASD_project.World.Models.Interfaces;
using Items;

namespace ASD_project.World.Models
{
    [ExcludeFromCodeCoverage]
    public abstract class Tile : ITile
    {
        public int XPosition { get; set; }
        public int YPosition { get; set; }

        public string Symbol { get; set; }
        public bool IsAccessible { get; set; }
        public List<Item> ItemsOnTile { get; set; }
    }
}