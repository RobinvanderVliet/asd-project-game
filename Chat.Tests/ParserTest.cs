using System;
using System.Diagnostics.CodeAnalysis;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Chat.antlr.ast;
using Chat.antlr.ast.actions;
using Chat.antlr.grammar;
using Chat.antlr.parser;
using NUnit.Framework;

namespace Chat.Tests
{
    [ExcludeFromCodeCoverage]
    public class ParserTest
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
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }

            return listener.ast;
        }

        [Test]
        public void Test_AstListener_MoveForward2Steps()
        {
            AST sut = SetupParser("move forward 2");
            AST exp = MoveCommand(2, "forward");

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_MoveForward1Steps()
        {
            AST sut = SetupParser("move forward 1");
            AST exp = MoveCommand(1, "forward");

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_MoveForwardWithoutEnteringSteps()
        {
            AST sut = SetupParser("move forward");
            AST exp = MoveCommand(1, "forward"); // if no steps entered always 1 step 

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_MoveBackward2Steps()
        {
            AST sut = SetupParser("move backward 2");
            AST exp = MoveCommand(2, "backward");

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_MoveBackward1Steps()
        {
            AST sut = SetupParser("move backward 1");
            AST exp = MoveCommand(1, "backward");

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_MoveBackwardWithoutEnteringSteps()
        {
            AST sut = SetupParser("move backward");
            AST exp = MoveCommand(1, "backward"); // if no steps entered always 1 step 

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_MoveLeft2Steps()
        {
            AST sut = SetupParser("move left 2");
            AST exp = MoveCommand(2, "left");

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_MoveLeft1Steps()
        {
            AST sut = SetupParser("move left 1");
            AST exp = MoveCommand(1, "left");

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_MoveLeftWithoutEnteringSteps()
        {
            AST sut = SetupParser("move left");
            AST exp = MoveCommand(1, "left"); // if no steps entered always 1 step 

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_MoveRight2Steps()
        {
            AST sut = SetupParser("move right 2");
            AST exp = MoveCommand(2, "right");

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_MoveRight1Steps()
        {
            AST sut = SetupParser("move right 1");
            AST exp = MoveCommand(1, "right");

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_MoveRightWithoutEnteringSteps()
        {
            AST sut = SetupParser("move right");
            AST exp = MoveCommand(1, "right"); // if no steps entered always 1 step 

            Assert.AreEqual(exp, sut);
        }
        
        // Help method for checking command
        public static AST MoveCommand(int steps, string direction)
        {
            Input moveForward = new Input();


            moveForward.AddChild(new Move()
                .AddChild(new Direction(direction))
                .AddChild(new Step(steps)));

            return new AST(moveForward);
        }
    }
}