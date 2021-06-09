namespace ASD_Game.Agent.Antlr.Ast
{
    public class ASTError
    {
        private readonly string _message;


        public ASTError(string message)
        {
            _message = message;
        }


        public override string ToString()
        {
            return "ERROR: " + _message;
        }
    }
}