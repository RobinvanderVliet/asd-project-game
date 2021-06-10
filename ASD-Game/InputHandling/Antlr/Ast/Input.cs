using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace ASD_Game.InputHandling.Antlr.Ast
{
    public class Input : ASTNode, IEquatable<Input>
    {
        private ArrayList _body;
        public ArrayList Body { get => _body; private set => _body = value; }

        public Input()
        {
            _body = new ArrayList();
        }

        public override ASTNode AddChild(ASTNode child)
        {
            _body.Add(child);
            return this;
        }

        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            return Equals(obj as Input);
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(Input other)
        {
            if (other == null)
                return false;

            if (_body.Count != other._body.Count)
                return false;

            for (int i = 0; i < _body.Count; i++)
            {
                if (!_body[i].Equals(other._body[i]))
                {
                    return false;
                }
            }
            return true;
        }
        public override int GetHashCode()
        {
            return _body.GetHashCode();
        }

    }
}