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
    public class Parser : PlayerCommandsBaseListener

    {
        private AST ast { get; }
        private Stack currentContainer;

        public Parser()
        {
            ast = new AST();
            currentContainer = new Stack();
        }

        public override void EnterMove(PlayerCommandsParser.MoveContext context)
        {
            currentContainer.Push(new Move());
        }

        public override void ExitMove(PlayerCommandsParser.MoveContext context)
        {
            currentContainer.Pop();
        }

        public override void EnterDirection(PlayerCommandsParser.DirectionContext context)
        {
            Move move = (Move) currentContainer.Peek();
            move.addChild(new Direction(context.GetText()));
        }

        public override void EnterStep(PlayerCommandsParser.StepContext context)
        {
            Move move = (Move) currentContainer.Peek();
            move.addChild(new Step(Convert.ToInt32(context.GetText())));
        }
    }
}