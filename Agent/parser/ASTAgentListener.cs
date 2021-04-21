using System;
using System.Collections.Generic;
using System.Linq;
using Agent.antlr.grammar;
using Antlr4.Runtime.Misc;
using Agent.antlr.ast.implementation;
using Agent.antlr.ast.implementation.comparables;
using Agent.antlr.ast.implementation.comparables.subjects;
using Action = Agent.antlr.ast.implementation.Action;

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
            ActionReference reference = new ActionReference(context.STRING().GetText());
            currentContainer.Push(reference);
        }

        public override void EnterActionBlock([NotNull] AgentConfigurationParser.ActionBlockContext context)
        {
            Action action = new Action(context.GetText());
            currentContainer.Peek().AddChild(action);
        }

        public override void EnterActionSubject([NotNull] AgentConfigurationParser.ActionSubjectContext context)
        {
            ActionReference reference = new ActionReference(context.action().STRING().GetText());
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
            Item item = new Item(context.children.Where(c => c.ToString() != null).FirstOrDefault().GetText());
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
            Rule rule = new Rule(context.setting().GetText(), context.STRING().ElementAt(1).GetText());
            currentContainer.Push(rule);
        }

        public override void EnterSettingBlock([NotNull] AgentConfigurationParser.SettingBlockContext context)
        {
            Setting block = new Setting(context.setting().children.Where(c => c.GetText() != null).FirstOrDefault().GetText());
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
            Subject subject = (Subject)context.children.Where(c => c.GetText() != null).FirstOrDefault();
            Subject node = CreateSubject(subject);
            currentContainer.Push(node);
        }

        private Subject CreateSubject(Subject subject)
        {
            return subject.ToString() switch
            {
                "player" => new Player(subject.Name),
                "npc" => new NPC(subject.Name),
                "opponent" => new Opponent(subject.Name),
                "inventory" => new Inventory(subject.Name),
                "current" => new Current(subject.Name),
                "tile" => new Tile(subject.Name),
                _ => throw new Exception("subject is an invallid type:" + subject),
            };
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
            base.ExitRule(context);
        }

        public override void ExitSettingBlock([NotNull] AgentConfigurationParser.SettingBlockContext context)
        {
            Node temp = currentContainer.Pop();
            currentContainer.Peek().AddChild(temp);
        }

        public override void ExitActionBlock([NotNull] AgentConfigurationParser.ActionBlockContext context)
        {
            base.ExitActionBlock(context);
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
            Node temp = currentContainer.Pop();
            currentContainer.Peek().AddChild(temp);
        }

        public override void ExitActionSubject([NotNull] AgentConfigurationParser.ActionSubjectContext context)
        {
            base.ExitActionSubject(context);
        }

        public override void ExitComparable([NotNull] AgentConfigurationParser.ComparableContext context)
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

        /* ################################### */
        /* ##            SUBJECS            ## */
        /* ################################### */


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
            base.EnterInventory(context);
        }

        public override void ExitInventory([NotNull] AgentConfigurationParser.InventoryContext context)
        {
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

        public override void ExitString([NotNull] AgentConfigurationParser.StringContext context)
        {
            base.ExitString(context);
        }
    }
}
