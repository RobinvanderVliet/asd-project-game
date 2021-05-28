using System.Collections.Generic;

namespace Agent.Antlr.Ast
{
    public class Action : Node
    {
        public List<Node> Conditions {get ;}
        public string Name { get;}

        public Action(string name)
        {
            Conditions = new();
            Name = name;
        }


        public override string GetNodeType()
        {
            return "Action";
        }

        public override List<Node> GetChildren()
        {
            var children = new List<Node>();
            children.AddRange(Conditions);
            children.AddRange(body);
            return children;
        }

        public override Node AddChild(Node node)
        {
            if (node is Condition condition)
                Conditions.Add(condition);
            else
                body.Add(node);

            return this;
        }

        public override Node RemoveChild(Node node)
        {
            if (node is Condition condition)
                Conditions.Remove(condition);
            else
                body.Remove(node);

            return this;
        }
    }
}