using Antlr4.Runtime;

namespace Agent.Tests.parser
{
    public class TestErrorHandler : BaseErrorListener
    {

        private string message;
        
        override public void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg,
            RecognitionException e)
        {
            this.message = msg;
        }

        override public string ToString()
        {
            return message;
        }
    }
}