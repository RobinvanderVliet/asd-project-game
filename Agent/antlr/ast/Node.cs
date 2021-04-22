using System.Collections.Generic;
using System.Text;

namespace Agent.antlr.ast
{
    /*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: [to be determined].

    This file is created by team: 1.
     
    Goal of this file: [making_the_system_work].
     
    */
 
    
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
            return this.body;
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
            return this.error;
        }

        public void SetError(string message)
        {
            this.error = new ASTError(message);
        }

        private string BuildString(StringBuilder builder)
        {
            builder.Append("[" + this.GetNodeType() + "]");
            foreach (var child in this.GetChildren()) {
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