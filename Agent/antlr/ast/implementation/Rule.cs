using System.Collections;

namespace Agent.antlr.ast
{
    /*
     * 
     * @author Abdul     
    */
    public class Rule : Node, IRule
    {
        private ArrayList settings = new ArrayList();
        private ArrayList body = new ArrayList();

        public string SettingName { get; set; }
        public string Value { get; set; }

        public string GetNodeType()
        {
            return "Rule";
        }

        public new ArrayList GetChildren()
        {
            var children = new ArrayList();
            children.AddRange(settings);
            children.AddRange(body);
            return children;
        }

        public new Node AddChild(INode node)
        {
            if (node is Setting setting)
                settings.Add(setting);
            else
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