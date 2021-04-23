using System;
using System.Collections;
using Chat.antlr.ast;
using Chat.antlr.ast.actions;
using Chat.antlr.grammar;

namespace Chat.antlr.parser
{
    public class ASTListener : PlayerCommandsBaseListener

    {
        private readonly Stack _currentContainer;

        public ASTListener()
        {
            ast = new AST();
            _currentContainer = new Stack();
        }

        public AST ast { get; }

        public override void EnterMove(PlayerCommandsParser.MoveContext context)
        {
            _currentContainer.Push(new Move());
        }

        public override void ExitMove(PlayerCommandsParser.MoveContext context)
        {
            ast.root.AddChild((ASTNode) _currentContainer.Pop());
        }

        public override void EnterDirection(PlayerCommandsParser.DirectionContext context)
        {
            var move = (Move) _currentContainer.Peek();
            move.AddChild(new Direction(context.GetText()));
        }

        public override void EnterStep(PlayerCommandsParser.StepContext context)
        {
            var move = (Move) _currentContainer.Peek();
            move.AddChild(new Step(Convert.ToInt32(context.GetText())));
        }
    }
}