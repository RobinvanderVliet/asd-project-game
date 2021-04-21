using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Chat.antlr.ast;
using Chat.antlr.ast.actions;
using Chat.antlr.grammar;
using Chat.antlr.parser;
using NUnit.Framework;

namespace Chat.Tests
{public class ParserTest
    {


        public AST SetupParser(string text)
        {
            AntlrInputStream inputStream = new AntlrInputStream(text);
            PlayerCommandsLexer lexer = new PlayerCommandsLexer(inputStream);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            
            PlayerCommandsParser parser = new PlayerCommandsParser(tokens);
            
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
            return listener.getAST();
        }

        public static AST MoveForwardCommand()
        {
            Input moveForward = new Input();


            moveForward.addChild(new Move()
                .addChild(new Direction("forward"))
                .addChild(new Step(2)));
            
            return new AST(moveForward); 

        }

        [Test]
        public void AstListenerEnterMoveAddsToContainerTest()
        {
            AST sut = SetupParser("move up 2");
            AST exp = MoveForwardCommand();

            Assert.AreEqual(exp, sut);
        }
    }
    
}