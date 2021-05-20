using Agent.Antlr.Ast.Comparables;
using System.Collections.Generic;

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
            if (this._item != null)
                children.Add(this._item);
            if (this._subject != null)
                children.Add(this._subject);
            children.AddRange(this.body);
            
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