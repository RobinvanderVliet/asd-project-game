
namespace InputHandling.Antlr.Ast
{
    public class ASTNode
    {
        public virtual ASTNode AddChild(ASTNode child)
        {
            return this;
        }
    }
}