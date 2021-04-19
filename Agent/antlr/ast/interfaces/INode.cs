using System.Collections;
using Agent.antlr.ast;

namespace Agent
{
    public interface INode
    {
        string GetNodeType();
        
        ArrayList GetChildren();
        
        Node AddChild(INode node);

        Node RemoveChild(INode node);
    }
}