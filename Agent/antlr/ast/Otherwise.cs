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
        
        public override string GetNodeType()
        {
            return "Otherwise";
        }

        public override List<Node> GetChildren()
        {
            var children = new List<Node>();
            if(this._action!= null){
                children.Add(this._action);
            };
            children.AddRange(this.body);
            return children;
        }

        public override Node AddChild(Node node)
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