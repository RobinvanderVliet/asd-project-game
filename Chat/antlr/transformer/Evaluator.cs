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
            var input = (Input) node;
            var nodeBody = input.body;
            foreach (var nodeObject in nodeBody)
            {
                switch (nodeObject)
                {
                    case Move:
                        TransformMove((Move) nodeObject);
                        break;
                }
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
    }
}