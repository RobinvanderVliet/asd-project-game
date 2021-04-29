using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace InputCommandHandler.Antlr.Ast.Actions
{
    public class Shout : Command, IEquatable<Shout>
    {
        public Message message;

        public ArrayList GetChildren()
        {
            var children = new ArrayList();
            children.Add(message);
            return children;
        }

        public override ASTNode AddChild(ASTNode child)
        {
            if (child is Message)
            {
                message = (Message) child;
            }

            return this;
        }

        public ASTNode RemoveChild(ASTNode child)
        {
            if (child is Message && child == message)
            {
                message = null;
            }

            return this;
        }

        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            return this.Equals(obj as Shout);
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(Shout other)
        {
            if (other == null)
                return false;

            return message.Equals(other.message);
        }
    }
}

