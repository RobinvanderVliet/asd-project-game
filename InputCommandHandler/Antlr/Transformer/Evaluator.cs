using InputCommandHandler.antlr.ast;
using InputCommandHandler.antlr.ast.actions;
using InputCommandHandler.exception;
using Player.Model;
using Player.Services;

namespace InputCommandHandler.antlr.transformer
{
    public class Evaluator : ITransform
    {
        private readonly IPlayerService _playerService;

        public Evaluator(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        public void Apply(AST ast)
        {
            TransformNode(ast.root);
        }

        private void TransformNode(ASTNode node)
        {
            var input = (Input)node;
            var nodeBody = input.body;
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
                }
        }

        private void TransformMove(Move move)
        {
            switch (move.steps.value)
            {
                case < 1:
                    throw new MoveException("Too few steps, the minimum is 1.");
                case > 10:
                    throw new MoveException("Too many steps, the maximum is 10.");
                default:
                    _playerService.HandleDirection(move.direction.value, move.steps.value);
                    break;
            }
        }

        private void TransformPickup()
        {
            _playerService.PickupItem();
        }

        private void TransformDrop(Drop drop)
        {
            _playerService.DropItem(drop.itemName.value);
        }

        private void TransformAttack(Attack attack)
        {
            _playerService.Attack(attack.direction.value);
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
            _playerService.Say(say.message.value);
        }

        private void TransformShout(Shout shout)
        {
            _playerService.Shout(shout.message.value);
        }
    }
}