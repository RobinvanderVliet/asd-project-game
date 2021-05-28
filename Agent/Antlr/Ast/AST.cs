using System.Collections.Generic;

namespace Agent.Antlr.Ast
{

    public class AST
    {
        public Configuration root;

        public AST( )
        {
            root = new Configuration();
        }
        public AST(Configuration root)
        {
            this.root = root;
        }

        public void SetRoot(Configuration configuration)
        {
            root = configuration;
        }

        public List<ASTError> GetErrors()
        {
            var errors = new List<ASTError>();
            CollectErrors(errors, root);
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
