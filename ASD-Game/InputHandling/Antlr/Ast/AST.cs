using System;
using System.Diagnostics.CodeAnalysis;

namespace ASD_Game.InputHandling.Antlr.Ast
{
    public class AST : IEquatable<AST>
    {
        private Input _root;
        public Input Root { get => _root; private set => _root = value; }

        public AST()
        {
            _root = new Input();
        }

        public AST(Input input)
        {
            _root = input;
        }

        public void SetRoot(Input input)
        {
            _root = input;
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(AST other)
        {
            if (other == null)
                return false;

            return _root.Equals(other._root);
        }

        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            return Equals(obj as AST);
        }

        public override int GetHashCode()
        {
            return _root.GetHashCode();
        }
    }
}