using System;
using System.Diagnostics.CodeAnalysis;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using InputCommandHandler.Antlr.Ast;
using InputCommandHandler.Antlr.Ast.Actions;
using InputCommandHandler.Antlr.Grammar;
using InputCommandHandler.Antlr.Parser;
using NUnit.Framework;

namespace InputCommandHandler.Tests
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
        public void Test_AstListener_CreatesDropAst()
        {
            AST exp = DropCommand();
            
            AST sut = SetupParser("drop");

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesExitAst()
        {
            AST exp = ExitCommand();
            
            AST sut = SetupParser("exit");

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesAttackAstTWithDirectionForward()
        {
            AST exp = AttackCommand("forward");
            
            AST sut = SetupParser("attack forward");

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesAttackAstTWithDirectionLeft()
        {
            AST exp = AttackCommand("left");
            
            AST sut = SetupParser("attack left");

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesAttackAstTWithDirectionRight()
        {
            AST exp = AttackCommand("right");
            
            AST sut = SetupParser("attack right");

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesAttackAstTWithDirectionBackward()
        {
            AST exp = AttackCommand("backward");
            
            AST sut = SetupParser("attack backward");

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesPickupAst()
        {
            AST exp = PickupCommand();
            
            AST sut = SetupParser("pickup");

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesSayAstWithMessage()
        {
            AST exp = SayCommand("\"hello world!\"");
            
            AST sut = SetupParser("say \"hello world!\"");

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesShoutAstWithMessage()
        {
            AST exp = ShoutCommand("\"hello world!\"");
            
            AST sut = SetupParser("shout \"hello world!\"");

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesReplaceAst()
        {
            AST exp = ReplaceCommand();
            
            AST sut = SetupParser("replace");

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesPauseAst()
        {
            AST exp = PauseCommand();
            
            AST sut = SetupParser("pause");

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesResumeAst()
        {
            AST exp = ResumeCommand();
            
            AST sut = SetupParser("resume");

            Assert.AreEqual(exp, sut);
        }
        
        [Test]
        public void Test_AstListener_CreatesMoveAstWithForward2Steps()
        {
            AST exp = MoveCommand(2, "forward");
            
            AST sut = SetupParser("move forward 2");

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesMoveAstWithForward1Step()
        {
            AST exp = MoveCommand(1, "forward");
            
            AST sut = SetupParser("move forward 1");

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesMoveAstWithForward0Steps()
        {
            AST exp = MoveCommand(1, "forward"); // if no steps entered always 1 step 
            
            AST sut = SetupParser("move forward");

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesMoveAstWithBackward2Steps()
        {
            AST exp = MoveCommand(2, "backward");
            
            AST sut = SetupParser("move backward 2");

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesMoveAstWithBackward1Step()
        {
            AST exp = MoveCommand(1, "backward");
            
            AST sut = SetupParser("move backward 1");

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesMoveAstWithBackward0Steps()
        {
            AST exp = MoveCommand(1, "backward"); // if no steps entered always 1 step 
            
            AST sut = SetupParser("move backward");

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesMoveAstWithLeft2Steps()
        {
            AST exp = MoveCommand(2, "left");
            
            AST sut = SetupParser("move left 2");

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesMoveAstWithLeft1Step()
        {
            AST exp = MoveCommand(1, "left");
            
            AST sut = SetupParser("move left 1");

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesMoveAstWithLeft0Steps()
        {
            AST exp = MoveCommand(1, "left"); // if no steps entered always 1 step 
            
            AST sut = SetupParser("move left");

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesMoveAstWithRight2Steps()
        {
            AST exp = MoveCommand(2, "right");
            
            AST sut = SetupParser("move right 2");

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesMoveAstWithRight2Step()
        {
            AST exp = MoveCommand(1, "right");
            
            AST sut = SetupParser("move right 1");

            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesMoveAstWithRight0Steps()
        {
            AST exp = MoveCommand(1, "right"); // if no steps entered always 1 step 
            
            AST sut = SetupParser("move right");

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
        
        public static AST InspectCommand(string inventorySlot)
        {
            Input inspect = new Input();

            inspect.AddChild(new Inspect()
                .AddChild(new InventorySlot(inventorySlot)));
            
            return new AST(inspect);
        }
        
        [Test]
        public void Test_AstListener_CreatesInspectCommand()
        {
            // Arrange
            const string inventorySlot = "armor";
            AST exp = InspectCommand(inventorySlot);
            
            // Act
            AST sut = SetupParser($"inspect \"{inventorySlot}\"");

            // Assert
            Assert.AreEqual(exp, sut);
        }
    }
}