using Chat.antlr.ast;
using Chat.antlr.ast.actions;
using Chat.antlr.transformer;
using Moq;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using Chat.exception;
using Player.Model;
using Player.Services;

namespace Chat.Tests
{
    [ExcludeFromCodeCoverage]
    class EvaluatorTests
    {
        private Evaluator sut;
        private Mock<IPlayerService> _mockedPlayerService;


        [SetUp]
        public void Setup()
        {
            _mockedPlayerService = new Mock<IPlayerService>();
            sut = new Evaluator(_mockedPlayerService.Object);
        }

        [Test]
        public void Test_HandleDirection_RunsCorrectly()
        {
            var ast = MoveAST(1, "up");

            _mockedPlayerService.Setup(x => x.HandleDirection("up", 1));
            sut.Apply(ast);
            _mockedPlayerService.VerifyAll();
        }
        
        [Test]
        public void Test_HandleDirection_ThrowsExceptionWithStepsLessThan1()
        {
            var ast = MoveAST(0, "up");
            
            Assert.Throws<MoveException>(() => sut.Apply(ast));
        }
        
        [Test]
        public void Test_HandleDirection_ThrowsExceptionWithStepsMoreThan10()
        {
            var ast = MoveAST(11, "up");
            
            Assert.Throws<MoveException>(() => sut.Apply(ast));
        }

        public static AST MoveAST(int steps, string direction)
        {
            Input move = new Input();
            move.AddChild(new Move()
                .AddChild(new Direction(direction))
                .AddChild(new Step(steps)));
            return new AST(move);
        }

        [Test]
        public void Test_HandlePickupActionIsCalled()
        {
            var ast = PickupAST();
            _mockedPlayerService.Setup(mockedPlayer => mockedPlayer.PickupItem());

            sut.Apply(ast);

            _mockedPlayerService.Verify(mockedPlayer => mockedPlayer.PickupItem(), Times.Once);
        }

        public static AST PickupAST()
        {
            Input pickup = new Input();
            pickup.AddChild(new Pickup());
            return new AST(pickup);
        }
        
        [Test]
        public void Test_HandleDropActionIsCalled()
        {
            var ast = DropAST("item");
            _mockedPlayerService.Setup(mockedPlayer => mockedPlayer.DropItem("item"));

            sut.Apply(ast);

            _mockedPlayerService.Verify(mockedPlayer => mockedPlayer.DropItem("item"), Times.Once);
        }

        public static AST DropAST(string itemName)
        {
            Input drop = new Input();
            drop.AddChild(new Drop()
                .AddChild(new Message(itemName)));
            return new AST(drop);
        }
        
        [Test]
        public void Test_HandleAttackActionIsCalled()
        {
            string direction = "right";
            var ast = AttackAST(direction);
            _mockedPlayerService.Setup(mockedPlayer => mockedPlayer.Attack(direction) );

            sut.Apply(ast);

            _mockedPlayerService.Verify(mockedPlayer => mockedPlayer.Attack(direction) , Times.Once);
        }

        public static AST AttackAST(string direction)
        {
            Input Attack = new Input();
            Attack.AddChild(new Attack()
                .AddChild(new Direction(direction)));
            return new AST(Attack);
        }
        
        [Test]
        public void Test_HandleSayActionIsCalled()
        {
            var ast = SayAST("test");
            _mockedPlayerService.Setup(mockedPlayer => mockedPlayer.Say("test"));

            sut.Apply(ast);

            _mockedPlayerService.Verify(mockedPlayer => mockedPlayer.Say("test"), Times.Once);
        }

        public static AST SayAST(string message)
        {
            Input say = new Input();
            say.AddChild(new Say()
                .AddChild(new Message(message)));
            return new AST(say);
        }
        
        [Test]
        public void Test_HandleShoutActionIsCalled()
        {
            var ast = ShoutAST("test");
            _mockedPlayerService.Setup(mockedPlayer => mockedPlayer.Shout("test"));

            sut.Apply(ast);

            _mockedPlayerService.Verify(mockedPlayer => mockedPlayer.Shout("test"), Times.Once);
        }

        public static AST ShoutAST(string message)
        {
            Input say = new Input();
            say.AddChild(new Shout()
                .AddChild(new Message(message)));
            return new AST(say);
        }
        
        [Test]
        public void Test_Apply_HandleExitActionIsCalled()
        {
            var ast = ExitAst();
            _mockedPlayerService.Setup(mockedPlayer => mockedPlayer.ExitCurrentGame());

            sut.Apply(ast);

            _mockedPlayerService.Verify(mockedPlayer => mockedPlayer.ExitCurrentGame(), Times.Once);
        }

        public static AST ExitAst()
        {
            Input exit = new Input();
            exit.AddChild(new Exit());
            return new AST(exit);
        }
        
        [Test]
        public void Test_Apply_HandlePauseActionIsCalled()
        {
            var ast = PauseAst();
            _mockedPlayerService.Setup(mockedPlayer => mockedPlayer.Pause());

            sut.Apply(ast);

            _mockedPlayerService.Verify(mockedPlayer => mockedPlayer.Pause(), Times.Once);
        }

        public static AST PauseAst()
        {
            Input pause = new Input();
            pause.AddChild(new Pause());
            return new AST(pause);
        }
        
        [Test]
        public void Test_Apply_HandleResumeActionIsCalled()
        {
            var ast = ResumeAst();
            _mockedPlayerService.Setup(mockedPlayer => mockedPlayer.Resume());

            sut.Apply(ast);

            _mockedPlayerService.Verify(mockedPlayer => mockedPlayer.Resume(), Times.Once);
        }

        public static AST ResumeAst()
        {
            Input resume = new Input();
            resume.AddChild(new Resume());
            return new AST(resume);
        }
        
        [Test]
        public void Test_Apply_HandleReplaceActionIsCalled()
        {
            var ast = ReplaceAst();
            _mockedPlayerService.Setup(mockedPlayer => mockedPlayer.ReplaceByAgent());

            sut.Apply(ast);

            _mockedPlayerService.Verify(mockedPlayer => mockedPlayer.ReplaceByAgent(), Times.Once);
        }

        public static AST ReplaceAst()
        {
            Input replace = new Input();
            replace.AddChild(new Replace());
            return new AST(replace);
        }
    }
}