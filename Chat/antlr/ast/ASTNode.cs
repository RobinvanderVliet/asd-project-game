namespace Chat.antlr.ast
{
    public class ASTNode
    {
        public virtual ASTNode AddChild(ASTNode child)
        {
            return this;
        }
    }
}