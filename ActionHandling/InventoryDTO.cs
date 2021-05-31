using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player
{
    public class InventoryDTO
    {
        public InventoryType InventoryType { get; set; }
        public int Index { get; set; }

        public InventoryDTO(InventoryType inventoryType, int index)
        {
            InventoryType = inventoryType;
            Index = index;
        }
    }
}
