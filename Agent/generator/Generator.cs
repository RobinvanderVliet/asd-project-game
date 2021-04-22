using Agent.antlr.ast;
using Agent.antlr.ast.comparables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agent.antlr.ast.implementation;
using Action = Agent.antlr.ast.Action;


namespace Agent
{
    public class Generator
    {
        StringBuilder stringBuilder = new StringBuilder();
        String player;

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
            if (parent is Rule)
            {
                GenerateRule((Rule)parent);
            }
            foreach (Node child in parent.GetChildren())
            {

                if (child is Action)
                {
                    GenerateAction((Action)child, ((Setting)parent).SettingName);
                } 
                else 
                {
                    GenerateCondition(child, ((Setting)parent).SettingName);
                }
            }
        }

        private void GenerateRule(Rule parent)
        {
            stringBuilder.Append(parent.SettingName).Append("=").Append(parent.Value);
            stringBuilder.Append(Environment.NewLine);
        }

        private void GenerateCondition(Node parent, String setting)
        {
            foreach (Node child in parent.GetChildren())
            {
                GenerateClause(child, setting);
            }
        }

        private void GenerateAction(Action parent, string settingName)
        {
            stringBuilder.Append(settingName).Append("_").Append("????").Append("=").Append(parent.Name);
            stringBuilder.Append(Environment.NewLine);
            foreach(Node child in parent.GetChildren())
            {
                GenerateCondition(child, settingName);
            }
        }

        private void GenerateClause(Node parent, string settingName)
        {
            if (parent is When)
            {
                generateWhen(parent, settingName, "true");
            }
            else 
            {
                generateOther(parent, settingName, "false");
            }
        }

        private void generateWhen(Node parent, string settingName, string v)
        {
            for (int i = 0; i < parent.GetChildren().Count; i++)
            {
                switch (i)
                {
                    case 0:
                        stringBuilder.Append(settingName).Append("_").Append("????").Append("_").Append("????").Append("_")
                            .Append("comparable").Append("=");
                        GenerateCompareble(((When)parent).GetComparableL());
                        stringBuilder.Append(Environment.NewLine);
                        break;
                    case 1:
                        stringBuilder.Append(settingName).Append("_").Append("????").Append("_").Append("????").Append("_")
                            .Append("threshold").Append("=");
                        GenerateCompareble(((When)parent).GetComparableR());
                        stringBuilder.Append(Environment.NewLine);
                        break;
                    case 2:
                        stringBuilder.Append(settingName).Append("_").Append("????").Append("_").Append("????").Append("_")
                            .Append("comparision").Append("=").Append(((When)parent).GetComparison().ComparisonType);
                        stringBuilder.Append(Environment.NewLine);
                        break;
                    case 3:
                        stringBuilder.Append(settingName).Append("_").Append("????").Append("_").Append("????").Append("_")
                            .Append("comparision").Append("_").Append(v).Append("=").Append(((When)parent).GetThen().Name);
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

        private void GenerateCompareble(Comparable node)
        {
            var nodeBase = node.GetType().FullName;
            if (nodeBase.Contains("Item")) {
                stringBuilder.Append(((Item)node).Name);
            } else if (nodeBase.Contains("Int")) {
                stringBuilder.Append(((Int)node).Value);
            } else if (nodeBase.Contains("Stat")) {
                stringBuilder.Append(((Stat)node).Name);
            } else if (nodeBase.Contains("subjects")) {
                stringBuilder.Append(((Subject)node).Name);
            }
        }
    }
}
