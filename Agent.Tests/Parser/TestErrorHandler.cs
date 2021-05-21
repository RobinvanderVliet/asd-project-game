using Antlr4.Runtime;
using System.Diagnostics.CodeAnalysis;

namespace Agent.Tests.parser
{
    [ExcludeFromCodeCoverage]
    public class TestErrorHandler : BaseErrorListener
    {

        private string message;
        
        override public void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg,
            RecognitionException e)
        {
            message = msg;
        }

        override public string ToString()
        {
            return message;
        }
    }
}