using Agent.antlr.ast.comparables;
using System.Collections.Generic;

namespace Agent.antlr.ast
{
    /*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: [to be determined].

    This file is created by team: 1.
     
    Goal of this file: [making_the_system_work].
     
    */
    public class ActionReference : Node
    {
        private Subject subject;
        private Item item;
        
        public string Name { get; set; }
        
        public ActionReference(string name)
        {
            Name = name;
        }


        public new string GetNodeType()
        {
            return "ActionReference";
        }

        public new List<Node> GetChildren()
        {
            var children = new List<Node>();
            if (item != null)
                children.Add(item);
            if (subject != null)
                children.Add(subject);
            children.AddRange(body);
            
            return children;
        }

        public new Node AddChild(Node node)
        {
            switch (node) {
                case Subject subject:
                    this.subject = subject;
                    break;
                case Item item:
                    this.item = item;
                    break;
                default:
                    this.body.Add(node);
                    break;
            }
            return this;
        }
    }
}