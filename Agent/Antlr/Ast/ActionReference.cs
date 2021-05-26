using Agent.Antlr.Ast.Comparables;
using System.Collections.Generic;

namespace Agent.Antlr.Ast
{
    public class ActionReference : Node
    {
        private Item _item;
        public Item Item {get => _item;}
        
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
            if (_item != null)
                children.Add(_item);
            children.AddRange(body);
            
            return children;
        }

        public override Node AddChild(Node node)
        {
            switch (node) {
                case Item item:
                    _item = item;
                    break;
                default:
                    body.Add(node);
                    break;
            }
            return this;
        }
    }
}