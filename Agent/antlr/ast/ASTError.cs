namespace Agent.antlr.ast
{
    public class ASTError
    {
        private string message;


        public ASTError(string message)
        {
            this.message = message;
        }


        override public string ToString()
        {
            return "ERROR: " + this.message;
        }
    }
}