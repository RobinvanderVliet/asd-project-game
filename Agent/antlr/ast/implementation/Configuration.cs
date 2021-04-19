using System.Collections;
using System.IO.Compression;

namespace Agent.antlr.ast
{
    /*
     * 
     * @author Abdul     
    */
    public class Configuration : Node, IConfiguration
    {
        private ArrayList body;

        public Configuration()
        {
            this.body = new ArrayList();
        }

        public Configuration(ArrayList body)
        {
            this.body = body;
        }
        
        public new string GetNodeType()
        {
            return "Configuration";
        }

        public new ArrayList GetChildren()
        {
            return this.body;
        }

        public new Node AddChild(INode node)
        {
            body.Add(node);
            return this;
        }

        public new Node RemoveChild(INode node)
        {
            body.Remove(node);
            return this;
        }
    }
}