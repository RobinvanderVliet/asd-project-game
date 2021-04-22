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
                    case Move:
                        transformMove((Move)nodeBody[i]);
                        break;
                }
        }

        private void transformMove(Move move)
        {
            {
                _playerModel.HandleDirection(move.direction.ToString(), move.steps.value);
            }
        }
    }
}