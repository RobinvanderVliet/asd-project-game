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

        public void Apply(AST ast)
        {
            TransformNode(ast.root);
        }

        private void TransformNode(ASTNode node)
        {
            var input = (Input) node;
            var nodeBody = input.body;
            for (int i = 0; i < nodeBody.Count; i++)
                switch (nodeBody[i])
                {
                    case Move:
                        TransformMove((Move) nodeBody[i]);
                        break;
                }
        }

        private void TransformMove(Move move)
        {
            _playerModel.HandleDirection(move.direction.value,move.steps.value);
        }
    }
}