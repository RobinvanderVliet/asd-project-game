using System;
using ActionHandling;
using Chat;
using InputHandling.Antlr.Ast;
using InputHandling.Antlr.Ast.Actions;
using InputHandling.Exceptions;
using Session;

namespace InputHandling.Antlr.Transformer
{
    public class Evaluator : IEvaluator
    {
        private ISessionHandler _sessionHandler;
        private IMoveHandler _moveHandler;
        private IGameSessionHandler _gameSessionHandler;
        private IChatHandler _chatHandler;
        
        private const int MINIMUM_STEPS = 1;
        private const int MAXIMUM_STEPS = 10;
        private String _commando;

        public Evaluator(ISessionHandler sessionHandler, IMoveHandler moveHandler, IGameSessionHandler gameSessionHandler, IChatHandler chatHandler)
        {
            _sessionHandler = sessionHandler;
            _moveHandler = moveHandler;
            _gameSessionHandler = gameSessionHandler;
            _chatHandler = chatHandler;
        }
        public void Apply(AST ast)
        {
            TransformNode(ast.Root);
        }

        private void TransformNode(ASTNode node)
        {
            var input = (Input)node;
            var nodeBody = input.Body;
            for (int i = 0; i < nodeBody.Count; i++)
                switch (nodeBody[i])
                {
                    case Attack:
                        TransformAttack((Attack)nodeBody[i]);
                        break;
                    case Drop:
                        TransformDrop((Drop)nodeBody[i]);
                        break;
                    case Exit:
                        TransformExit();
                        break;
                    case Move:
                        TransformMove((Move)nodeBody[i]);
                        break;
                    case Pause:
                        TransformPause();
                        break;
                    case Pickup:
                        TransformPickup();
                        break;
                    case Replace:
                        TransformReplace();
                        break;
                    case Resume:
                        TransformResume();
                        break;
                    case Say:
                        TransformSay((Say)nodeBody[i]);
                        break;
                    case Shout:
                        TransformShout((Shout)nodeBody[i]);
                        break;
                    case CreateSession:
                        TransformCreateSession((CreateSession)nodeBody[i]);
                        break;
                    case JoinSession:
                        TransformJoinSession((JoinSession)nodeBody[i]);
                        break;
                    case RequestSessions:
                        TransformRequestSessions((RequestSessions)nodeBody[i]);
                        break;
                    case StartSession:
                        TransformStartSession((StartSession)nodeBody[i]);
                        break;
                    case Inspect:
                        TransformInspect((Inspect)nodeBody[i]);
                        break;
                }
        }

        private void TransformMove(Move move)
        {
            switch (move.Steps.StepValue)
            {
                case < MINIMUM_STEPS:
                    throw new MoveException($"Too few steps, the minimum is {MINIMUM_STEPS}.");
                case > MAXIMUM_STEPS:
                    throw new MoveException($"Too many steps, the maximum is {MAXIMUM_STEPS}.");
                default:
                    _moveHandler.SendMove(move.Direction.DirectionValue, move.Steps.StepValue);
                    break;
            }
        }

        private void TransformPickup()
        {
            // TODO: Call InventoryHandler method
        }

        private void TransformDrop(Drop drop)
        {
            // TODO: Call InventoryHandler method with (drop.ItemName.MessageValue)
        }

        private void TransformAttack(Attack attack)
        {
            // TODO: Call AttackHandler method with (attack.Direction.DirectionValue)
        }

        private void TransformExit()
        {
            // TODO: Implement exitHandler
        }

        private void TransformPause()
        {
            // TODO: Remove? Must be replaced by save
        }

        private void TransformReplace()
        {
            // TODO: Replace by agent, maybe an AgentHandler?
        }

        private void TransformResume()
        {
            // TODO: Remove? Must be replaced by load functionality
        }

        private void TransformSay(Say say)
        {
            _chatHandler.SendSay(say.Message.MessageValue);
        }

        private void TransformShout(Shout shout)
        {
            _chatHandler.SendShout(shout.Message.MessageValue);
        }

        private void TransformCreateSession(CreateSession createSession)
        {
            _sessionHandler.CreateSession(createSession.Message.MessageValue);
        }

        private void TransformJoinSession(JoinSession joinSession)
        {
            _sessionHandler.JoinSession(joinSession.Message.MessageValue);
        }

        private void TransformRequestSessions(RequestSessions requestSessions)
        {
            _sessionHandler.RequestSessions();
        }

        private void TransformStartSession(StartSession startSession)
        {
            _gameSessionHandler.SendGameSession();
        }
        
        private void TransformInspect(Inspect inspect)
        {
            string slot = inspect.InventorySlot.InventorySlotValue;
            if (slot == "armor" | slot == "weapon" | slot == "helmet" | slot == "slot 1" | slot == "slot 2" | slot == "slot 3")
            {
                _playerService.InspectItem(inspect.InventorySlot.InventorySlotValue);
            }
            else
            {
                throw new SlotException($"The slot you provided {slot} is not valid.");
            }
        }
    }
}