using System;
using System.Diagnostics.CodeAnalysis;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Chat.antlr.ast;
using Chat.antlr.ast.actions;
using Chat.antlr.grammar;
using Chat.antlr.parser;
using Microsoft.VisualBasic;
using NUnit.Framework;

namespace Chat.Tests
{
    [ExcludeFromCodeCoverage]
    public class ParserTest
    {
        public static AST SetupParser(string text)
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

            return listener.getAST();
        }

        public static AST PickupCommand()
        {
            Input pickup = new Input();

            pickup.AddChild(new Pickup());

            return new AST(pickup);
        }

        public static AST ExitCommand()
        {
            Input pickup = new Input();

            pickup.AddChild(new Exit());

            return new AST(pickup);
        }

        public static AST AttackCommand(string direction)
        {
            Input attack = new Input();

            attack.AddChild(new Attack()
                .AddChild(new Direction(direction)));

            return new AST(attack);
        }

        public static AST DropCommand()
        {
            Input attack = new Input();

            attack.AddChild(new Drop());

            return new AST(attack);
        }

        public static AST SayCommand(string message)
        {
            Input say = new Input();

            say.AddChild(new Say()
                .AddChild(new Message(message)));

            return new AST(say);
        }

        public static AST ShoutCommand(string message)
        {
            Input shout = new Input();

            shout.AddChild(new Shout()
                .AddChild(new Message(message)));

            return new AST(shout);
        }

        public static AST ReplaceCommand()
        {
            Input replace = new Input();

            replace.AddChild(new Replace());

            return new AST(replace);
        }

        public static AST PauseCommand()
        {
            Input pause = new Input();

            pause.AddChild(new Pause());

            return new AST(pause);
        }

        public static AST ResumeCommand()
        {
            Input resume = new Input();

            resume.AddChild(new Resume());

            return new AST(resume);
        }

        [Test]
        public void DropCommandInputTest()
        {
            AST sut = SetupParser("drop");
            AST exp = DropCommand();

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void ExitCommandInputTest()
        {
            AST sut = SetupParser("exit");
            AST exp = ExitCommand();

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void AttackCommandWithDirectionAttacksForwardTest()
        {
            AST sut = SetupParser("attack forward");
            AST exp = AttackCommand("forward");

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void AttackCommandWithLeftDirectionTest()
        {
            AST sut = SetupParser("attack left");
            AST exp = AttackCommand("left");

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void AttackCommandWithRightDirectionTest()
        {
            AST sut = SetupParser("attack right");
            AST exp = AttackCommand("right");

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void AttackCommandBackWardDirectionTest()
        {
            AST sut = SetupParser("attack backward");
            AST exp = AttackCommand("backward");

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void PickupCommandTest()
        {
            AST sut = SetupParser("pickup");
            AST exp = PickupCommand();

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void SayCommandWithMessageTest()
        {
            AST sut = SetupParser("say \"hello world!\"");
            AST exp = SayCommand("\"hello world!\"");

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void ShoutCommandWithMessageTest()
        {
            AST sut = SetupParser("shout \"hello world!\"");
            AST exp = ShoutCommand("\"hello world!\"");

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void ReplaceCommandTest()
        {
            AST sut = SetupParser("replace");
            AST exp = ReplaceCommand();

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void PauseCommandTest()
        {
            AST sut = SetupParser("pause");
            AST exp = PauseCommand();

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void ResumeCommandTest()
        {
            AST sut = SetupParser("resume");
            AST exp = ResumeCommand();

            Assert.AreEqual(exp, sut);
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
                .AddChild(new Step(steps))
            );

            return new AST(moveForward);
        }
    }
}