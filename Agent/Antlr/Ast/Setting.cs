using System.Collections.Generic;

namespace Agent.Antlr.Ast
{
    
    public class Setting : Node
    {
        
        private readonly List<Node> _conditions = new ();
        private readonly List<Node> _actions = new ();

        public string SettingName {get ;}
        
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
            children.AddRange(_conditions);
            children.AddRange(_actions);
            children.AddRange(body);
            return children;
        }

        public override Node AddChild(Node node)
        {
            switch (node) 
            {
                case Condition:
                    _conditions.Add(node);
                    break;
                case Action:
                    _actions.Add(node);
                    break;
                default:
                    body.Add(node);
                    break;
                
            }
            return this;
        }
    }
}