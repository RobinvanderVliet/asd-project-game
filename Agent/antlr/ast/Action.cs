using System.Collections.Generic;

namespace Agent.antlr.ast
{
    /*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: [to be determined].

    This file is created by team: 1.
     
    Goal of this file: [making_the_system_work].
     
    */
    public class Action : Node
    {
        private List<Node> conditions = new List<Node>();

        public string Name { get; set; }

        public Action(string name)
        {
            Name = name;
        }


        public new string GetNodeType()
        {
            return "Action";
        }

        public new List<Node> GetChildren()
        {
            var children = new List<Node>();
            children.AddRange(this.conditions);
            children.AddRange(this.body);
            return children;
        }

        public new Node AddChild(Node node)
        {
            if (node is Condition condition)
                this.conditions.Add(condition);
            else
                this.body.Add(node);

            return this;
        }

        public new Node RemoveChild(Node node)
        {
            if (node is Condition condition)
                conditions.Remove(condition);
            else
                body.Remove(node);

            return this;
        }
    }
}