namespace Agent.Antlr.Ast
{

    public class AST
    {
        public Configuration root;

        public AST( )
        {
            this.root = new Configuration();
        }
        public AST(Configuration root)
        {
            this.root = root;
        }

        public void SetRoot(Configuration configuration)
        {
            this.root = configuration;
        }
    }
}
