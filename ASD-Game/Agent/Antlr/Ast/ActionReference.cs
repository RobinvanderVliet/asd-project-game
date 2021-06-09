using System.Collections.Generic;
using ASD_Game.Agent.Antlr.Ast.Comparables;

namespace ASD_Game.Agent.Antlr.Ast
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
            switch (node)
            {
                case Subject subject:
                    _subject = subject;
                    break;
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