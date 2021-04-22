using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Chat.antlr.ast.actions
{
    public class Say : Command, IEquatable<Say>
    {
        public Message message;

        public override ArrayList getChildren()
        {
            var children = new ArrayList();
            children.Add(message);
            return children;
        }

        public override ASTNode addChild(ASTNode child)
        {
            if (child is Message)
            {
                message = (Message) child;
            }

            return this;
        }

        public override ASTNode removeChild(ASTNode child)
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
            return this.Equals(obj as Say);
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(Say other)
        {
            if (other == null)
                return false;

            return message.Equals(other.message);
        }
    }
}