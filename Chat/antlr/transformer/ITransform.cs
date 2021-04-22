using Chat.antlr.ast;

namespace Chat.antlr.transformer
{
    public interface ITransform
    {
        void apply(AST ast);
    }
}