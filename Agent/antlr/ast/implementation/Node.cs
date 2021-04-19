using System.Collections;

namespace Agent.antlr.ast
{
    /*
     * 
     * @author Abdul     
    */
    public class Node : INode
    {

        private ArrayList children;
        
        public string GetNodeType()
        {
            //TODO moet voor elke node override worden
            return "Node";
        }

        public ArrayList GetChildren()
        {
            //TODO
            return new ArrayList();
        }

        public Node AddChild(INode node)
        {
            //TODO
            return this;
        }

        public Node RemoveChild(INode node)
        {
            //TODO
            return this;
        }
    }
}