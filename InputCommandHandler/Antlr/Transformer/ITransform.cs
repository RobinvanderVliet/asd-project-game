using InputCommandHandler.Antlr.Ast;

namespace InputCommandHandler.Antlr.Transformer
{
    public interface ITransform
    {
        void Apply(AST ast);
    }
}