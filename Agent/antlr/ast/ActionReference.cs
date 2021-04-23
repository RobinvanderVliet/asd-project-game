using Agent.antlr.ast.comparables;
using System.Collections.Generic;

namespace Agent.antlr.ast
{
    public class ActionReference : Node
    {
        private Subject subject;
        private Item item;
        
        public string Name { get; set; }
        
        public ActionReference(string name)
        {
            Name = name;
        }


        public override string GetNodeType()
        {
            return "ActionReference";
        }

        public override List<Node> GetChildren()
        {
            var children = new List<Node>();
            if (item != null)
                children.Add(item);
            if (subject != null)
                children.Add(subject);
            children.AddRange(body);
            
            return children;
        }

        public override Node AddChild(Node node)
        {
            switch (node) {
                case Subject subject:
                    this.subject = subject;
                    break;
                case Item item:
                    this.item = item;
                    break;
                default:
                    this.body.Add(node);
                    break;
            }
            return this;
        }
    }
}