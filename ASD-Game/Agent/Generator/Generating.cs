using Agent.Antlr.Ast;
using Agent.Antlr.Ast.Comparables;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Action = Agent.Antlr.Ast.Action;

namespace Agent.Generator
{
    public class Generating
    {
        protected StringBuilder _stringBuilder;

        public Generating()
        {
            _stringBuilder = new StringBuilder();
        }

        public string Execute(AST ast)
        {
            try
            {

                Parallel.ForEach(ast.root.GetChildren(), node =>
                {
                    string text = "";
                    text += GenerateConfiguration(node);
                    lock (_stringBuilder)
                    {
                        _stringBuilder.Append(text);
                    }
                });
            }
            catch (Exception e)
            {
                _stringBuilder.Append(e.Message);
            }
            return _stringBuilder.ToString();
        }
        public string GenerateConfiguration(Node parent)
        {
            StringBuilder builder = new();

            var parentRule = parent as Rule;
            if (parentRule != null)
            {
                builder.Append(GenerateRule((Rule)parent));
            }

            foreach (Node child in parent.GetChildren())
            {
                if (child is Action)
                {
                    builder.Append(GenerateAction((Action)child, ((Setting)parent).SettingName));
                }
                else
                {
                    builder.Append(GenerateCondition(child, ((Setting)parent).SettingName, "default"));
                }
            }
            return builder.ToString();
        }
        private string GenerateRule(Rule parent)
        {
            return parent.SettingName + "=" + parent.Value + Environment.NewLine;
        }
        private string GenerateCondition(Node parent, string setting, string action)
        {
            StringBuilder builder = new();
            foreach (Node child in parent.GetChildren())
            {
                string subject = GenerateCompareble(((Condition)parent).GetWhenClause().GetComparableL());
                builder.Append(GenerateClause(child, setting, action, subject));
            }
            return builder.ToString();
        }
        private string GenerateAction(Action parent, string settingName)
        {
            StringBuilder builder = new(); 
            builder.Append(settingName + "_" + parent.Name + "=" + parent.Name + Environment.NewLine);
            foreach (Node child in parent.GetChildren())
            {
                builder.Append(GenerateCondition(child, settingName, parent.Name));
            }
            return builder.ToString();
        }
        private string GenerateClause(Node parent, string settingName, string action, string subject)
        {
            StringBuilder builder = new();
            if (parent is When)
            {
                builder.Append(generateWhen(parent, settingName, action, subject, "true"));
                if (parent.GetChildren().FirstOrDefault(c => c.GetNodeType() == "Otherwise") != null)
                {
                    builder.Append(generateOther(parent.GetChildren().FirstOrDefault(c => c.GetNodeType() == "Otherwise"), settingName, action, subject, "false"));
                }
            }
            return builder.ToString();
        }
        private string generateWhen(Node parent, string settingName, string action, string subject, string status)
        {
            StringBuilder builder = new();
            for (int i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 0:
                        builder.Append(settingName + "_" + action + "_" + subject + "_" + "comparable" + "=" + GenerateCompareble(((When)parent).GetComparableL()) + Environment.NewLine);
                        break;
                    case 1:
                        builder.Append(settingName + "_" + action + "_" + subject + "_" + "treshold" + "=" + GenerateCompareble(((When)parent).GetComparableR()) + Environment.NewLine);
                        break;
                    case 2:
                        builder.Append(settingName + "_" + action + "_" + subject + "_" + "comparision" + "=" + ((When)parent).GetComparison().ComparisonType + Environment.NewLine);
                        break;
                    case 3:
                        builder.Append(settingName + "_" + action + "_" + subject + "_" + "comparision" + "_" + status + "=" + ((When)parent).GetThen().Name + Environment.NewLine);
                        break;
                }
            }
            return builder.ToString();
        }
        private string generateOther(Node parent, string settingName, string action, string subject, string status)
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
            else if (nodeBase.Contains("Subjects"))
            {
                return (((Subject)node).Name);
            }
            else { return ""; }
        }
    }
}
