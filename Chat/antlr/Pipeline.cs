using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Chat.antlr.ast;
using Chat.antlr.grammar;
using Chat.antlr.parser;

namespace Chat.antlr
{
    public class Pipeline : IAntlrErrorListener<IToken>
    {
        public AST ast { get; set; }
        public List<String> errors { get; }

        public Pipeline()
        {
            errors = new List<string>();
        }

        public void parseCommando(String input)
        {
            //Lex (with Antlr's generated lexer)
            if (!input.StartsWith("say") && !input.StartsWith("whisper") && !input.StartsWith("shout"))
            {
                input = input.ToLower();
            }
            AntlrInputStream inputStream = new AntlrInputStream(input);
            PlayerCommandsLexer lexer = new PlayerCommandsLexer(inputStream);
            lexer.RemoveErrorListeners();
            errors.Clear();
            try
            {
                CommonTokenStream tokens = new CommonTokenStream(lexer);

                //Parse (with Antlr's generated parser)
                var errorListener = new Pipeline();
                PlayerCommandsParser parser = new PlayerCommandsParser(tokens);
                parser.RemoveErrorListeners();
                parser.AddErrorListener(errorListener);

                var parseTree = parser.input();

                //Extract AST from the Antlr parse tree
                ASTListener listener = new ASTListener();
                ParseTreeWalker walker = new ParseTreeWalker();
                walker.Walk(listener, parseTree);

                ast = listener.ast;
            }
            catch (RecognitionException e)
            {
                this.ast = new AST();
                //errors.add(e.getMessage());
            }
            catch (ParseCanceledException e)
            {
                this.ast = new AST();
                //errors.add("Syntax error");
            }
        }


        public void clearErrors()
        {
            errors.Clear();
        }
        

        public void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg,
            RecognitionException e)
        {
            Console.WriteLine(msg);
            //throw new NotImplementedException();
        }
    }
}