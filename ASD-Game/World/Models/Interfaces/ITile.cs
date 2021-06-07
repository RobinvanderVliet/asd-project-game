using System.Collections.Generic;
using ASD_project.Items;

namespace ASD_project.World.Models.Interfaces
{
    public interface ITile
    {
        bool IsAccessible { get; set; }
        string Symbol { get; set; }
        int XPosition { get; set; }
        int YPosition { get; set; }
        int StaminaCost { get; set; }
        public List<Item> ItemsOnTile { get; set; }
    }
}