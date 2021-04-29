using InputCommandHandler.antlr.ast;

namespace InputCommandHandler.antlr.transformer
{
    public interface ITransform
    {
        void Apply(AST ast);
    }
}