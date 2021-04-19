using Agent.antlr.ast.interfaces;
using System.Collections;

namespace Agent.antlr.ast.implementation
{
    /*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: [to be determined].

    This file is created by team: 1.
     
    Goal of this file: [making_the_system_work].
     
    */
    public class Otherwise : Node, IOtherwise
    {
        private IActionReference _action;
        
        public string Value { get; set; }
        
        public new string GetNodeType()
        {
            return "Otherwise";
        }

        public new ArrayList GetChildren()
        {
            var children = new ArrayList() {
                this._action
            };
            children.AddRange(this.body);
            return children;
        }

        public new INode AddChild(INode node)
        {
            if (node is IActionReference actionReference) {
                this._action = actionReference;
            }
            else {
                this.body.Add(node);
            }

            return this;
        }
    }
}