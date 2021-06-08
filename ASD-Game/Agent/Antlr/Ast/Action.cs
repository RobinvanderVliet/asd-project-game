using System.Collections.Generic;

namespace Agent.Antlr.Ast
{
    public class Action : Node
    {
        private readonly List<Node> _conditions = new List<Node>();

        public string Name { get; set; }

        public Action(string name)
        {
            Name = name;
        }


        public override string GetNodeType()
        {
            return "Action";
        }

        public override List<Node> GetChildren()
        {
            var children = new List<Node>();
            children.AddRange(_conditions);
            children.AddRange(body);
            return children;
        }

        public override Node AddChild(Node node)
        {
            if (node is Condition condition)
                _conditions.Add(condition);
            else
                body.Add(node);

            return this;
        }

        public override Node RemoveChild(Node node)
        {
            if (node is Condition condition)
                _conditions.Remove(condition);
            else
                body.Remove(node);

            return this;
        }
    }
}