using System.Collections.Generic;
using ASD_Game.Items;

namespace ASD_Game.World.Models.Interfaces
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