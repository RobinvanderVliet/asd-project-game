using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace ASD_Game.InputHandling.Antlr.Ast.Actions
{
    public class Inspect : Command
    {
        private InventorySlot _inventorySlot;
        [ExcludeFromCodeCoverage]
        public InventorySlot InventorySlot { get => _inventorySlot; private set => _inventorySlot = value; }

        [ExcludeFromCodeCoverage]
        public ArrayList GetChildren()
        {
            var children = new ArrayList();
            children.Add(_inventorySlot);
            return children;
        }

        public override ASTNode AddChild(ASTNode child)
        {
            if (child is InventorySlot)
            {
                _inventorySlot = (InventorySlot)child;
            }

            return this;
        }

        [ExcludeFromCodeCoverage]
        public ASTNode RemoveChild(ASTNode child)
        {
            if (child is InventorySlot && child == _inventorySlot)
            {
                _inventorySlot = null;
            }

            return this;
        }

        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            return Equals(obj as Inspect);
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(Inspect other)
        {
            return _inventorySlot.Equals(other.InventorySlot);
        }

        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
        {
            return _inventorySlot.GetHashCode();
        }
    }
}