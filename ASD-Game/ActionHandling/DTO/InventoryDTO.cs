using System.Diagnostics.CodeAnalysis;

namespace ASD_Game.ActionHandling.DTO
{
    [ExcludeFromCodeCoverage]
    public class InventoryDTO
    {
        public string UserId { get; set; }
        public InventoryType InventoryType { get; set; }
        public int Index { get; set; }

        public InventoryDTO(string userId, InventoryType inventoryType, int index)
        {
            UserId = userId;
            InventoryType = inventoryType;
            Index = index;
        }
    }
}
