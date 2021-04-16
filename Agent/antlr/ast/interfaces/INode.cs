using System.Collections;

namespace Agent
{
    public interface INode
    {
        string GetNodeType();
        
        ArrayList GetChildren();
        
        void AddChild(INode node);

        void RemoveChild(INode node);
    }
}