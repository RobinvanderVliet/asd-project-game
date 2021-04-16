using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Chat.antlr.ast;
using Chat.antlr.grammar;
using Chat.antlr.parser;
using NUnit.Framework;

namespace Chat.Tests
{
    public class ParserTest
    {


        public AST SetupParser(string text)
        {
            AntlrInputStream inputStream = new AntlrInputStream(text);
            PlayerCommandsLexer lexer = new PlayerCommandsLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            PlayerCommandsParser parser = new PlayerCommandsParser(commonTokenStream);

            ASTListener listener = new ASTListener();
            try
            {
                var parseTree = parser.input();
                ParseTreeWalker walker = new ParseTreeWalker();
                walker.Walk(listener, parseTree);
            }
            catch (ParseCanceledException e)
            {
                Assert.Fail(e.ToString()); 
            }
            return listener.Ast;
        }
        
    }
}