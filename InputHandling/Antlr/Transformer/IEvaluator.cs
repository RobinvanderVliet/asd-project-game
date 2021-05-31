using InputHandling.Antlr.Ast;

namespace InputHandling.Antlr.Transformer
{
    public interface IEvaluator
    {
        void Apply(AST ast);
    }
}