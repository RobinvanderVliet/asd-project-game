using System.Collections.Generic;

namespace Agent.antlr.ast
{
    public class Action : Node
    {
        private List<Node> conditions = new List<Node>();

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
            children.AddRange(this.conditions);
            children.AddRange(this.body);
            return children;
        }

        public override Node AddChild(Node node)
        {
            if (node is Condition condition)
                this.conditions.Add(condition);
            else
                this.body.Add(node);

            return this;
        }

        public override Node RemoveChild(Node node)
        {
            if (node is Condition condition)
                conditions.Remove(condition);
            else
                body.Remove(node);

            return this;
        }
    }
}