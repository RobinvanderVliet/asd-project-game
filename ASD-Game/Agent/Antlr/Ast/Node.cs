using System.Collections.Generic;
using System.Text;

namespace ASD_Game.Agent.Antlr.Ast
{

    public class Node
    {

        protected ASTError error = null;
        protected List<Node> body = new List<Node>();

        public virtual string GetNodeType()
        {
            return "Node";
        }

        public virtual List<Node> GetChildren()
        {
            return body;
        }

        public virtual Node AddChild(Node node)
        {
            body.Add(node);
            return this;
        }

        public virtual Node RemoveChild(Node node)
        {
            body.Remove(node);
            return this;
        }

        public ASTError GetError()
        {
            return error;
        }

        public void SetError(string message)
        {
            error = new ASTError(message);
        }

        public bool HasError()
        {
            return error != null;
        }

        private string BuildString(StringBuilder builder)
        {
            builder.Append("[" + GetNodeType() + "]");
            foreach (var child in GetChildren())
            {
                child.BuildString(builder);
            }

            return builder.ToString();
        }

        public override string ToString()
        {
            return BuildString(new StringBuilder()).ToString();
        }
    }
}