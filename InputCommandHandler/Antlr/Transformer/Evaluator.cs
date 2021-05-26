using System;
using InputCommandHandler.Antlr.Ast;
using InputCommandHandler.Antlr.Ast.Actions;
using InputCommandHandler.Exceptions;
using Player.Services;
using Session;

namespace InputCommandHandler.Antlr.Transformer
{
    public class Evaluator : ITransform
    {
        private readonly IPlayerService _playerService;
        private readonly ISessionService _sessionService;
        private const int MINIMUM_STEPS = 1;
        private const int MAXIMUM_STEPS = 10;
        private String _commando;

        public Evaluator(IPlayerService playerService, ISessionService sessionService)
        {
            _playerService = playerService;
            _sessionService = sessionService;
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
                    case Search:
                        TransformSearch();
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
                    _playerService.HandleDirection(move.Direction.DirectionValue, move.Steps.StepValue);
                    break;
            }
        }

        private void TransformPickup()
        {
            _playerService.PickupItem();
        }

        private void TransformDrop(Drop drop)
        {
            _playerService.DropItem(drop.ItemName.MessageValue);
        }

        private void TransformAttack(Attack attack)
        {
            _playerService.Attack(attack.Direction.DirectionValue);
        }

        private void TransformExit()
        {
            _playerService.ExitCurrentGame();
        }

        private void TransformPause()
        {
            _playerService.Pause();
        }

        private void TransformReplace()
        {
            _playerService.ReplaceByAgent();
        }

        private void TransformResume()
        {
            _playerService.Resume();
        }

        private void TransformSay(Say say)
        {
            _playerService.Say(say.Message.MessageValue);
        }

        private void TransformShout(Shout shout)
        {
            _playerService.Shout(shout.Message.MessageValue);
        }

        private void TransformCreateSession(CreateSession createSession)
        {
            _sessionService.CreateSession(createSession.Message.MessageValue);
        }

        private void TransformJoinSession(JoinSession joinSession)
        {
            _sessionService.JoinSession(joinSession.Message.MessageValue);
        }

        private void TransformRequestSessions(RequestSessions requestSessions)
        {
            _sessionService.RequestSessions();
        }

        private void TransformStartSession(StartSession startSession)
        {
            _sessionService.StartSession();
        }

        private void TransformSearch()
        {
            _playerService.Search();
        }

    }
}