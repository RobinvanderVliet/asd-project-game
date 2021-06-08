using System;
using System.Diagnostics.CodeAnalysis;

namespace ASD_Game.InputHandling.Antlr.Ast.Actions
{
    public class Pickup : Command, IEquatable<Pickup>
    {
        private Step _item;
        [ExcludeFromCodeCoverage]
        public Step Item { get => _item; private set => _item = value; }

        public override ASTNode AddChild(ASTNode child)
        {
            if (child is Step step)
            {
                _item = step;
            }

            return this;
        }

        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            return Equals(obj as Pickup);
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(Pickup other)
        {
            if (other == null)
                return false;

            return _item.Equals(other._item);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}