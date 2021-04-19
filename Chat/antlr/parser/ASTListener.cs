/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD-project-game.
 
    This file is created by team: 2
     
    Goal of this file: Parser class for add actions to stack.
     
*/
using System;
using System.Collections;
using Chat.antlr.ast;
using Chat.antlr.ast.actions;
using Chat.antlr.grammar;

namespace Chat.antlr.parser
{
    public class ASTListener : PlayerCommandsBaseListener

    {
        private AST ast;
        private Stack _currentContainer;

        public ASTListener()
        {
            ast = new AST();
            _currentContainer = new Stack();
        }

        public AST getAST()
        {
            return ast;
        }

        public override void EnterMove(PlayerCommandsParser.MoveContext context)
        {
            _currentContainer.Push(new Move());
        }

        public override void ExitMove(PlayerCommandsParser.MoveContext context)
        {
            
           ast.root.addChild((ASTNode)_currentContainer.Pop());
        }

        public override void EnterDirection(PlayerCommandsParser.DirectionContext context)
        {
            Move move = (Move) _currentContainer.Peek();
            move.addChild(new Direction(context.GetText()));
        }

        public override void EnterStep(PlayerCommandsParser.StepContext context)
        {
            Move move = (Move) _currentContainer.Peek();
            move.addChild(new Step(Convert.ToInt32(context.GetText())));
        }
    }
}