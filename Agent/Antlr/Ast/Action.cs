using System.Collections.Generic;

namespace Agent.Antlr.Ast
{
    public class Action : Node
    {
        private List<Node> _conditions = new List<Node>();
        public List<Node> Conditions { get => _conditions; }
        private string _name;
        public string Name { get => _name;}

        public Action(string name)
        {
            _name = name;
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