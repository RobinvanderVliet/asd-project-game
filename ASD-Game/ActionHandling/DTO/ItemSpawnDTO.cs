using System.Diagnostics.CodeAnalysis;
using ASD_Game.Items;

namespace ASD_Game.ActionHandling.DTO
{
    [ExcludeFromCodeCoverage]
    public class ItemSpawnDTO
    {
        public int XPosition;
        public int YPosition;
        public Item Item;
    }
}