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
    public class Action : Node, IAction
    {
        private ArrayList conditions = new ArrayList();

        public string Name { get; set; }
        
        public Action(string name)
        {
            Name = name;
        }


        public new string GetNodeType()
        {
            return "Action";
        }

        public new ArrayList GetChildren()
        {
            var children = new ArrayList();
            children.AddRange(this.conditions);
            children.AddRange(this.body);
            return children;
        }

        public new INode AddChild(INode node)
        {
            if (node is ICondition condition) {
                this.conditions.Add(condition);
            }
            else {
                this.body.Add(node);
            }

            return this;
        }

    }
}