using Agent.Antlr.Ast.Comparables;
using System.Collections.Generic;

namespace Agent.Antlr.Ast
{
    public class ActionReference : Node
    {
        public Item Item;
        public readonly string Name;
        
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
            if (Item != null)
                children.Add(Item);
            children.AddRange(body);

            return children;
        }

        public override Node AddChild(Node node)
        {
            switch (node) {
                case Item item:
                    Item = item;
                    break;
                default:
                    body.Add(node);
                    break;
            }
            return this;
        }
    }
}