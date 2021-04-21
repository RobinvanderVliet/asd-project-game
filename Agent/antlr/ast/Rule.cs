using System.Collections.Generic;

namespace Agent.antlr.ast
{
    /*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: [to be determined].
 
    This file is created by team: 1.
     
    Goal of this file: [making_the_system_work].
     
    */
    
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
        
        public string GetNodeType()
        {
            return "Rule";
        }

        public new List<Node> GetChildren()
        {
            var children = new List<Node>();
            children.AddRange(settings);
            children.AddRange(body);
            return children;
        }

        public new Node AddChild(Node node)
        {
            if (node is Setting setting)
                settings.Add(setting);
            else
                body.Add(node);

            return this;
        }

        public new Node RemoveChild(Node node)
        {
            if (node is Setting setting)
                settings.Remove(setting);
            else 
                body.Remove(node);
            return this;
        }
    }
}