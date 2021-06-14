using System.Collections.Generic;

namespace ASD_Game.Agent.Antlr.Ast
{

    public class AST
    {
        public Configuration Root;

        public AST()
        {
            Root = new Configuration();
        }
        public AST(Configuration root)
        {
            Root = root;
        }

        public void SetRoot(Configuration configuration)
        {
            Root = configuration;
        }

        public virtual List<ASTError> GetErrors()
        {
            var errors = new List<ASTError>();
            CollectErrors(errors, Root);
            return errors;
        }
        private void CollectErrors(List<ASTError> errors, Node node)
        {
            if (node.HasError())
            {
                errors.Add(node.GetError());
            }
            foreach (var child in node.GetChildren())
            {
                CollectErrors(errors,child);
            }
        }
    }
}
