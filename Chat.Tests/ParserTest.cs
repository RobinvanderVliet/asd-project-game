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
{
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
            catch (ParseCanceledException e)
            {
                Assert.Fail(e.ToString());
            }
            
            return listener.ast;
        }

        [Test]
        public void AstListenerMoveForward2StepsTest()
        {
            AST sut = SetupParser("move forward 2");
            AST exp = MoveForwardCommand(2);

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void AstListenerMoveForward1StepsTest()
        {
            AST sut = SetupParser("move forward 1");
            AST exp = MoveForwardCommand(1);

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void AstListenerMoveForwardWithoutEnteringStepsTest()
        {
            AST sut = SetupParser("move forward");
            AST exp = MoveForwardCommand(1); // if no steps entered always 1 step 

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void AstListenerMoveBackward2StepsTest()
        {
            AST sut = SetupParser("move backward 2");
            AST exp = MoveBackwardCommand(2);

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void AstListenerMoveBackward1StepsTest()
        {
            AST sut = SetupParser("move backward 1");
            AST exp = MoveBackwardCommand(1);

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void AstListenerMoveBackwardWithoutEnteringStepsTest()
        {
            AST sut = SetupParser("move backward");
            AST exp = MoveBackwardCommand(1); // if no steps entered always 1 step 

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void AstListenerMoveLeft2StepsTest()
        {
            AST sut = SetupParser("move left 2");
            AST exp = MoveLeftCommand(2);

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void AstListenerMoveLeft1StepsTest()
        {
            AST sut = SetupParser("move left 1");
            AST exp = MoveLeftCommand(1);

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void AstListenerMoveLeftWithoutEnteringStepsTest()
        {
            AST sut = SetupParser("move left");
            AST exp = MoveLeftCommand(1); // if no steps entered always 1 step 

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void AstListenerMoveRight2StepsTest()
        {
            AST sut = SetupParser("move right 2");
            AST exp = MoveRightCommand(2);

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void AstListenerMoveRight1StepsTest()
        {
            AST sut = SetupParser("move right 1");
            AST exp = MoveRightCommand(1);

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void AstListenerMoveRightWithoutEnteringStepsTest()
        {
            AST sut = SetupParser("move right");
            AST exp = MoveRightCommand(1); // if no steps entered always 1 step 

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void AstListenerThrowsSyntaxErrorWhenMoveCommandIsNotRecognised()
        {
            AST sut = SetupParser("mv left 1");
            // new error
        }
        
        [Test]
        public void AstListenerThrowsSyntaxErrorWhenDirectionCommandIsNotRecognised()
        {
            AST sut = SetupParser("mv lf 1");
            // new error
        }

        // Help functions for checking command
        public static AST MoveForwardCommand(int steps)
        {
            Input moveForward = new Input();


            moveForward.addChild(new Move()
                .addChild(new Direction("forward"))
                .addChild(new Step(steps)));

            return new AST(moveForward);
        }

        public static AST MoveBackwardCommand(int steps)
        {
            Input moveForward = new Input();


            moveForward.addChild(new Move()
                .addChild(new Direction("backward"))
                .addChild(new Step(steps)));

            return new AST(moveForward);
        }

        public static AST MoveLeftCommand(int steps)
        {
            Input moveForward = new Input();


            moveForward.addChild(new Move()
                .addChild(new Direction("left"))
                .addChild(new Step(steps)));

            return new AST(moveForward);
        }

        public static AST MoveRightCommand(int steps)
        {
            Input moveForward = new Input();


            moveForward.addChild(new Move()
                .addChild(new Direction("right"))
                .addChild(new Step(steps)));

            return new AST(moveForward);
        }
    }
}