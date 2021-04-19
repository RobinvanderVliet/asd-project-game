using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Chat.antlr.ast;
using Chat.antlr.ast.actions;
using Chat.antlr.grammar;
using Chat.antlr.parser;
using Microsoft.VisualBasic;
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

            return listener.getAST();
        }
        
        public static AST PickUpCommand()
        {
            Input pickUp = new Input();

            pickUp.addChild(new PickUp());

            return new AST(pickUp);
        }

        public static AST MoveForwardCommand()
        {
            Input moveForward = new Input();
            
            moveForward.addChild(new Move()
                .addChild(new Direction("forward"))
                .addChild(new Step(2)));

            return new AST(moveForward);
        }
        
        public static AST ExitCommand()
        {
            Input pickUp = new Input();

            pickUp.addChild(new Exit());
            
            return new AST(pickUp);
        }


        public static AST AttackCommand(string direction)
        {
            Input attack = new Input();

            attack.addChild(new Attack()
                .addChild(new Direction(direction)));

            return new AST(attack);
        }
        
        public static AST DropCommand()
        {
            Input attack = new Input();

            attack.addChild(new Drop());

            return new AST(attack);
        }        
        
        public void DropCommandInputTest()
        {
            AST sut = SetupParser("drop");
            AST exp = DropCommand();

            Assert.AreEqual(exp, sut);
        }
        

        public void ExitCommandInputTest()
        {
            AST sut = SetupParser("exit");
            AST exp = ExitCommand();

            Assert.AreEqual(exp, sut);
        }

        public void AttackCommandWithoutDirectionAttacksForwardTest(string direction)
        {
            AST sut = SetupParser("attack");
            AST exp = AttackCommand("");

            Assert.AreEqual(exp, sut);
        }
        public void AttackCommandWithLeftDirectionTest(string direction)
        {
            AST sut = SetupParser("attack left");
            AST exp = AttackCommand("left");

            Assert.AreEqual(exp, sut);
        }
        public void AttackCommandWithRightDirectionTest(string direction)
        {
            AST sut = SetupParser("attack right");
            AST exp = AttackCommand("right");

            Assert.AreEqual(exp, sut);
        }
        
        public void AttackCommandBackWardDirectionTest(string direction)
        {
            AST sut = SetupParser("attack backward");
            AST exp = AttackCommand("backward");

            Assert.AreEqual(exp, sut);
        }


      

        public void PickUpCommandTest()
        {
            AST sut = SetupParser("pickup");
            AST exp = PickUpCommand();

            Assert.AreEqual(exp, sut);
        }

        public void PickUpCommandWithWronglyTypedPickedUpThrowsExceptionTest()
        {
            AST sut = SetupParser("pickup left");
            AST exp = PickUpCommand();


            Assert.AreEqual(exp, sut);
        }


        [Test]
        public void AstListenerEnterMoveAddsToContainerTest()
        {
            AST sut = SetupParser("move forward 2");
            AST exp = MoveForwardCommand();

            Assert.AreEqual(exp, sut);
        }
    }
}