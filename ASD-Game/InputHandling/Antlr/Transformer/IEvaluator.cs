using ASD_Game.InputHandling.Antlr.Ast;

namespace ASD_Game.InputHandling.Antlr.Transformer
{
    public interface IEvaluator
    {
        void Apply(AST ast);
    }
}