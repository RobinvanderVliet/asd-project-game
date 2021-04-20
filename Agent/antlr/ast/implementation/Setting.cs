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
    public class Setting : Node, ISetting
    {
        
        private ArrayList conditions = new ArrayList();
        private ArrayList actions = new ArrayList();
        
        public string SettingName { get; set; }
        
        public Setting(string settingName)
        {
            SettingName = settingName;
        }


        public new string GetNodeType()
        {
            return "Setting";
        }

        public new ArrayList GetChildren()
        {
            var children = new ArrayList();
            children.AddRange(conditions);
            children.AddRange(actions);
            children.AddRange(body);
            return children;
        }

        public new INode AddChild(INode node)
        {
            switch (node) 
            {
                case IConfiguration:
                    conditions.Add(node);
                    break;
                case IAction:
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