using System.Collections.Generic;

namespace Agent.antlr.ast
{
    
    public class Setting : Node
    {
        
        private List<Node> conditions = new List<Node>();
        private List<Node> actions = new List<Node>();
        
        public string SettingName { get; set; }
        
        public Setting(string settingName)
        {
            SettingName = settingName;
        }


        public override string GetNodeType()
        {
            return "Setting";
        }

        public override List<Node> GetChildren()
        {
            var children = new List<Node>();
            children.AddRange(conditions);
            children.AddRange(actions);
            children.AddRange(body);
            return children;
        }

        public override Node AddChild(Node node)
        {
            switch (node) 
            {
                case Condition:
                    conditions.Add(node);
                    break;
                case Action:
                    actions.Add(node);
                    break;
                default:
                    body.Add(node);
                    break;
                
            }
            return this;
        }
    }
}