using System;
using System.Collections.Generic;
using System.Linq;
using Agent.antlr.grammar;
using Antlr4.Runtime.Misc;
using Agent.antlr.ast.implementation;
using Agent.antlr.ast;
using Action = Agent.antlr.ast.Action;
using Agent.antlr.ast.comparables;
using Agent.antlr.ast.comparables.subjects;

namespace Agent.parser
{
    public class ASTAgentListener : AgentConfigurationBaseListener
    {
        private AST ast;
        private Stack<Node> currentContainer;

        public ASTAgentListener()
        {
            ast = new AST();
            currentContainer = new Stack<Node>();
        }

        public AST GetAST() { return ast; }

        public override void EnterAction([NotNull] AgentConfigurationParser.ActionContext context)
        {
            if (currentContainer.Peek() is When || currentContainer.Peek() is Otherwise)
            {
                ActionReference reference = new ActionReference(context.STRING().GetText());
                currentContainer.Push(reference);
            }
        }

        public override void EnterActionBlock([NotNull] AgentConfigurationParser.ActionBlockContext context)
        {
            Action action = new Action(context.children.FirstOrDefault().GetText());
            currentContainer.Push(action);
        }

        public override void EnterActionSubject([NotNull] AgentConfigurationParser.ActionSubjectContext context)
        {
            ActionReference reference = new ActionReference(context.action().STRING().GetText());
            Console.WriteLine("");
            currentContainer.Push(reference);
        }

        public override void EnterComparison([NotNull] AgentConfigurationParser.ComparisonContext context)
        {
            Comparison comparison = new Comparison(context.children.Where(c => c.GetText() != null).FirstOrDefault().GetText());
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
        }

        public override void EnterItem([NotNull] AgentConfigurationParser.ItemContext context)
        {
            String temp = context.children.FirstOrDefault().GetText();
            Item item = new Item(temp);
            currentContainer.Push(item);
        }

        public override void EnterItemStat([NotNull] AgentConfigurationParser.ItemStatContext context)
        {
            base.EnterItemStat(context);
        }

        public override void EnterOtherwiseClause([NotNull] AgentConfigurationParser.OtherwiseClauseContext context)
        {
            Otherwise otherwise = new Otherwise();
            currentContainer.Push(otherwise);
        }

        public override void EnterRule([NotNull] AgentConfigurationParser.RuleContext context)
        {
            Rule rule = new Rule(context.children.ElementAt(0).GetText(), context.children.ElementAt(2).GetText());
            currentContainer.Push(rule);
        }

        public override void EnterSettingBlock([NotNull] AgentConfigurationParser.SettingBlockContext context)
        {
            Setting block = new Setting(context.setting().children.Where(c => c.GetText() != null).FirstOrDefault().GetText());
            currentContainer.Push(block);
        }

        public override void EnterStat([NotNull] AgentConfigurationParser.StatContext context)
        {
            Stat stat = new Stat(context.children.Where(c => c.GetText() != null).FirstOrDefault().GetText());
            currentContainer.Push(stat);
        }

        public override void EnterSubject([NotNull] AgentConfigurationParser.SubjectContext context)
        {
            Subject subject = (Subject)context.children.Where(c => c.GetText() != null).FirstOrDefault();
            currentContainer.Push(subject);
        }


        public override void EnterSubjectStat([NotNull] AgentConfigurationParser.SubjectStatContext context)
        {
            base.EnterSubjectStat(context);
        }

        public override void EnterWhenClause([NotNull] AgentConfigurationParser.WhenClauseContext context)
        {
            When clause = new When();
            currentContainer.Push(clause);
        }

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

        public override void ExitConfiguration([NotNull] AgentConfigurationParser.ConfigurationContext context)
        {
            Node temp = currentContainer.Pop();
            ast.SetRoot((Configuration)temp);
        }

        public override void ExitRule([NotNull] AgentConfigurationParser.RuleContext context)
        {
            Node temp = currentContainer.Pop();
            currentContainer.Peek().AddChild(temp);
        }

        public override void ExitSettingBlock([NotNull] AgentConfigurationParser.SettingBlockContext context)
        {
            Node temp = currentContainer.Pop();
            currentContainer.Peek().AddChild(temp);
        }

        public override void ExitActionBlock([NotNull] AgentConfigurationParser.ActionBlockContext context)
        {
            Node temp = currentContainer.Pop();
            currentContainer.Peek().AddChild(temp);
        }

        public override void ExitCondition([NotNull] AgentConfigurationParser.ConditionContext context)
        {
            Node temp = currentContainer.Pop();
            currentContainer.Peek().AddChild(temp);
        }

        public override void ExitComparison([NotNull] AgentConfigurationParser.ComparisonContext context)
        {
            Node temp = currentContainer.Pop();
            currentContainer.Peek().AddChild(temp);
        }

        public override void ExitAction([NotNull] AgentConfigurationParser.ActionContext context)
        {
            if (currentContainer.Peek() is ActionReference && ((ActionReference)currentContainer.Peek()).Name != "use") 
            {
                Node temp = currentContainer.Pop();
                currentContainer.Peek().AddChild(temp);
            }
        }

        public override void ExitActionSubject([NotNull] AgentConfigurationParser.ActionSubjectContext context)
        {
            Node temp = currentContainer.Pop();
            currentContainer.Peek().AddChild(temp);
        }

        public override void ExitItemStat([NotNull] AgentConfigurationParser.ItemStatContext context)
        {
            base.ExitItemStat(context);
        }

        public override void ExitSubjectStat([NotNull] AgentConfigurationParser.SubjectStatContext context)
        {
            base.ExitSubjectStat(context);
        }

        public override void ExitSubject([NotNull] AgentConfigurationParser.SubjectContext context)
        {
            base.ExitSubject(context);
        }


        public override void EnterNpc([NotNull] AgentConfigurationParser.NpcContext context)
        {
            base.EnterNpc(context);
        }

        public override void ExitNpc([NotNull] AgentConfigurationParser.NpcContext context)
        {
            base.ExitNpc(context);
        }

        public override void EnterCurrent([NotNull] AgentConfigurationParser.CurrentContext context)
        {
            base.EnterCurrent(context);
        }

        public override void ExitCurrent([NotNull] AgentConfigurationParser.CurrentContext context)
        {
            base.ExitCurrent(context);
        }

        public override void EnterOpponent([NotNull] AgentConfigurationParser.OpponentContext context)
        {
            base.EnterOpponent(context);
        }

        public override void ExitOpponent([NotNull] AgentConfigurationParser.OpponentContext context)
        {
            base.ExitOpponent(context);
        }

        public override void EnterTile([NotNull] AgentConfigurationParser.TileContext context)
        {
            base.EnterTile(context);
        }

        public override void ExitTile([NotNull] AgentConfigurationParser.TileContext context)
        {
            base.ExitTile(context);
        }

        public override void EnterInventory([NotNull] AgentConfigurationParser.InventoryContext context)
        {
            Inventory inventory = new Inventory(context.INVENTORY().GetText());
            currentContainer.Push(inventory);
        }

        public override void ExitInventory([NotNull] AgentConfigurationParser.InventoryContext context)
        {
            Node temp = currentContainer.Pop();
            currentContainer.Peek().AddChild(temp);
            base.ExitInventory(context);
        }

        public override void EnterPlayer([NotNull] AgentConfigurationParser.PlayerContext context)
        {
            Player player = new Player(context.PLAYER().GetText());
            currentContainer.Push(player);
        }

        public override void ExitPlayer([NotNull] AgentConfigurationParser.PlayerContext context)
        {
            Node temp = currentContainer.Pop();
            currentContainer.Peek().AddChild(temp);
        }

        public override void EnterComparable([NotNull] AgentConfigurationParser.ComparableContext context)
        {
            int x = 0;
            String value = context.children.FirstOrDefault().GetText();
            if (int.TryParse(value, out x)) {
                Int node = new Int(int.Parse(context.children.FirstOrDefault().GetText()));
                currentContainer.Peek().AddChild(node);
            }
        }
    }
}
