/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD-project-game.
 
    This file is created by team: 2
     
    Goal of this file: pipeline for parse command.
     
*/

using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Chat.antlr.ast;
using Chat.antlr.grammar;
using Chat.antlr.parser;
using Chat.antlr.transformer;
using Chat.exception;

namespace Chat.antlr
{
    public class Pipeline : IAntlrErrorListener<IToken>
    {
        public AST Ast { get; private set; }

        public Pipeline()
        {
        }

        public void ParseCommand(String input)
        {
            //Lex (with Antlr's generated lexer)
            if (!input.StartsWith("say") && !input.StartsWith("whisper") && !input.StartsWith("shout"))
            {
                input = input.ToLower();
            }

            AntlrInputStream inputStream = new AntlrInputStream(input);
            PlayerCommandsLexer lexer = new PlayerCommandsLexer(inputStream);
            lexer.RemoveErrorListeners();

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

            Ast = listener.ast;
        }


        public void transform()
        {
            if (Ast == null)
                return;
            (new Evaluator()).apply(Ast);
        }

        public void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine,
            string msg,
            RecognitionException e)
        {
            throw new CommandSyntaxException(msg);
        }
    }
}