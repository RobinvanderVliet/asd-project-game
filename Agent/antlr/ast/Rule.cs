using System.Collections.Generic;

namespace Agent.antlr.ast
{
    
    public class Rule : Node
    {
        private List<Node> settings = new List<Node>();

        public string SettingName { get; set; }
        public string Value { get; set; }

        
        public Rule(string settingName, string value)
        {
            SettingName = settingName;
            Value = value;
        }
        
        public override string GetNodeType()
        {
            return "Rule";
        }

        public override List<Node> GetChildren()
        {
            var children = new List<Node>();
            children.AddRange(settings);
            children.AddRange(body);
            return children;
        }

        public override Node AddChild(Node node)
        {
            if (node is Setting setting)
                settings.Add(setting);
            else
                body.Add(node);

            return this;
        }

        public override Node RemoveChild(Node node)
        {
            if (node is Setting setting)
                settings.Remove(setting);
            else 
                body.Remove(node);
            return this;
        }
    }
}