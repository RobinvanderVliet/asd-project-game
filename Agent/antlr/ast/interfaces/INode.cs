using System.Collections;

namespace Agent.antlr.ast.interfaces
{
    /*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: [to be determined].

    This file is created by team: 1.
     
    Goal of this file: [making_the_system_work].
     
    */
    public interface INode
    {
        string GetNodeType();
        
        ArrayList GetChildren();
        
        INode AddChild(INode node);

        INode RemoveChild(INode node);
    }
}