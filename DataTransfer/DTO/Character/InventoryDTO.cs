using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DataTransfer.DTO.Character
{
    [ExcludeFromCodeCoverage]
    public class InventoryDTO
    {
        private List<ItemDTO> ItemList { get; set; }
    }
}