using ASD_project.InputHandling.Antlr.Ast;

namespace ASD_project.InputHandling.Antlr.Transformer
{
    public interface IEvaluator
    {
        void Apply(AST ast);
    }
}