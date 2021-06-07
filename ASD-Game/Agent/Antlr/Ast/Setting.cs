using System.Collections.Generic;

namespace ASD_Game.Agent.Antlr.Ast
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