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
    public class Node : INode
    {

        protected ArrayList body = new ArrayList();
        
        public string GetNodeType()
        {
            return "Node";
        }

        public ArrayList GetChildren()
        {
            return this.body;
        }

        public INode AddChild(INode node)
        {
            body.Add(node);
            return this;
        }

        public INode RemoveChild(INode node)
        {
            body.Remove(node);
            return this;
        }
    }
}