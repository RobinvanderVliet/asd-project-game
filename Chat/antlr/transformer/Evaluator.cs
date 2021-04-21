using System.Security.Cryptography;
using Chat.antlr.ast;
using Chat.antlr.ast.actions;

namespace Chat.antlr.transformer
{
    public class Evaluator : ITransform
    {
        public void apply(AST ast)
        {
            transformNode(ast.root);
        }

        private void transformNode(ASTNode node)
        {
            if (node is Move)
            {
                transformMove(node);
            }
        }

        private void transformMove(ASTNode node)
        {
            Move move = (Move)node;
            // if (move.direction.ToString() == "forward")
            {
                
                // .Net methode (move.direction.ToString(), move.steps)
            }
        }
    }
}