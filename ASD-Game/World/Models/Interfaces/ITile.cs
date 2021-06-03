using System.Collections.Generic;
using Items;

namespace ASD_project.World.Models.Interfaces
{
    public interface ITile
    {
        bool IsAccessible { get; set; }
        string Symbol { get; set; }
        int XPosition { get; set; }
        int YPosition { get; set; }
        public List<Item> ItemsOnTile { get; set; }
    }
}