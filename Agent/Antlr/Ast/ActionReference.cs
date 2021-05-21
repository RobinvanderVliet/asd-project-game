using System.Collections.Generic;
using Agent.Antlr.Ast.Comparables;

namespace Agent.Antlr.Ast
{
    public class ActionReference : Node
    {
        private Subject _subject;
        private Item _item;
        
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
            if (_subject != null)
                children.Add(_subject);
            children.AddRange(body);
            
            return children;
        }

        public override Node AddChild(Node node)
        {
            switch (node) {
                case Subject subject:
                    this._subject = subject;
                    break;
                case Item item:
                    this._item = item;
                    break;
                default:
                    this.body.Add(node);
                    break;
            }
            return this;
        }
    }
}