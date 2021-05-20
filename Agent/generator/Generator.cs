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
        private string _stringBuilder = "";

        public string Execute(AST ast)
        {
            try
            {
               Parallel.ForEach(ast.root.GetChildren(), node =>
               {
                   GenerateConfiguration(node);
               });
            }
            catch (Exception e)
            {
                _stringBuilder += e.Message;
            }

            return _stringBuilder;
        }

        private void GenerateConfiguration(Node parent) 
        {
            string text = "";
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
            _stringBuilder += text;
        }

        private string GenerateRule(Rule parent)
        {
            return parent.SettingName + "=" + parent.Value + Environment.NewLine;
        }

        private string GenerateCondition(Node parent, string setting, string action)
        {
            string text = ""; 
            foreach (Node child in parent.GetChildren())
            {
                string subject = GenerateCompareble(((Condition)parent).GetWhenClause().GetComparableL());
                text += GenerateClause(child, setting, action, subject);
            }
            return text;
        }

        private string GenerateAction(Action parent, string settingName)
        {
            String text = settingName + "_" + parent.Name + "=" + parent.Name + Environment.NewLine;
            foreach(Node child in parent.GetChildren())
            {
                text += GenerateCondition(child, settingName, parent.Name);
            }
            return text;
        }

        private string GenerateClause(Node parent, String settingName, String action ,String subject)
        {
            string text = "";
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

        private string generateWhen(Node parent, String settingName, String action ,String subject ,String status)
        {
            string text = "";
            Parallel.For(0, parent.GetChildren().Count, i =>
            {
                switch (i)
                {
                    case 0:
                        text += settingName + "_" + action + "_" + subject + "_" + "comparable" + "=" + GenerateCompareble(((When)parent).GetComparableL()) + Environment.NewLine;
                        break;
                    case 1:
                        text += settingName + "_" + action + "_" + subject + "_" + "treshold" + "=" + GenerateCompareble(((When)parent).GetComparableR()) + Environment.NewLine; 
                        break;
                    case 2:
                        text += settingName + "_" + action + "_" + subject + "_" + "comparision" + "=" + ((When)parent).GetComparison().ComparisonType + Environment.NewLine;
                        break;
                    case 3:
                        text += settingName + "_" + action + "_" + subject + "_" + "comparision" + "_"+ status + "=" + ((When)parent).GetThen().Name + Environment.NewLine;
                        break;
                }
            });
            return text;
        }

        private string generateOther(Node parent, string settingName, string action ,string subject ,string status)
        {
            return settingName + "_" + action + "_" + subject + "_" + "comparision" + "_" + status + "=" + ((ActionReference)((Otherwise)parent).GetChildren().FirstOrDefault()).Name + Environment.NewLine;
        }

        private string GenerateCompareble(Comparable node)
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
