using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agent.antlr.grammar;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Agent.antlr.ast;
using Agent.antlr.ast.implementation;
using Agent.antlr.ast.implementation.comparables;
using Agent.antlr.ast.comparables;
using Agent.antlr.ast.implementation.comparables.subjects;
using Action = Agent.antlr.ast.Action;

namespace Agent.parser
{
    class ASTAgentListener : AgentConfigurationBaseListener
    {
        private AST ast;
        private Stack<Node> currentContainer;

        public ASTAgentListener()
        {
            ast = new AST();
            currentContainer = new Stack<Node>();
        }

        public AST GetAST(){ return ast; }

        public override void EnterAction([NotNull] AgentConfigurationParser.ActionContext context)
        {
            ActionReference reference = new ActionReference();
            reference.action = context.STRING();
            currentContainer.Push(reference);
        }

        public override void EnterActionBlock([NotNull] AgentConfigurationParser.ActionBlockContext context)
        {
            Action action = new Action(context.GetText());
            currentContainer.Peek().AddChild(action);
        }

        public override void EnterActionSubject([NotNull] AgentConfigurationParser.ActionSubjectContext context)
        {
            ActionReference reference = new ActionReference();
            reference.action = context.action().STRING();
            currentContainer.Push(reference);
        }

        public override void EnterComparable([NotNull] AgentConfigurationParser.ComparableContext context)
        {
            //base.EnterComparable();
        }

        public override void EnterComparison([NotNull] AgentConfigurationParser.ComparisonContext context)
        {
            Comparison comparison = new Comparison();
            currentContainer.Push(comparison);
        }

        public override void EnterCondition([NotNull] AgentConfigurationParser.ConditionContext context)
        {
            Condition condition = new Condition();
            currentContainer.Push(condition);
        }

        public override void EnterConfiguration([NotNull] AgentConfigurationParser.ConfigurationContext context)
        {
            Configuration configuration = new Configuration();
            currentContainer.Push(configuration);
            ast.SetRoot(configuration);
        }

        public override void EnterItem([NotNull] AgentConfigurationParser.ItemContext context)
        {
            Item item = new Item(context.children.Where(c => c.ToString() != null).FirstOrDefault().GetText());
            currentContainer.Push(item);
        }

        public override void EnterItemStat([NotNull] AgentConfigurationParser.ItemStatContext context)
        {
            /*
            Stat stat = new Stat();
            stat.Name = context.stat().children.Where(c => c.GetText() != null).FirstOrDefault().GetText();
            currentContainer.Push(stat);
            */
        }

        public override void EnterOtherwiseClause([NotNull] AgentConfigurationParser.OtherwiseClauseContext context)
        {
            Otherwise otherwise = new Otherwise();
            currentContainer.Push(otherwise);
        }

        public override void EnterRule([NotNull] AgentConfigurationParser.RuleContext context)
        {
            Rule rule = new Rule();
            rule.SetValue = context.STRING();
            currentContainer.Push(rule);
        }

        public override void EnterSetting([NotNull] AgentConfigurationParser.SettingContext context)
        {
            Setting node = (Setting)currentContainer.Pop();
            node.SettingName = context.GetText();
            currentContainer.Push(node);
        }

        public override void EnterSettingBlock([NotNull] AgentConfigurationParser.SettingBlockContext context)
        {
            Setting block = new Setting();
            currentContainer.Push(block);
        }

        public override void EnterStat([NotNull] AgentConfigurationParser.StatContext context)
        {
            Stat node = (Stat)currentContainer.Pop();
            node.Name = context.children.Where(c => c.GetText() != null).FirstOrDefault().GetText();
            currentContainer.Push(node);
        }

        public override void EnterString([NotNull] AgentConfigurationParser.StringContext context)
        {
            base.EnterString(context);
        }

        public override void EnterSubject([NotNull] AgentConfigurationParser.SubjectContext context)
        {
            String subject = context.children.Where(c => c.GetText() != null).FirstOrDefault().GetText();
            Subject node = CreateSubject(subject);
            currentContainer.Push(node);
        }

        private Subject CreateSubject(string subject)
        {
            return subject switch
            {
                "player" => new Player(),
                "npc" => new NPC(),
                "opponent" => new Opponent(),
                "inventory" => new Inventory(),
                "current" => new Current(),
                "tile" => new Tile(),
                _ => throw new Exception("subject is an invallid type:" + subject),
            };
        }

        public override void EnterSubjectStat([NotNull] AgentConfigurationParser.SubjectStatContext context)
        {
            /*
            Stat stat = new Stat();
            stat.Name = context.stat().children.Where(c => c.GetText() != null).FirstOrDefault().GetText();
            currentContainer.Push(stat);
            */
        }

        public override void EnterWhenClause([NotNull] AgentConfigurationParser.WhenClauseContext context)
        {
            When clause = new When();
            currentContainer.Push(clause);
        }

        /* ################################### */
        /* ##             EXIT              ## */
        /* ################################### */

        public override void ExitItem([NotNull] AgentConfigurationParser.ItemContext context)
        {
            Node temp = currentContainer.Pop();
            currentContainer.Peek().AddChild(temp);
        }

        public override void ExitOtherwiseClause([NotNull] AgentConfigurationParser.OtherwiseClauseContext context)
        {
            Node temp = currentContainer.Pop();
            currentContainer.Peek().AddChild(temp);
        }

        public override void ExitSetting([NotNull] AgentConfigurationParser.SettingContext context)
        {
            Node temp = currentContainer.Pop();
            currentContainer.Peek().AddChild(temp);
        }

        public override void ExitStat([NotNull] AgentConfigurationParser.StatContext context)
        {
            Node temp = currentContainer.Pop();
            currentContainer.Peek().AddChild(temp);
        }

        public override void ExitWhenClause([NotNull] AgentConfigurationParser.WhenClauseContext context)
        {
            Node temp = currentContainer.Pop();
            currentContainer.Peek().AddChild(temp);
        }

    }
}
