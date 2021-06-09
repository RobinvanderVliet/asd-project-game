using System.Collections.Generic;

namespace ASD_Game.Agent.Antlr.Ast
{

    public class Setting : Node
    {
        
        public readonly List<Node> Conditions = new ();
        public readonly List<Node> Actions = new ();

        public readonly string SettingName;
        
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
            children.AddRange(Conditions);
            children.AddRange(Actions);
            children.AddRange(body);
            return children;
        }

        public override Node AddChild(Node node)
        {
            switch (node)
            {
                case Condition:
                    Conditions.Add(node);
                    break;
                case Action:
                    Actions.Add(node);
                    break;
                default:
                    body.Add(node);
                    break;

            }
            return this;
        }
    }
}