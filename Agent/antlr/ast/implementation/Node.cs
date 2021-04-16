using System.Collections;

namespace Agent.antlr.ast
{
    public class Node : INode
    {

        private ArrayList children;
        
        public string GetNodeType()
        {
            //TODO
            return "TODO";
        }

        public ArrayList GetChildren()
        {
            //TODO
            return new ArrayList();
        }

        public void AddChild(INode node)
        {
            //TODO
        }

        public void RemoveChild(INode node)
        {
            //TODO
        }
    }
}