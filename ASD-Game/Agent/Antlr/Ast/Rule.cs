using System.Collections.Generic;

namespace ASD_Game.Agent.Antlr.Ast
{

    public class Rule : Node
    {
        public readonly List<Node> Settings = new();
        public readonly string SettingName;
        public readonly string Value;


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
            children.AddRange(Settings);
            children.AddRange(body);
            return children;
        }

        public override Node AddChild(Node node)
        {
            if (node is Setting setting)
                Settings.Add(setting);
            else
                body.Add(node);

            return this;
        }

        public override Node RemoveChild(Node node)
        {
            if (node is Setting setting)
                Settings.Remove(setting);
            else 
                body.Remove(node);
            return this;
        }
    }
}