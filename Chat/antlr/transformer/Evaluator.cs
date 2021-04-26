using Chat.antlr.ast;
using Chat.antlr.ast.actions;
using Chat.exception;
using Player.Model;

namespace Chat.antlr.transformer
{
    public class Evaluator : ITransform
    {
        private readonly IPlayerModel _playerModel;

        public Evaluator(IPlayerModel playerModel)
        {
            _playerModel = playerModel;
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
                        TransformExit((Exit)nodeBody[i]);
                        break;
                    case Move:
                        TransformMove((Move)nodeBody[i]);
                        break;
                    case Pause:
                        TransformPause((Pause)nodeBody[i]);
                        break;
                    case Pickup:
                        TransformPickup((Pickup)nodeBody[i]);
                        break;
                    case Replace:
                        TransformReplace((Replace)nodeBody[i]);
                        break;
                    case Resume:
                        TransformResume((Resume)nodeBody[i]);
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
                    _playerModel.HandleDirection(move.direction.value, move.steps.value);
                    break;
            }
        }

        private void TransformPickup(Pickup pickup)
        {
            _playerModel.PickupItem();
        }

        private void TransformDrop(Drop drop)
        {
            // _playerModel.DropItem(item);
        }

        private void TransformAttack(Attack attack)
        {
            _playerModel.HandleAttackAction();
        }

        private void TransformExit(Exit exit)
        {
            _playerModel.HandleExitAction();
        }

        private void TransformPause(Pause pause)
        {
            _playerModel.HandlePauseAction();
        }

        private void TransformReplace(Replace replace)
        {
            _playerModel.HandleReplaceAction();
        }

        private void TransformResume(Resume resume)
        {
            _playerModel.HandleResumeAction();
        }

        private void TransformSay(Say say)
        {
            _playerModel.HandleSayAction();
        }

        private void TransformShout(Shout shout)
        {
            _playerModel.HandleShoutAction();
        }
    }
}