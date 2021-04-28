using Agent.antlr.ast;
using Agent.antlr.ast.comparables;
using System;
using System.Linq;
using Action = Agent.antlr.ast.Action;
using System.Threading.Tasks;

namespace Agent
{
    public class Generator
    {
        String stringBuilder = "";

        public String Execute(AST ast)
        {
            Parallel.ForEach (ast.root.GetChildren(), node =>
            {
                GenerateConfiguration(node);
            });

            return stringBuilder;
        }

        public void GenerateConfiguration(Node parent) 
        {
            String text = "";
            if (parent is Rule)
            {
                text += GenerateRule((Rule)parent);
            }

            foreach (Node child in parent.GetChildren())
            {
                if (child is Action)
                {
                    text += GenerateAction((Action)child, ((Setting)parent).SettingName);
                } 
                else 
                {
                    text += GenerateCondition(child, ((Setting)parent).SettingName, "default");
                }
            }
            stringBuilder += text;
        }

        private String GenerateRule(Rule parent)
        {
            return parent.SettingName + "=" + parent.Value + Environment.NewLine;
        }

        private String GenerateCondition(Node parent, String setting, String action)
        {
            String text = ""; 
            foreach (Node child in parent.GetChildren())
            {
                text += GenerateClause(child, setting, action ,GenerateCompareble(((Condition)parent).GetWhenClause().GetComparableL()));
            }
            return text;
        }

        private String GenerateAction(Action parent, string settingName)
        {
            String text = settingName + "_" + parent.Name + "=" + parent.Name + Environment.NewLine;
            foreach(Node child in parent.GetChildren())
            {
                text += GenerateCondition(child, settingName, parent.Name);
            }
            return text;
        }

        private String GenerateClause(Node parent, string settingName, String action ,String subject)
        {
            String text = "";
            if (parent is When)
            {
                text += generateWhen(parent, settingName, action, subject ,"true");
                if(parent.GetChildren().Where(c => c.GetNodeType() == "Otherwise").FirstOrDefault() != null)
                {
                    text += generateOther(parent.GetChildren().Where(c => c.GetNodeType() == "Otherwise").FirstOrDefault(), settingName, action, subject ,"false");
                }
            }
            return text;
        }

        private String generateWhen(Node parent, string settingName, String action ,String subject ,string status)
        {
            String text = "";
            Parallel.For(0, parent.GetChildren().Count, i =>
            {
                switch (i)
                {
                    case 0:
                        text += settingName + "_" + subject + "_" + action + "_" + "comparable" + "=" + GenerateCompareble(((When)parent).GetComparableL()) + Environment.NewLine;
                        break;
                    case 1:
                        text += settingName + "_" + subject + "_" + action + "_" + "treshold" + "=" + GenerateCompareble(((When)parent).GetComparableR()) + Environment.NewLine; 
                        break;
                    case 2:
                        text += settingName + "_" + subject + "_" + action + "_" + "comparision" + "=" + ((When)parent).GetComparison().ComparisonType + Environment.NewLine;
                        break;
                    case 3:
                        text += settingName + "_" + subject + "_" + action + "_" + "comparision" + "_"+ status + "=" + ((When)parent).GetThen().Name + Environment.NewLine;
                        break;
                }
            });
            return text;
        }

        private String generateOther(Node parent, string settingName, String action ,String subject ,string status)
        {
            return settingName + "_" + subject + "_" + action + "_" + "comparision" + "_" + status + "=" + ((ActionReference)((Otherwise)parent).GetChildren().FirstOrDefault()).Name + Environment.NewLine;
        }

        private String GenerateCompareble(Comparable node)
        {
            var nodeBase = node.GetType().FullName;
            if (nodeBase.Contains("Item"))
            {
                return (((Item)node).Name);
            }
            else if (nodeBase.Contains("Int"))
            {
                return ((Int)node).Value.ToString();
            }
            else if (nodeBase.Contains("Stat"))
            {
                return (((Stat)node).Name);
            }
            else if (nodeBase.Contains("subjects"))
            {
                return (((Subject)node).Name);
            }
            else { return "";}
        }
    }
}
