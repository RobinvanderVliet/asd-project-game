namespace Agent.Antlr.Ast
{
    public class ASTError
    {
        private string _message;


        public ASTError(string message)
        {
            this._message = message;
        }


        override public string ToString()
        {
            return "ERROR: " + this._message;
        }
    }
}