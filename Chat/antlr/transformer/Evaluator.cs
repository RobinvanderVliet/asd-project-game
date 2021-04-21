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
            switch (node)
            {
                case Move:
                    transformMove(node);
                    break;
            }
        }

        private void transformMove(ASTNode node)
        {
            Move move = (Move)node;
            {
                _playerModel.HandleDirection(move.direction.ToString(), Convert.ToInt32(move.steps));
            }
        }
    }
}