using System.Collections.Generic;

namespace Agent.Antlr.Ast
{
    
    public class Setting : Node
    {
        
        private List<Node> _conditions = new List<Node>();
        private List<Node> _actions = new List<Node>();
        
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
            children.AddRange(this._conditions);
            children.AddRange(this._actions);
            children.AddRange(this.body);
            return children;
        }

        public override Node AddChild(Node node)
        {
            switch (node) 
            {
                case Condition:
                    this._conditions.Add(node);
                    break;
                case Action:
                    this._actions.Add(node);
                    break;
                default:
                    this.body.Add(node);
                    break;
                
            }
            return this;
        }
    }
}