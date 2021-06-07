using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using ASD_project.InputHandling.Antlr.Ast;
using ASD_project.InputHandling.Antlr.Parser;
using ASD_project.InputHandling.Antlr.Transformer;
using ASD_project.InputHandling.Exceptions;
using InputHandling.Antlr.Grammar;

namespace ASD_project.InputHandling.Antlr
{
    public class Pipeline : IAntlrErrorListener<IToken>, IPipeline
    {
        private IEvaluator _evaluator;
        private AST _ast;
        public AST Ast { get => _ast; private set => _ast = value; }

        public Pipeline(IEvaluator evaluator)
        {
            _evaluator = evaluator;
        }
        public void SyntaxError(IRecognizer recognizer, 
                                IToken offendingSymbol, 
                                int line, 
                                int charPositionInLine, 
                                string msg, 
                                RecognitionException e)
        {
            throw new CommandSyntaxException(msg);
        }

        public void ParseCommand(string input)
        {
            //Lex (with Antlr's generated lexer)
            if (!input.StartsWith("say") && !input.StartsWith("whisper") && !input.StartsWith("shout"))
            {
                input = input.ToLower();
            }

            var inputStream = new AntlrInputStream(input);
            var lexer = new PlayerCommandsLexer(inputStream);
            lexer.RemoveErrorListeners();

            var tokens = new CommonTokenStream(lexer);

            //Parse (with Antlr's generated parser)
            var parser = new PlayerCommandsParser(tokens);
            parser.RemoveErrorListeners();
            parser.AddErrorListener(this);

            var parseTree = parser.input();

            //Extract AST from the Antlr parse tree
            var listener = new ASTListener();
            var walker = new ParseTreeWalker();
            walker.Walk(listener, parseTree);

            _ast = listener.getAST();
        }
        public void Transform()
        {
            if (_ast == null)
            {
                return;
            }
            _evaluator.Apply(_ast);
        }
    }
}