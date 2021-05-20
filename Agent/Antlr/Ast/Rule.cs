using System.Collections.Generic;

namespace Agent.Antlr.Ast
{
    
    public class Rule : Node
    {
        private List<Node> _settings = new List<Node>();

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
            children.AddRange(this._settings);
            children.AddRange(this.body);
            return children;
        }

        public override Node AddChild(Node node)
        {
            if (node is Setting setting)
                this._settings.Add(setting);
            else
                this.body.Add(node);

            return this;
        }

        public override Node RemoveChild(Node node)
        {
            if (node is Setting setting)
                this._settings.Remove(setting);
            else 
                this.body.Remove(node);
            return this;
        }
    }
}