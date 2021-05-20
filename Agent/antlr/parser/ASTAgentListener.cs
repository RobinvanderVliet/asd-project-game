using Agent.antlr.ast;
using Agent.antlr.ast.comparables;
using Agent.antlr.ast.comparables.subjects;
using Agent.Antlr.grammar;
using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using Action = Agent.antlr.ast.Action;

namespace Agent.antlr.parser
{
    public class ASTAgentListener : AgentConfigurationBaseListener
    {
        private AST _ast;
        private Stack<Node> _currentContainer;
        private bool _itemStat;

        public ASTAgentListener()
        {
            this._ast = new AST();
            this._currentContainer = new Stack<Node>();
        }

        public AST GetAST() { return this._ast; }

        public override void EnterAction([NotNull] AgentConfigurationParser.ActionContext context)
        {
            if (this._currentContainer.Peek() is When || this._currentContainer.Peek() is Otherwise)
            {
                ActionReference reference = new ActionReference(context.STRING().GetText());
                this._currentContainer.Push(reference);
            }
        }

        public override void EnterActionBlock([NotNull] AgentConfigurationParser.ActionBlockContext context)
        {
            Action action = new Action(context.children.FirstOrDefault().GetText());
            this._currentContainer.Push(action);
        }

        public override void EnterActionSubject([NotNull] AgentConfigurationParser.ActionSubjectContext context)
        {
            ActionReference reference = new ActionReference(context.action().STRING().GetText());
            Console.WriteLine("");
            this._currentContainer.Push(reference);
        }

        public override void EnterComparison([NotNull] AgentConfigurationParser.ComparisonContext context)
        {
            Comparison comparison = new Comparison(context.children.Where(c => c.GetText() != null).FirstOrDefault().GetText());
            this._currentContainer.Push(comparison);
        }

        public override void EnterCondition([NotNull] AgentConfigurationParser.ConditionContext context)
        {
            Condition condition = new Condition();
            this._currentContainer.Push(condition);
        }

        public override void EnterConfiguration([NotNull] AgentConfigurationParser.ConfigurationContext context)
        {
            Configuration configuration = new Configuration();
            this._currentContainer.Push(configuration);
        }

        public override void EnterItem([NotNull] AgentConfigurationParser.ItemContext context)
        {
            String temp = context.children.FirstOrDefault().GetText();
            Item item = new Item(temp);
            this._currentContainer.Push(item);
        }

        public override void EnterItemStat([NotNull] AgentConfigurationParser.ItemStatContext context)
        {
            this._itemStat = true;
        }

        public override void EnterOtherwiseClause([NotNull] AgentConfigurationParser.OtherwiseClauseContext context)
        {
            Otherwise otherwise = new Otherwise();
            this._currentContainer.Push(otherwise);
        }

        public override void EnterRule([NotNull] AgentConfigurationParser.RuleContext context)
        {
            Rule rule = new Rule(context.children.ElementAt(0).GetText(), context.children.ElementAt(2).GetText());
            this._currentContainer.Push(rule);
        }

        public override void EnterSettingBlock([NotNull] AgentConfigurationParser.SettingBlockContext context)
        {
            Setting block = new Setting(context.setting().children.Where(c => c.GetText() != null).FirstOrDefault().GetText());
            this._currentContainer.Push(block);
        }

        public override void EnterStat([NotNull] AgentConfigurationParser.StatContext context)
        {
            Stat stat = new Stat(context.children.Where(c => c.GetText() != null).FirstOrDefault().GetText());
            this._currentContainer.Push(stat);
        }

        public override void EnterSubject([NotNull] AgentConfigurationParser.SubjectContext context)
        {
            Subject subject = (Subject)context.children.Where(c => c.GetText() != null).FirstOrDefault();
            this._currentContainer.Push(subject);
        }


        public override void EnterSubjectStat([NotNull] AgentConfigurationParser.SubjectStatContext context)
        {
            base.EnterSubjectStat(context);
        }

        public override void EnterWhenClause([NotNull] AgentConfigurationParser.WhenClauseContext context)
        {
            When clause = new When();
            this._currentContainer.Push(clause);
        }

        public override void ExitItem([NotNull] AgentConfigurationParser.ItemContext context)
        {
            if (!this._itemStat)
            {
                Node temp = this._currentContainer.Pop();
                this._currentContainer.Peek().AddChild(temp);
            }
        }

        public override void ExitOtherwiseClause([NotNull] AgentConfigurationParser.OtherwiseClauseContext context)
        {
            Node temp = this._currentContainer.Pop();
            this._currentContainer.Peek().AddChild(temp);
        }

        public override void ExitStat([NotNull] AgentConfigurationParser.StatContext context)
        {
            if (!this._itemStat) 
            { 
                Node temp = this._currentContainer.Pop();
                this._currentContainer.Peek().AddChild(temp);
            }
        }

        public override void ExitWhenClause([NotNull] AgentConfigurationParser.WhenClauseContext context)
        {
            Node temp = this._currentContainer.Pop();
            this._currentContainer.Peek().AddChild(temp);
        }

        public override void ExitConfiguration([NotNull] AgentConfigurationParser.ConfigurationContext context)
        {
            Node temp = this._currentContainer.Pop();
            this._ast.SetRoot((Configuration)temp);
        }

        public override void ExitRule([NotNull] AgentConfigurationParser.RuleContext context)
        {
            Node temp = this._currentContainer.Pop();
            this._currentContainer.Peek().AddChild(temp);
        }

        public override void ExitSettingBlock([NotNull] AgentConfigurationParser.SettingBlockContext context)
        {
            Node temp = this._currentContainer.Pop();
            this._currentContainer.Peek().AddChild(temp);
        }

        public override void ExitActionBlock([NotNull] AgentConfigurationParser.ActionBlockContext context)
        {
            Node temp = this._currentContainer.Pop();
            this._currentContainer.Peek().AddChild(temp);
        }

        public override void ExitCondition([NotNull] AgentConfigurationParser.ConditionContext context)
        {
            Node temp = this._currentContainer.Pop();
            this._currentContainer.Peek().AddChild(temp);
        }

        public override void ExitComparison([NotNull] AgentConfigurationParser.ComparisonContext context)
        {
            Node temp = this._currentContainer.Pop();
            this._currentContainer.Peek().AddChild(temp);
        }

        public override void ExitAction([NotNull] AgentConfigurationParser.ActionContext context)
        {
            if (this._currentContainer.Peek() is ActionReference && ((ActionReference)this._currentContainer.Peek()).Name != "use") 
            {
                Node temp = this._currentContainer.Pop();
                this._currentContainer.Peek().AddChild(temp);
            }
        }

        public override void ExitActionSubject([NotNull] AgentConfigurationParser.ActionSubjectContext context)
        {
            Node temp = this._currentContainer.Pop();
            this._currentContainer.Peek().AddChild(temp);
        }

        public override void ExitItemStat([NotNull] AgentConfigurationParser.ItemStatContext context)
        {
            //merge item + state
            Node temp = this._currentContainer.Pop();
            this._currentContainer.Peek().AddChild(temp);

            //push itemStat in When
            temp = this._currentContainer.Pop();
            this._currentContainer.Peek().AddChild(temp);

            this._itemStat = false;
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
            NPC npc = new NPC(context.NPC().GetText());
            this._currentContainer.Push(npc);
        }

        public override void ExitNpc([NotNull] AgentConfigurationParser.NpcContext context)
        {
            Node temp = this._currentContainer.Pop();
            this._currentContainer.Peek().AddChild(temp);
        }

        public override void EnterCurrent([NotNull] AgentConfigurationParser.CurrentContext context)
        {
            Current current = new Current(context.CURRENT().GetText());
            this._currentContainer.Push(current);
        }

        public override void ExitCurrent([NotNull] AgentConfigurationParser.CurrentContext context)
        {
            Node temp = this._currentContainer.Pop();
            this._currentContainer.Peek().AddChild(temp);
        }

        public override void EnterOpponent([NotNull] AgentConfigurationParser.OpponentContext context)
        {
            Opponent opponent = new Opponent(context.OPPONENT().GetText());
            this._currentContainer.Push(opponent);
        }

        public override void ExitOpponent([NotNull] AgentConfigurationParser.OpponentContext context)
        {
            Node temp = this._currentContainer.Pop();
            this._currentContainer.Peek().AddChild(temp);
        }

        public override void EnterTile([NotNull] AgentConfigurationParser.TileContext context)
        {
            Tile tile = new Tile(context.STRING().GetText());
            this._currentContainer.Push(tile);
        }

        public override void ExitTile([NotNull] AgentConfigurationParser.TileContext context)
        {
            Node temp = this._currentContainer.Pop();
            this._currentContainer.Peek().AddChild(temp);
        }

        public override void EnterInventory([NotNull] AgentConfigurationParser.InventoryContext context)
        {
            Inventory inventory = new Inventory(context.INVENTORY().GetText());
            this._currentContainer.Push(inventory);
        }

        public override void ExitInventory([NotNull] AgentConfigurationParser.InventoryContext context)
        {
            Node temp = this._currentContainer.Pop();
            this._currentContainer.Peek().AddChild(temp);
        }

        public override void EnterPlayer([NotNull] AgentConfigurationParser.PlayerContext context)
        {
            Player player = new Player(context.PLAYER().GetText());
            this._currentContainer.Push(player);
        }

        public override void ExitPlayer([NotNull] AgentConfigurationParser.PlayerContext context)
        {
            Node temp = this._currentContainer.Pop();
            this._currentContainer.Peek().AddChild(temp);
        }

        public override void EnterComparable([NotNull] AgentConfigurationParser.ComparableContext context)
        {
            int x = 0;
            String value = context.children.FirstOrDefault().GetText();
            if (int.TryParse(value, out x)) {
                Int node = new Int(int.Parse(context.children.FirstOrDefault().GetText()));
                this._currentContainer.Peek().AddChild(node);
            }
        }
    }
}
