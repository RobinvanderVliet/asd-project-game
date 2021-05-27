using System;
using System.Collections;
using InputCommandHandler.Antlr.Ast;
using InputCommandHandler.Antlr.Ast.Actions;
using InputCommandHandler.Antlr.Grammar;

namespace InputCommandHandler.Antlr.Parser
{
    public class ASTListener : PlayerCommandsBaseListener
    {
        private AST _ast;
        private Stack _currentContainer;

        public ASTListener()
        {
            _ast = new AST();
            _currentContainer = new Stack();
        }

        public AST getAST()
        {
            return _ast;
        }

        public override void EnterMove(PlayerCommandsParser.MoveContext context)
        {
            _currentContainer.Push(new Move());
        }

        public override void ExitMove(PlayerCommandsParser.MoveContext context)
        {
            _ast.Root.AddChild((ASTNode) _currentContainer.Pop());
        }

        public override void EnterAttack(PlayerCommandsParser.AttackContext context)
        {
            _currentContainer.Push(new Attack());
        }

        public override void ExitAttack(PlayerCommandsParser.AttackContext context)
        {
            _ast.Root.AddChild((ASTNode) _currentContainer.Pop());
        }

        public override void EnterDrop(PlayerCommandsParser.DropContext context)
        {
            _currentContainer.Push(new Drop());
        }

        public override void ExitDrop(PlayerCommandsParser.DropContext context)
        {
            _ast.Root.AddChild((ASTNode) _currentContainer.Pop());
        }

        public override void EnterPickup(PlayerCommandsParser.PickupContext context)
        {
            _currentContainer.Push(new Pickup());
        }

        public override void ExitPickup(PlayerCommandsParser.PickupContext context)
        {
            _ast.Root.AddChild((ASTNode) _currentContainer.Pop());
        }

        public override void EnterExit(PlayerCommandsParser.ExitContext context)
        {
            _currentContainer.Push(new Exit());
        }

        public override void ExitExit(PlayerCommandsParser.ExitContext context)
        {
            _ast.Root.AddChild((ASTNode) _currentContainer.Pop());
        }

        public override void EnterSay(PlayerCommandsParser.SayContext context)
        {
            _currentContainer.Push(new Say());
        }

        public override void ExitSay(PlayerCommandsParser.SayContext context)
        {
            _ast.Root.AddChild((ASTNode) _currentContainer.Pop());
        }

        public override void EnterCreateSession(PlayerCommandsParser.CreateSessionContext context)
        {
            _currentContainer.Push(new CreateSession());
        }

        public override void ExitCreateSession(PlayerCommandsParser.CreateSessionContext context)
        {
            _ast.Root.AddChild((ASTNode) _currentContainer.Pop());
        }

        public override void EnterJoinSession(PlayerCommandsParser.JoinSessionContext context)
        {
            _currentContainer.Push(new JoinSession());
        }

        public override void ExitJoinSession(PlayerCommandsParser.JoinSessionContext context)
        {
            _ast.Root.AddChild((ASTNode) _currentContainer.Pop());
        }

        public override void EnterRequestSessions(PlayerCommandsParser.RequestSessionsContext context)
        {
            _currentContainer.Push(new RequestSessions());
        }

        public override void ExitRequestSessions(PlayerCommandsParser.RequestSessionsContext context)
        {
            _ast.Root.AddChild((ASTNode) _currentContainer.Pop());
        }

        public override void EnterStartSession(PlayerCommandsParser.StartSessionContext context)
        {
            _currentContainer.Push(new StartSession());
        }

        public override void ExitStartSession(PlayerCommandsParser.StartSessionContext context)
        {
            _ast.Root.AddChild((ASTNode) _currentContainer.Pop());
        }

        public override void EnterStartGame(PlayerCommandsParser.StartGameContext context)
        {
            _currentContainer.Push(new StartSession());
        }

        public override void ExitStartGame(PlayerCommandsParser.StartGameContext context)
        {
            _ast.Root.AddChild((ASTNode)_currentContainer.Pop());
        }

        public override void EnterLoadGame(PlayerCommandsParser.LoadGameContext context)
        {
            _currentContainer.Push(new StartSession());
        }

        public override void ExitLoadGame(PlayerCommandsParser.LoadGameContext context)
        {
            _ast.Root.AddChild((ASTNode)_currentContainer.Pop());
        }

        public override void EnterShout(PlayerCommandsParser.ShoutContext context)
        {
            _currentContainer.Push(new Shout());
        }

        public override void ExitShout(PlayerCommandsParser.ShoutContext context)
        {
            _ast.Root.AddChild((ASTNode) _currentContainer.Pop());
        }

        public override void EnterReplace(PlayerCommandsParser.ReplaceContext context)
        {
            _currentContainer.Push(new Replace());
        }

        public override void ExitReplace(PlayerCommandsParser.ReplaceContext context)
        {
            _ast.Root.AddChild((ASTNode) _currentContainer.Pop());
        }

        public override void EnterPause(PlayerCommandsParser.PauseContext context)
        {
            _currentContainer.Push(new Pause());
        }

        public override void ExitPause(PlayerCommandsParser.PauseContext context)
        {
            _ast.Root.AddChild((ASTNode) _currentContainer.Pop());
        }

        public override void EnterResume(PlayerCommandsParser.ResumeContext context)
        {
            _currentContainer.Push(new Resume());
        }

        public override void ExitResume(PlayerCommandsParser.ResumeContext context)
        {
            _ast.Root.AddChild((ASTNode) _currentContainer.Pop());
        }

        public override void EnterDirection(PlayerCommandsParser.DirectionContext context)
        {
            var action = _currentContainer.Peek();

            if (action is Move)
            {
                Move move = (Move) action;
                move.AddChild(new Direction(context.GetText()));
            }
            else if (action is Attack)
            {
                Attack attack = (Attack) action;
                attack.AddChild(new Direction(context.GetText()));
            }
        }

        public override void EnterStep(PlayerCommandsParser.StepContext context)
        {
            Move move = (Move) _currentContainer.Peek();
            move.AddChild(new Step(Convert.ToInt32(context.GetText())));
        }

        public override void EnterMessage(PlayerCommandsParser.MessageContext context)
        {
            var action = _currentContainer.Peek();

            if (action is Say)
            {
                Say say = (Say) action;
                say.AddChild(new Message(context.GetText()));
            }
            else if (action is Shout)
            {
                Shout shout = (Shout) action;
                shout.AddChild(new Message(context.GetText()));
            }
            else if (action is CreateSession createSession)
            {
                createSession.AddChild(new Message(context.GetText()));
            }
            else if (action is JoinSession joinSession)
            {
                joinSession.AddChild(new Message(context.GetText()));
            }
            else if (action is StartSession startSession)
            {
                startSession.AddChild(new Message(context.GetText()));
            }
        }

        public override void EnterRequestSavedGames(PlayerCommandsParser.RequestSavedGamesContext context)
        {
            _currentContainer.Push(new RequestSavedGames());
        }

        public override void ExitRequestSavedGames(PlayerCommandsParser.RequestSavedGamesContext context)
        {
            _ast.Root.AddChild((ASTNode) _currentContainer.Pop());
        }
    }
}