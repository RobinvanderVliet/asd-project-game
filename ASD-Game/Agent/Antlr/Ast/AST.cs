namespace Agent.Antlr.Ast
{

    public class AST
    {
        private Configuration _root;
        public Configuration root { get => _root; set => _root = value; }

        public AST()
        {
            _root = new Configuration();
        }
        public AST(Configuration root)
        {
            _root = root;
        }

        public void SetRoot(Configuration configuration)
        {
            _root = configuration;
        }
    }
}
