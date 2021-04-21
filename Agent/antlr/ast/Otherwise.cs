using System.Collections.Generic;

namespace Agent.antlr.ast
{
    /*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: [to be determined].

    This file is created by team: 1.
     
    Goal of this file: [making_the_system_work].
     
    */
    public class Otherwise : Node
    {
        private ActionReference _action;
        
        public string Value { get; set; }
        
        public new string GetNodeType()
        {
            return "Otherwise";
        }

        public new List<Node> GetChildren()
        {
            var children = new List<Node>() {
                this._action
            };
            children.AddRange(this.body);
            return children;
        }

        public new Node AddChild(Node node)
        {
            if (node is ActionReference actionReference) {
                this._action = actionReference;
            }
            else {
                this.body.Add(node);
            }

            return this;
        }
    }
}