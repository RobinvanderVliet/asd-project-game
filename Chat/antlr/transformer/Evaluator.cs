using System;
using System.Security.Cryptography;
using Chat.antlr.ast;
using Chat.antlr.ast.actions;
using Player;

namespace Chat.antlr.transformer
{
    public class Evaluator : ITransform
    {
        private readonly IPlayerModel _playerModel;

        public Evaluator(IPlayerModel playerModel)
        {
            _playerModel = playerModel;
        }

        public void apply(AST ast)
        {
            transformNode(ast.root);
        }

        private void transformNode(ASTNode node)
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

        private void transformMove(Move move)
        {
            {
                _playerModel.HandleDirection(move.direction.ToString(), move.steps.value);
            }
        }

        private void transformPickup(Pickup pickup)
        {
            _playerModel.HandleItemAction("pickup");     
        }

        private void transformDrop(Drop drop)
        {
            _playerModel.HandleItemAction("drop");
        }

        private void transformAttack(Attack attack)
        {
            _playerModel.HandleAttackAction("attack");
        }

        private void transformExit(Exit exit)
        {
            _playerModel.HandleExitAction("exit");
        }

        private void transformPause(Pause pause)
        {
            _playerModel.HandlePauseAction("pause");
        }

        private void transformReplace(Replace replace)
        {
            _playerModel.HandleReplaceAction("replace");
        }

        private void transformResume(Resume resume)
        {
            _playerModel.HandleResumeAction("resume");
        }

        private void transformSay(Say say)
        {
            _playerModel.HandleSayAction("say");
        }

        private void transformShout(Shout shout)
        {
            _playerModel.HandleShoutAction("shout");
        }
    }
}