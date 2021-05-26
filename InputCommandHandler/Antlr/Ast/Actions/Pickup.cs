using System;
using System.Diagnostics.CodeAnalysis;

namespace InputCommandHandler.Antlr.Ast.Actions
{
    public class Pickup : Command, IEquatable<Pickup>
    {
        private Step _item;
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
            return other != null && _item.Equals(other._item);
        }
    }
}