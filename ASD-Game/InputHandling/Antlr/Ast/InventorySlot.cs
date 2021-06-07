using System.Diagnostics.CodeAnalysis;

namespace ASD_Game.InputHandling.Antlr.Ast
{
    public class InventorySlot : ASTNode
    {
        private string _inventorySlot;
        public string InventorySlotValue { get => _inventorySlot; private set => _inventorySlot = value; }

        public InventorySlot(string inventorySlot)
        {
            _inventorySlot = inventorySlot;
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(InventorySlot other)
        {
            if (other == null)
                return false;

            return _inventorySlot == other.InventorySlotValue;
        }

        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            return Equals(obj as InventorySlot);
        }
        
        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
        {
            return _inventorySlot.GetHashCode();
        }
    }
}