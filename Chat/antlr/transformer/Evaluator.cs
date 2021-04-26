        {
        private void transformAttack(Attack attack)

        }
            _playerModel.HandleItemAction("drop");
        {
        private void transformDrop(Drop drop)

        }
            _playerModel.HandleItemAction("pickup");     
        {
        private void transformPickup(Pickup pickup)
        private void TransformMove(Move move)
        }
                    break;
            }
                    _playerModel.HandleDirection(move.direction.value, move.steps.value);
                default:
                    throw new MoveException("Too many steps, the maximum is 10.");
                case > 10:
                    throw new MoveException("Too few steps, the minimum is 1.");
                case < 1:
            {
            switch (move.steps.value)
        {
        }
                }
                        break;
                        transformShout((Shout)nodeBody[i]);
                        break;
                        transformSay((Say)nodeBody[i]);
                    case Shout:
                    case Say:
                        transformResume((Resume)nodeBody[i]);
                        break;
                    case Resume:
                        break;
                    case Replace:
                        transformReplace((Replace)nodeBody[i]);
                        transformDrop((Drop)nodeBody[i]);
                        break;
                        break;
                    case Move:
                        transformMove((Move)nodeBody[i]);
                    case Pause:
                        transformPause((Pause)nodeBody[i]);
                        break;
                    case Pickup:
                        transformPickup((Pickup)nodeBody[i]);
                        break;
                        break;
                        transformExit((Exit)nodeBody[i]);
                    case Exit:
                    case Drop:
                        break;
                        transformAttack((Attack)nodeBody[i]);
                {
                    case Attack:
            for (int i = 0; i < nodeBody.Count; i++)
                switch (nodeBody[i])
            var nodeBody = input.body;
            var input = (Input)node;

        private void transformNode(ASTNode node)
        {

        }
            TransformNode(ast.root);
        {
        public void Apply(AST ast)

            _playerModel = playerModel;
        }
        {
        public Evaluator(IPlayerModel playerModel)
        private readonly IPlayerModel _playerModel;

    public class Evaluator : ITransform
    {
{

namespace Chat.antlr.transformer
using Player.Model;
using Chat.exception;
using Chat.antlr.ast.actions;
﻿using Chat.antlr.ast;