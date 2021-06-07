using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace ASD_Game.InputHandling.Antlr.Ast.Actions
{
    public class Drop : Command, IEquatable<Drop>
    {
        private InventorySlot _inventorySlot;
        public InventorySlot InventorySlot { get => _inventorySlot; private set => _inventorySlot = value; }

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
            return Equals(obj as Drop);
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(Drop other)
        {
            return true;
        }
    }
}