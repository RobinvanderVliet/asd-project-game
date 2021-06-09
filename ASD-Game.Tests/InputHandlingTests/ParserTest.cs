using System;
using System.Diagnostics.CodeAnalysis;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using ASD_Game.InputHandling.Antlr.Ast;
using ASD_Game.InputHandling.Antlr.Ast.Actions;
using ASD_Game.InputHandling.Antlr.Parser;
using InputHandling.Antlr.Grammar;
using NUnit.Framework;

namespace ASD_Game.Tests.InputHandlingTests
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

        public static AST PickupCommand(int number)
        {
            Input pickup = new Input();

            pickup.AddChild(new Pickup()
                .AddChild(new Step(number)));

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
            Input drop = new Input();

            drop.AddChild(new Drop());

            return new AST(drop);
        }

        public static AST SayCommand(string message)
        {
            Input say = new Input();

            say.AddChild(new Say()
                .AddChild(new Message(message)));

            return new AST(say);
        }

        public static AST UseCommand(int index)
        {
            Input use = new Input();

            use.AddChild(new Use()
                .AddChild(new Step(index)));

            return new AST(use);
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
        
        public static AST SearchCommand()
        {
            Input search = new Input();

            search.AddChild(new Search());

            return new AST(search);
        }

        [Test]
        public void Test_AstListener_CreatesDropAst()
        {
            //act
            AST exp = DropCommand();
            //arrange
            AST sut = SetupParser("drop");
            //assert
            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesExitAst()
        {
            //act
            AST exp = ExitCommand();
            //arrange
            AST sut = SetupParser("exit");
            //assert
            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesAttackAstTWithDirectionForward()
        {
            //act
            AST exp = AttackCommand("forward");
            //arrange
            AST sut = SetupParser("attack forward");
            //assert
            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesAttackAstTWithDirectionLeft()
        {
            //act
            AST exp = AttackCommand("left");
            //arrange
            AST sut = SetupParser("attack left");
            //assert
            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesAttackAstTWithDirectionRight()
        {
            //act
            AST exp = AttackCommand("right");
            //arrange
            AST sut = SetupParser("attack right");
            //assert
            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesAttackAstTWithDirectionBackward()
        {
            //act
            AST exp = AttackCommand("backward");
            //arrange
            AST sut = SetupParser("attack backward");
            //assert
            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesPickupAst()
        {
            // Act
            AST exp = PickupCommand(1);
            
            // Arrange
            AST sut = SetupParser("pickup 1");
            
            // Assert
            Assert.AreEqual(exp, sut);
        }
        
        [Test]
        public void Test_AstListener_CreatesPickupAstWithDoubleDigits()
        {
            // Act
            AST exp = PickupCommand(10);
            
            // Arrange
            AST sut = SetupParser("pickup 10");
            
            // Assert
            Assert.AreEqual(exp, sut);
        }
        
        [Test]
        public void Test_AstListener_CreatesSayAstWithMessage()
        {
            //act
            AST exp = SayCommand("\"hello world!\"");
            //arrange
            AST sut = SetupParser("say \"hello world!\"");
            //assert
            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesUseAstWithIndex()
        {
            //act
            int index = 3;
            AST exp = UseCommand(index);
            //arrange
            AST sut = SetupParser("use 3");
            //assert
            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesShoutAstWithMessage()
        {
            //act
            AST exp = ShoutCommand("\"hello world!\"");
            //arrange
            AST sut = SetupParser("shout \"hello world!\"");
            //assert
            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesReplaceAst()
        {
            //act
            AST exp = ReplaceCommand();
            //arrange
            AST sut = SetupParser("replace");
            //assert
            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesPauseAst()
        {
            //act
            AST exp = PauseCommand();
            //arrange
            AST sut = SetupParser("pause");
            //assert
            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesResumeAst()
        {
            //act
            AST exp = ResumeCommand();
            //arrange
            AST sut = SetupParser("resume");
            //assert
            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesSearchAst()
        {
            //act
            AST exp = SearchCommand();
            //arrange
            AST sut = SetupParser("search");
            //assert
            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesMoveAstWithForward2Steps()
        {
            //act
            AST exp = MoveCommand(2, "forward");
            //arrange
            AST sut = SetupParser("move forward 2");
            //assert
            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesMoveAstWithForward1Step()
        {
            //act
            AST exp = MoveCommand(1, "forward");
            //arrange
            AST sut = SetupParser("move forward 1");
            //assert
            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesMoveAstWithForward0Steps()
        {
            //act
            AST exp = MoveCommand(1, "forward"); // if no steps entered always 1 step 
            //arrange
            AST sut = SetupParser("move forward");
            //assert
            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesMoveAstWithBackward2Steps()
        {
            //act
            AST exp = MoveCommand(2, "backward");
            //arrage
            AST sut = SetupParser("move backward 2");
            //assert
            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesMoveAstWithBackward1Step()
        {
            //act
            AST exp = MoveCommand(1, "backward");
            //arrange
            AST sut = SetupParser("move backward 1");
            //assert
            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesMoveAstWithBackward0Steps()
        {
            //act
            AST exp = MoveCommand(1, "backward"); // if no steps entered always 1 step 
            //arrange
            AST sut = SetupParser("move backward");
            //assert
            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesMoveAstWithLeft2Steps()
        {
            //act
            AST exp = MoveCommand(2, "left");
            //arrange
            AST sut = SetupParser("move left 2");
            //assert
            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesMoveAstWithLeft1Step()
        {
            //act
            AST exp = MoveCommand(1, "left");
            //arrange
            AST sut = SetupParser("move left 1");
            //assert
            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesMoveAstWithLeft0Steps()
        {
            //act
            AST exp = MoveCommand(1, "left"); // if no steps entered always 1 step 
            //arrange
            AST sut = SetupParser("move left");
            //assert
            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesMoveAstWithRight2Steps()
        {
            //act
            AST exp = MoveCommand(2, "right");
            //arrange
            AST sut = SetupParser("move right 2");
            //assert
            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesMoveAstWithRight2Step()
        {
            //act
            AST exp = MoveCommand(1, "right");
            //arrange
            AST sut = SetupParser("move right 1");
            //assert
            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesMoveAstWithRight0Steps()
        {
            //act
            AST exp = MoveCommand(1, "right"); // if no steps entered always 1 step 
            //arrange
            AST sut = SetupParser("move right");
            //assert
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

        [Test]
        public void Test_AstListener_CreatesRequestSessions()
        {
            // Arrange
            AST exp = RequestSessionsCommand();
            
            // Act
            AST sut = SetupParser("request_sessions");

            // Assert
            Assert.AreEqual(exp, sut);
        }

        private static AST RequestSessionsCommand()
        {
            Input requestSessions = new Input();

            requestSessions.AddChild(new RequestSessions());

            return new AST(requestSessions);
        }

        [Test]
        public void Test_AstListener_CreatesCreateSession()
        {
            // Arrange
            const string sessionName = "cool world";
            AST exp = CreateSessionCommand(sessionName);
            
            // Act
            AST sut = SetupParser($"create_session \"{sessionName}\"");

            // Assert
            Assert.AreEqual(exp, sut);
        }

        private static AST CreateSessionCommand(string sessionName)
        {
            Input createSession = new Input();

            createSession.AddChild(new CreateSession()
                .AddChild(new Message(sessionName)));

            return new AST(createSession);
        }

        [Test]
        public void Test_AstListener_CreatesJoinSession()
        {
            // Arrange
            const string sessionId = "1234-1234";
            AST exp = JoinSessionCommand(sessionId);
            
            // Act
            AST sut = SetupParser($"join_session \"{sessionId}\"");

            // Assert
            Assert.AreEqual(exp, sut);
        }

        private static AST JoinSessionCommand(string sessionId)
        {
            Input joinSession = new Input();

            joinSession.AddChild(new JoinSession()
                .AddChild(new Message(sessionId)));

            return new AST(joinSession);
        }
        
        [Test]
        public void Test_AstListener_StartSession()
        {
            // Arrange
            AST exp = StartSessionCommand();
            
            // Act
            AST sut = SetupParser("start_session");

            // Assert
            Assert.AreEqual(exp, sut);
        }

        private static AST StartSessionCommand()
        {
            Input startSession = new Input();

            startSession.AddChild(new StartSession());

            return new AST(startSession);
        }
        
        public static AST InspectCommand(string inventorySlot)
        {
            Input inspect = new Input();

            inspect.AddChild(new Inspect()
                .AddChild(new InventorySlot(inventorySlot)));
            
            return new AST(inspect);
        }
        
        [Test]
        public void Test_AstListener_CreatesInspectCommandWithArmor()
        {
            // Arrange
            const string inventorySlot = "armor";
            AST exp = InspectCommand(inventorySlot);
            
            // Act
            AST sut = SetupParser($"inspect {inventorySlot}");

            // Assert
            Assert.AreEqual(exp, sut);
        }
        
        [Test]
        public void Test_AstListener_CreatesInspectCommandWithHelmet()
        {
            // Arrange
            const string inventorySlot = "helmet";
            AST exp = InspectCommand(inventorySlot);
            
            // Act
            AST sut = SetupParser($"inspect {inventorySlot}");

            // Assert
            Assert.AreEqual(exp, sut);
        }
        
        [Test]
        public void Test_AstListener_CreatesInspectCommandWithWeapon()
        {
            // Arrange
            const string inventorySlot = "weapon";
            AST exp = InspectCommand(inventorySlot);
            
            // Act
            AST sut = SetupParser($"inspect {inventorySlot}");

            // Assert
            Assert.AreEqual(exp, sut);
        }

        [Test]
        public void Test_AstListener_CreatesInspectCommandWithSlot1()
        {
            // Arrange
            const string inventorySlot = "slot 1";
            AST exp = InspectCommand(inventorySlot);

            // Act
            AST sut = SetupParser($"inspect {inventorySlot}");

            // Assert
            Assert.AreEqual(exp, sut);
        }
    }
}