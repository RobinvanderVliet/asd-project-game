using Chat.antlr.ast;

namespace Chat.antlr.transformer
{
    public interface ITransform
    {
        void Apply(AST ast);
    }
}