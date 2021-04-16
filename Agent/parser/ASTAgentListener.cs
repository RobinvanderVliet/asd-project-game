using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agent.antlr.grammar;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;

namespace Agent.parser
{
    class ASTAgentListener : AgentConfigurationBaseListener
    {

        private AST ast;
        private Stack<ASTNode> currentContainer;

        public ASTAgentListener()
        {
            ast = new AST();
            currentContainer = new Stack<ASTNode>;
        }

        public AST getAST(){ return ast; }



        public override void EnterAction([NotNull] AgentConfigurationParser.ActionContext context)
        {
            base.EnterAction(context);
        }

        public override void EnterActionBlock([NotNull] AgentConfigurationParser.ActionBlockContext context)
        {
            base.EnterActionBlock(context);
        }

        public override void EnterActionSubject([NotNull] AgentConfigurationParser.ActionSubjectContext context)
        {
            base.EnterActionSubject(context);
        }

        public override void EnterComparable([NotNull] AgentConfigurationParser.ComparableContext context)
        {
            base.EnterComparable(context);
        }

        public override void EnterComparison([NotNull] AgentConfigurationParser.ComparisonContext context)
        {
            Configuration configuration = new Configuration();
            currentContainer.Push(configuration);
            ast.setRoot(configuration);
        }

        public override void EnterCondition([NotNull] AgentConfigurationParser.ConditionContext context)
        {
            base.EnterCondition(context);
        }

        public override void EnterConfiguration([NotNull] AgentConfigurationParser.ConfigurationContext context)
        {
            base.EnterConfiguration(context);
        }

        public override void EnterCurrent([NotNull] AgentConfigurationParser.CurrentContext context)
        {
            base.EnterCurrent(context);
        }

        public override void EnterEveryRule([NotNull] ParserRuleContext context)
        {
            base.EnterEveryRule(context);
        }

        public override void EnterInventory([NotNull] AgentConfigurationParser.InventoryContext context)
        {
            base.EnterInventory(context);
        }

        public override void EnterItem([NotNull] AgentConfigurationParser.ItemContext context)
        {
            base.EnterItem(context);
        }

        public override void EnterItemStat([NotNull] AgentConfigurationParser.ItemStatContext context)
        {
            base.EnterItemStat(context);
        }

        public override void EnterNpc([NotNull] AgentConfigurationParser.NpcContext context)
        {
            base.EnterNpc(context);
        }

        public override void EnterOpponent([NotNull] AgentConfigurationParser.OpponentContext context)
        {
            base.EnterOpponent(context);
        }

        public override void EnterOtherwiseClause([NotNull] AgentConfigurationParser.OtherwiseClauseContext context)
        {
            base.EnterOtherwiseClause(context);
        }

        public override void EnterPlayer([NotNull] AgentConfigurationParser.PlayerContext context)
        {
            base.EnterPlayer(context);
        }

        public override void EnterRule([NotNull] AgentConfigurationParser.RuleContext context)
        {
            Rule rule = new Rule();
            rule.setValue = context.STRING;
            currentContainer.Push(rule);
        }

        public override void EnterSetting([NotNull] AgentConfigurationParser.SettingContext context)
        {
            ASTNode node = currentContainer.Pop();
            node.addChild(new Setting(context.GetChild()));
            currentContainer.Push(node);
        }

        public override void EnterSettingBlock([NotNull] AgentConfigurationParser.SettingBlockContext context)
        {
            base.EnterSettingBlock(context);
        }

        public override void EnterStat([NotNull] AgentConfigurationParser.StatContext context)
        {
            base.EnterStat(context);
        }

        public override void EnterString([NotNull] AgentConfigurationParser.StringContext context)
        {
            base.EnterString(context);
        }

        public override void EnterSubject([NotNull] AgentConfigurationParser.SubjectContext context)
        {
            base.EnterSubject(context);
        }

        public override void EnterSubjectStat([NotNull] AgentConfigurationParser.SubjectStatContext context)
        {
            base.EnterSubjectStat(context);
        }

        public override void EnterTile([NotNull] AgentConfigurationParser.TileContext context)
        {
            base.EnterTile(context);
        }

        public override void EnterWhenClause([NotNull] AgentConfigurationParser.WhenClauseContext context)
        {
            base.EnterWhenClause(context);
        }

        public override void ExitAction([NotNull] AgentConfigurationParser.ActionContext context)
        {
            base.ExitAction(context);
        }

        public override void ExitActionBlock([NotNull] AgentConfigurationParser.ActionBlockContext context)
        {
            base.ExitActionBlock(context);
        }

        public override void ExitActionSubject([NotNull] AgentConfigurationParser.ActionSubjectContext context)
        {
            base.ExitActionSubject(context);
        }

        public override void ExitComparable([NotNull] AgentConfigurationParser.ComparableContext context)
        {
            base.ExitComparable(context);
        }

        public override void ExitComparison([NotNull] AgentConfigurationParser.ComparisonContext context)
        {
            base.ExitComparison(context);
        }

        public override void ExitCondition([NotNull] AgentConfigurationParser.ConditionContext context)
        {
            base.ExitCondition(context);
        }

        public override void ExitConfiguration([NotNull] AgentConfigurationParser.ConfigurationContext context)
        {
            base.ExitConfiguration(context);
        }

        public override void ExitCurrent([NotNull] AgentConfigurationParser.CurrentContext context)
        {
            base.ExitCurrent(context);
        }

        public override void ExitEveryRule([NotNull] ParserRuleContext context)
        {
            base.ExitEveryRule(context);
        }

        public override void ExitInventory([NotNull] AgentConfigurationParser.InventoryContext context)
        {
            base.ExitInventory(context);
        }

        public override void ExitItem([NotNull] AgentConfigurationParser.ItemContext context)
        {
            base.ExitItem(context);
        }

        public override void ExitItemStat([NotNull] AgentConfigurationParser.ItemStatContext context)
        {
            base.ExitItemStat(context);
        }

        public override void ExitNpc([NotNull] AgentConfigurationParser.NpcContext context)
        {
            base.ExitNpc(context);
        }

        public override void ExitOpponent([NotNull] AgentConfigurationParser.OpponentContext context)
        {
            base.ExitOpponent(context);
        }

        public override void ExitOtherwiseClause([NotNull] AgentConfigurationParser.OtherwiseClauseContext context)
        {
            base.ExitOtherwiseClause(context);
        }

        public override void ExitPlayer([NotNull] AgentConfigurationParser.PlayerContext context)
        {
            base.ExitPlayer(context);
        }

        public override void ExitRule([NotNull] AgentConfigurationParser.RuleContext context)
        {
            base.ExitRule(context);
        }

        public override void ExitSetting([NotNull] AgentConfigurationParser.SettingContext context)
        {
            base.ExitSetting(context);
        }

        public override void ExitSettingBlock([NotNull] AgentConfigurationParser.SettingBlockContext context)
        {
            base.ExitSettingBlock(context);
        }

        public override void ExitStat([NotNull] AgentConfigurationParser.StatContext context)
        {
            base.ExitStat(context);
        }

        public override void ExitString([NotNull] AgentConfigurationParser.StringContext context)
        {
            base.ExitString(context);
        }

        public override void ExitSubject([NotNull] AgentConfigurationParser.SubjectContext context)
        {
            base.ExitSubject(context);
        }

        public override void ExitSubjectStat([NotNull] AgentConfigurationParser.SubjectStatContext context)
        {
            base.ExitSubjectStat(context);
        }

        public override void ExitTile([NotNull] AgentConfigurationParser.TileContext context)
        {
            base.ExitTile(context);
        }

        public override void ExitWhenClause([NotNull] AgentConfigurationParser.WhenClauseContext context)
        {
            base.ExitWhenClause(context);
        }
    }
}
