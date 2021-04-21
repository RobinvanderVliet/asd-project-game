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
            ast.root.addChild((ASTNode) _currentContainer.Pop());
        }

        public override void EnterAttack(PlayerCommandsParser.AttackContext context)
        {
            _currentContainer.Push(new Attack());
        }

        public override void ExitAttack(PlayerCommandsParser.AttackContext context)
        {
            ast.root.addChild((ASTNode) _currentContainer.Pop());
        }

        public override void EnterDrop(PlayerCommandsParser.DropContext context)
        {
            _currentContainer.Push(new Drop());
        }

        public override void ExitDrop(PlayerCommandsParser.DropContext context)
        {
            ast.root.addChild((ASTNode) _currentContainer.Pop());
        }

        public override void EnterPickup(PlayerCommandsParser.PickupContext context)
        {
            _currentContainer.Push(new Pickup());
        }

        public override void ExitPickup(PlayerCommandsParser.PickupContext context)
        {
            ast.root.addChild((ASTNode) _currentContainer.Pop());
        }

        public override void EnterExit(PlayerCommandsParser.ExitContext context)
        {
            _currentContainer.Push(new Exit());
        }

        public override void ExitExit(PlayerCommandsParser.ExitContext context)
        {
            ast.root.addChild((ASTNode) _currentContainer.Pop());
        }

        public override void EnterSay(PlayerCommandsParser.SayContext context)
        {
            _currentContainer.Push(new Say());
        }

        public override void ExitSay(PlayerCommandsParser.SayContext context)
        {
            ast.root.addChild((ASTNode) _currentContainer.Pop());
        }

        public override void EnterShout(PlayerCommandsParser.ShoutContext context)
        {
            _currentContainer.Push(new Shout());
        }

        public override void ExitShout(PlayerCommandsParser.ShoutContext context)
        {
            ast.root.addChild((ASTNode) _currentContainer.Pop());
        }

        public override void EnterReplace(PlayerCommandsParser.ReplaceContext context)
        {
            _currentContainer.Push(new Replace());
        }

        public override void ExitReplace(PlayerCommandsParser.ReplaceContext context)
        {
            ast.root.addChild((ASTNode) _currentContainer.Pop());
        }

        public override void EnterPause(PlayerCommandsParser.PauseContext context)
        {
            _currentContainer.Push(new Pause());
        }

        public override void ExitPause(PlayerCommandsParser.PauseContext context)
        {
            ast.root.addChild((ASTNode) _currentContainer.Pop());
        }
        
        public override void EnterResume(PlayerCommandsParser.ResumeContext context)
        {
            _currentContainer.Push(new Resume());
        }

        public override void ExitResume(PlayerCommandsParser.ResumeContext context)
        {
            ast.root.addChild((ASTNode) _currentContainer.Pop());
        }

        public override void EnterDirection(PlayerCommandsParser.DirectionContext context)
        {
            var action = _currentContainer.Peek();

            if (action is Move)
            {
                Move move = (Move) action;
                move.addChild(new Direction(context.GetText()));
            }
            else if (action is Attack)
            {
                Attack attack = (Attack) action;
                attack.addChild(new Direction(context.GetText()));
            }
        }

        public override void EnterStep(PlayerCommandsParser.StepContext context)
        {
            Move move = (Move) _currentContainer.Peek();
            move.addChild(new Step(Convert.ToInt32(context.GetText())));
        }
        
        public override void EnterMessage(PlayerCommandsParser.MessageContext context)
        {
            var action = _currentContainer.Peek();

            if (action is Say)
            {
                Say say = (Say) action;
                say.addChild(new Message(context.GetText()));
            }
            else if (action is Shout)
            {
                Shout shout = (Shout) action;
                shout.addChild(new Message(context.GetText()));
            }
        }
    }
}