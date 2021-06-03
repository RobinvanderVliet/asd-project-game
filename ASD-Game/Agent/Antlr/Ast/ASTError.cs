namespace Agent.Antlr.Ast
{
    public class ASTError
    {
        private string _message;


        public ASTError(string message)
        {
            _message = message;
        }


        override public string ToString()
        {
            return "ERROR: " + _message;
        }
    }
}