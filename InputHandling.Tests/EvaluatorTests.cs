using System.Diagnostics.CodeAnalysis;
using Chat;
using InputHandling.Antlr.Ast;
using InputHandling.Antlr.Ast.Actions;
using InputHandling.Antlr.Transformer;
using InputHandling.Exceptions;
using Moq;
using NUnit.Framework;
using Player.ActionHandlers;
using Session;

namespace InputHandling.Tests
{
    [ExcludeFromCodeCoverage]
    public class EvaluatorTests
    {
        private Evaluator _sut;
        private Mock<ISessionHandler> _mockedSessionHandler;
        private Mock<IMoveHandler> _mockedMoveHandler;
        private Mock<IGameSessionHandler> _mockedGameSessionHandler;
        private Mock<IChatHandler> _mockedChatHandler;
    
        [SetUp]
        public void Setup()
        {
            _mockedSessionHandler = new Mock<ISessionHandler>();
            _mockedMoveHandler = new Mock<IMoveHandler>();
            _mockedGameSessionHandler = new Mock<IGameSessionHandler>();
            _mockedChatHandler = new Mock<IChatHandler>();
            _sut = new Evaluator(_mockedSessionHandler.Object, _mockedMoveHandler.Object, _mockedGameSessionHandler.Object, _mockedChatHandler.Object);
        }
    
        // [Test]
        // public void Test_HandleDirection_RunsCorrectly()
        // {
        //     //arrange
        //     var ast = MoveAST(1, "up");
        //
        //     _mockedPlayerService.Setup(x => x.HandleDirection("up", 1));
        //     //act
        //     _sut.Apply(ast);
        //     //assert
        //     _mockedPlayerService.VerifyAll();
        // }
        
        [Test]
        public void Test_HandleDirection_ThrowsExceptionWithStepsLessThan1()
        {
            //arrange
            var ast = MoveAST(0, "up");
            //act & assert
            Assert.Throws<MoveException>(() => _sut.Apply(ast));
        }
    
        [Test]
        public void Test_HandleDirection_ThrowsExceptionWithStepsMoreThan10()
        {
            //arrange
            var ast = MoveAST(11, "up");
            //act & assert
            Assert.Throws<MoveException>(() => _sut.Apply(ast));
        }
    
        public static AST MoveAST(int steps, string direction)
        {
            Input move = new Input();
            move.AddChild(new Move()
                .AddChild(new Direction(direction))
                .AddChild(new Step(steps)));
            return new AST(move);
        }
    
        // [Test]
        // public void Test_Apply_HandlePickupActionIsCalled()
        // {
        //     //arrange
        //     var ast = PickupAST();
        //     _mockedPlayerService.Setup(mockedPlayer => mockedPlayer.PickupItem());
        //     //act
        //     _sut.Apply(ast);
        //     //assert
        //     _mockedPlayerService.Verify(mockedPlayer => mockedPlayer.PickupItem(), Times.Once);
        // }
        //
        public static AST PickupAST()
        {
            Input pickup = new Input();
            pickup.AddChild(new Pickup());
            return new AST(pickup);
        }
    
        // [Test]
        // public void Test_Apply_HandleDropActionIsCalled()
        // {
        //     //arrange
        //     var ast = DropAST("item");
        //     _mockedPlayerService.Setup(mockedPlayer => mockedPlayer.DropItem("item"));
        //     //act
        //     _sut.Apply(ast);
        //     //assert
        //     _mockedPlayerService.Verify(mockedPlayer => mockedPlayer.DropItem("item"), Times.Once);
        // }
    
        public static AST DropAST(string itemName)
        {
            Input drop = new Input();
            drop.AddChild(new Drop()
                .AddChild(new Message(itemName)));
            return new AST(drop);
        }
    
        // [Test]
        // public void Test_Apply_HandleAttackActionIsCalled()
        // {
        //     //arrange
        //     string direction = "right";
        //     var ast = AttackAST(direction);
        //     _mockedPlayerService.Setup(mockedPlayer => mockedPlayer.Attack(direction));
        //     //act
        //     _sut.Apply(ast);
        //     //assert
        //     _mockedPlayerService.Verify(mockedPlayer => mockedPlayer.Attack(direction), Times.Once);
        // }
        //
        public static AST AttackAST(string direction)
        {
            Input Attack = new Input();
            Attack.AddChild(new Attack()
                .AddChild(new Direction(direction)));
            return new AST(Attack);
        }
    
        // [Test]
        // public void Test_Apply_HandleSayActionIsCalled()
        // {
        //     //arrange
        //     var ast = SayAST("test");
        //     _mockedPlayerService.Setup(mockedPlayer => mockedPlayer.Say("test"));
        //     //act
        //     _sut.Apply(ast);
        //     //assert
        //     _mockedPlayerService.Verify(mockedPlayer => mockedPlayer.Say("test"), Times.Once);
        // }
        //
        public static AST SayAST(string message)
        {
            Input say = new Input();
            say.AddChild(new Say()
                .AddChild(new Message(message)));
            return new AST(say);
        }
    
        // [Test]
        // public void Test_Apply_HandleShoutActionIsCalled()
        // {
        //     //arrange
        //     var ast = ShoutAST("test");
        //     _mockedPlayerService.Setup(mockedPlayer => mockedPlayer.Shout("test"));
        //     //act
        //     _sut.Apply(ast);
        //     //assert
        //     _mockedPlayerService.Verify(mockedPlayer => mockedPlayer.Shout("test"), Times.Once);
        // }
    
        public static AST ShoutAST(string message)
        {
            Input say = new Input();
            say.AddChild(new Shout()
                .AddChild(new Message(message)));
            return new AST(say);
        }
    
        // [Test]
        // public void Test_Apply_HandleExitActionIsCalled()
        // {
        //     //arrange
        //     var ast = ExitAst();
        //     _mockedPlayerService.Setup(mockedPlayer => mockedPlayer.ExitCurrentGame());
        //     //act
        //     _sut.Apply(ast);
        //     //assert
        //     _mockedPlayerService.Verify(mockedPlayer => mockedPlayer.ExitCurrentGame(), Times.Once);
        // }
    
        public static AST ExitAst()
        {
            Input exit = new Input();
            exit.AddChild(new Exit());
            return new AST(exit);
        }
    
        // [Test]
        // public void Test_Apply_HandlePauseActionIsCalled()
        // {
        //     //arrange
        //     var ast = PauseAst();
        //     _mockedPlayerService.Setup(mockedPlayer => mockedPlayer.Pause());
        //     //act
        //     _sut.Apply(ast);
        //     //assert
        //     _mockedPlayerService.Verify(mockedPlayer => mockedPlayer.Pause(), Times.Once);
        // }
    
        public static AST PauseAst()
        {
            Input pause = new Input();
            pause.AddChild(new Pause());
            return new AST(pause);
        }
    
        // [Test]
        // public void Test_Apply_HandleResumeActionIsCalled()
        // {
        //     //arrange
        //     var ast = ResumeAst();
        //     _mockedPlayerService.Setup(mockedPlayer => mockedPlayer.Resume());
        //     //act
        //     _sut.Apply(ast);
        //     //assert
        //     _mockedPlayerService.Verify(mockedPlayer => mockedPlayer.Resume(), Times.Once);
        // }
    
        public static AST ResumeAst()
        {
            Input resume = new Input();
            resume.AddChild(new Resume());
            return new AST(resume);
        }
    
        // [Test]
        // public void Test_Apply_HandleReplaceActionIsCalled()
        // {
        //     //arrange
        //     var ast = ReplaceAst();
        //     _mockedPlayerService.Setup(mockedPlayer => mockedPlayer.ReplaceByAgent());
        //     //act
        //     _sut.Apply(ast);
        //     //assert
        //     _mockedPlayerService.Verify(mockedPlayer => mockedPlayer.ReplaceByAgent(), Times.Once);
        // }
        
        public static AST ReplaceAst()
        {
            Input replace = new Input();
            replace.AddChild(new Replace());
            return new AST(replace);
        }
    
        // [Test]
        // public void Test_Apply_HandleRequestSessionsActionIsCalled()
        // {
        //     // Arrange
        //     var ast = RequestSessionsAst();
        //
        //     // Act
        //     _sut.Apply(ast);
        //
        //     // Assert
        //     _mockedSessionService.Verify(mockedSession => mockedSession.RequestSessions(), Times.Once);
        // }
    
        private static AST RequestSessionsAst()
        {
            Input requestSessions = new Input();
            requestSessions.AddChild(new RequestSessions());
            return new AST(requestSessions);
        }
    
        // [Test]
        // public void Test_Apply_HandleCreateSessionActionIsCalled()
        // {
        //     // Arrange
        //     const string sessionName = "cool world";
        //     var ast = CreateSessionAst(sessionName);
        //
        //     // Act
        //     _sut.Apply(ast);
        //
        //     // Assert
        //     _mockedSessionService.Verify(mockedSession => mockedSession.CreateSession(sessionName), Times.Once);
        // }
    
        private static AST CreateSessionAst(string sessionName)
        {
            Input createSession = new Input();
            createSession.AddChild(new CreateSession()
                .AddChild(new Message(sessionName)));
            return new AST(createSession);
        }
    
        // [Test]
        // public void Test_Apply_HandleJoinSessionActionIsCalled()
        // {
        //     // Arrange
        //     const string sessionId = "1234-1234";
        //     var ast = JoinSessionAst(sessionId);
        //
        //     // Act
        //     _sut.Apply(ast);
        //
        //     // Assert
        //     _mockedSessionService.Verify(mockedSession => mockedSession.JoinSession(sessionId), Times.Once);
        // }
    
        private static AST JoinSessionAst(string sessionId)
        {
            Input joinSession = new Input();
            joinSession.AddChild(new JoinSession()
                .AddChild(new Message(sessionId)));
            return new AST(joinSession);
        }
    }
}