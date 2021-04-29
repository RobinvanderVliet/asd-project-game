using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace InputCommandHandler.antlr.ast.actions
{
    public class Drop : Command, IEquatable<Drop>
    {
        public Message itemName;
        
        public ArrayList GetChildren()
        {
            var children = new ArrayList();
            children.Add(itemName);
            return children;
        }

        public override ASTNode AddChild(ASTNode child)
        {
            if (child is Message)
            {
                itemName = (Message) child;
            }

            return this;
        }

        public ASTNode RemoveChild(ASTNode child)
        {
            if (child is Message && child == itemName)
            {
                itemName = null;
            }

            return this;
        }
        
        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            return this.Equals(obj as Drop);
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(Drop other)
        {
            return true;
        }
    }
}