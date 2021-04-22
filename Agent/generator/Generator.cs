using Agent.antlr.ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Action = Agent.antlr.ast.Action;


namespace Agent
{
    public class Generator
    {
        StringBuilder stringBuilder = new StringBuilder();

        public string execute(AST ast) 
        {
            foreach (Node node in ast.root.GetChildren())
            {
                GenerateConfiguration(node);
            }

            return stringBuilder.ToString();
        }

        public void GenerateConfiguration(Node parent) 
        { 
            foreach(Node child in parent.GetChildren())
            {
                if (child is Rule)
                {
                    GenerateRule((Rule)child);
                }
                else
                {
                    GenerateSetting(child);
                }
            }
        }

        private void GenerateRule(Rule parent)
        {
            stringBuilder.Append(parent.SettingName).Append("=").Append(parent.Value);
            stringBuilder.Append(Environment.NewLine);
        }

        private void GenerateSetting(Node parent)
        {
            foreach (Node child in parent.GetChildren())
            {
                if (child is Action)
                {
                    GenerateAction((Action)child, ((Setting)parent).SettingName);
                }
                else
                {
                    GenerateCondition(child,((Setting)parent).SettingName);
                }
            }
        }

        private void GenerateAction(Action parent, string settingName)
        {
            stringBuilder.Append(settingName).Append("_").Append("????").Append("=").Append(parent.Name);
            foreach(Node child in parent.GetChildren())
            {
                GenerateCondition(child, settingName);
            }
        }

        private void GenerateCondition(Node parent, string settingName)
        {
            foreach (Node child in parent.GetChildren())
            {
                if (child is When)
                {
                    generateWhen(child, settingName, "true");
                }
                else 
                {
                    generateOther(child, settingName, "false");
                }
            }
        }

        private void generateWhen(Node parent, string settingName, string v)
        {
            foreach (Node child in parent.GetChildren()) 
            {
                switch (child.GetNodeType()) {
                    case "comparable":
                        stringBuilder.Append(settingName).Append("_").Append("????").Append("_").Append("????")
                            .Append("_").Append("comparable").Append("=").Append(((When)parent).GetComparableL().ToString());
                        stringBuilder.Append(Environment.NewLine);
                        break;
                    case "treshold":
                        stringBuilder.Append(settingName).Append("_").Append("????").Append("_").Append("????").Append("_")
                            .Append("threshold").Append("=").Append(((When)parent).GetComparableR().ToString());
                        stringBuilder.Append(Environment.NewLine);
                        break;
                    case "comparision":
                        stringBuilder.Append(settingName).Append("_").Append("????").Append("_").Append("????").Append("_")
                            .Append("comparision").Append("=").Append(((When)parent).GetComparison().ToString());
                        stringBuilder.Append(Environment.NewLine);
                        break;
                    case "then":
                        stringBuilder.Append(settingName).Append("_").Append("????").Append("_").Append("????").Append("_")
                            .Append("Comparision").Append("_").Append(v).Append("=").Append(((When)parent).GetThen().ToString());
                        stringBuilder.Append(Environment.NewLine);
                        break;
                }
            }   
        }

        private void generateOther(Node parent, string settingName, string v)
        {
            stringBuilder.Append(settingName).Append("_").Append("????").Append("_").Append("????").Append("_")
                .Append("Comparision").Append("_").Append(v).Append("=")
                .Append(((ActionReference)((Otherwise)parent).GetChildren().FirstOrDefault()).Name);
            stringBuilder.Append(Environment.NewLine);
        }
    }
}
