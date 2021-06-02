using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace InputHandling.Antlr.Ast.Actions
{
    public class Drop : Command, IEquatable<Drop>
    {
        private Message _itemName;
        public Message ItemName { get => _itemName; private set => _itemName = value; }

        public ArrayList GetChildren()
        {
            var children = new ArrayList();
            children.Add(_itemName);
            return children;
        }

        public override ASTNode AddChild(ASTNode child)
        {
            if (child is Message)
            {
                _itemName = (Message)child;
            }

            return this;
        }

        public ASTNode RemoveChild(ASTNode child)
        {
            if (child is Message && child == _itemName)
            {
                _itemName = null;
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
        public override int GetHashCode()
        {
            return _itemName.GetHashCode();
        }
    }
}