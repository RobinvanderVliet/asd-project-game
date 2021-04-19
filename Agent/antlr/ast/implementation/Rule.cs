using Agent.antlr.ast.interfaces;
using System.Collections;

namespace Agent.antlr.ast.implementation
{
    /*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: [to be determined].
 
    This file is created by team: 1.
     
    Goal of this file: [making_the_system_work].
     
    */
    
    public class Rule : Node, IRule
    {
        private ArrayList settings = new ArrayList();

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

        public new ArrayList GetChildren()
        {
            var children = new ArrayList();
            children.AddRange(settings);
            children.AddRange(body);
            return children;
        }

        public new INode AddChild(INode node)
        {
            if (node is ISetting setting)
                settings.Add(setting);
            else
                body.Add(node);

            return this;
        }

        public new INode RemoveChild(INode node)
        {
            if (node is ISetting setting)
                settings.Remove(setting);
            else 
                body.Remove(node);
            return this;
        }
    }
}