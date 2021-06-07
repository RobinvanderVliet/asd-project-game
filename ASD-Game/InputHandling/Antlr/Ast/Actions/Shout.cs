using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace ASD_Game.InputHandling.Antlr.Ast.Actions
{
    public class Shout : Command, IEquatable<Shout>
    {
        private Message _message;
        public Message Message { get => _message; private set => _message = value; }

        public ArrayList GetChildren()
        {
            var children = new ArrayList();
            children.Add(_message);
            return children;
        }

        public override ASTNode AddChild(ASTNode child)
        {
            if (child is Message)
            {
                _message = (Message)child;
            }

            return this;
        }

        public ASTNode RemoveChild(ASTNode child)
        {
            if (child is Message && child == _message)
            {
                _message = null;
            }

            return this;
        }

        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            return Equals(obj as Shout);
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(Shout other)
        {
            if (other == null)
                return false;

            return _message.Equals(other._message);
        }
        public override int GetHashCode()
        {
            return _message.GetHashCode();
        }
    }
}

