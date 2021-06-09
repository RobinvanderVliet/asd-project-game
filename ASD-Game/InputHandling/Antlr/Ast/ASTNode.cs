namespace ASD_Game.InputHandling.Antlr.Ast
{
    public class ASTNode
    {
        public virtual ASTNode AddChild(ASTNode child)
        {
            return this;
        }
    }
}