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
                        transformAttack((Attack)nodeBody[i]);
                        break;
                    case Drop:
                        transformDrop((Drop)nodeBody[i]);
                        break;
                    case Exit:
                        transformExit((Exit)nodeBody[i]);
                        break;
                    case Move:
                        transformMove((Move)nodeBody[i]);
                        break;
                    case Pause:
                        transformPause((Pause)nodeBody[i]);
                        break;
                    case Pickup:
                        transformPickup((Pickup)nodeBody[i]);
                        break;
                    case Replace:
                        transformReplace((Replace)nodeBody[i]);
                        break;
                    case Resume:
                        transformResume((Resume)nodeBody[i]);
                        break;
                    case Say:
                        transformSay((Say)nodeBody[i]);
                        break;
                    case Shout:
                        transformShout((Shout)nodeBody[i]);
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
            _playerModel.HandleItemAction("pickup");
        }

        private void TransformDrop(Drop drop)
        {
            _playerModel.HandleItemAction("drop");
        }

        private void TransformAttack(Attack attack)
        {
            _playerModel.HandleAttackAction("attack");
        }

        private void TransformExit(Exit exit)
        {
            _playerModel.HandleExitAction("exit");
        }

        private void TransformPause(Pause pause)
        {
            _playerModel.HandlePauseAction("pause");
        }

        private void TransformReplace(Replace replace)
        {
            _playerModel.HandleReplaceAction("replace");
        }

        private void TransformResume(Resume resume)
        {
            _playerModel.HandleResumeAction("resume");
        }

        private void TransformSay(Say say)
        {
            _playerModel.HandleSayAction("say");
        }

        private void TransformShout(Shout shout)
        {
            _playerModel.HandleShoutAction("shout");
        }
    }
}