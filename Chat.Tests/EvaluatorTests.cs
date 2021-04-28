using Chat.antlr.ast;
using Chat.antlr.ast.actions;
using Chat.antlr.transformer;
using Moq;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using Player.Model;

namespace Chat.Tests
{
    [ExcludeFromCodeCoverage]
    class EvaluatorTests
    {
        private Evaluator sut;
        private Mock<IPlayerModel> mockedPlayer;


        [SetUp]
        public void Setup()
        {
            mockedPlayer = new Mock<IPlayerModel>();
            sut = new Evaluator(mockedPlayer.Object);
        }

        [Test]
        public void Test_HandleDirection_RunsCorrectly()
        {
            var ast = MoveAST(1, "up");

            mockedPlayer.Setup(x => x.HandleDirection("up", 1));
            sut.Apply(ast);
            mockedPlayer.VerifyAll();
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
            mockedPlayer.Setup(mockedPlayer => mockedPlayer.PickupItem());

            sut.Apply(ast);

            mockedPlayer.Verify(mockedPlayer => mockedPlayer.PickupItem(), Times.Once);
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
            mockedPlayer.Setup(mockedPlayer => mockedPlayer.DropItem("item"));

            sut.Apply(ast);

            mockedPlayer.Verify(mockedPlayer => mockedPlayer.DropItem("item"), Times.Once);
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
            var ast = AttackAST();
            mockedPlayer.Setup(mockedPlayer => mockedPlayer. );

            sut.Apply(ast);

            mockedPlayer.Verify(mockedPlayer => mockedPlayer. , Times.Once);
        }

        public static AST AttackAST()
        {
            Input Attack = new Input();
            Attack.AddChild(new Attack());
            return new AST(Attack);
        }
        
        [Test]
        public void Test_HandleSayActionIsCalled()
        {
            var ast = SayAST("test");
            mockedPlayer.Setup(mockedPlayer => mockedPlayer.HandleSayAction("test"));

            sut.Apply(ast);

            mockedPlayer.Verify(mockedPlayer => mockedPlayer.HandleSayAction("test"), Times.Once);
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
            mockedPlayer.Setup(mockedPlayer => mockedPlayer.HandleShoutAction("test"));

            sut.Apply(ast);

            mockedPlayer.Verify(mockedPlayer => mockedPlayer.HandleShoutAction("test"), Times.Once);
        }

        public static AST ShoutAST(string message)
        {
            Input say = new Input();
            say.AddChild(new Shout()
                .AddChild(new Message(message)));
            return new AST(say);
        }
    }
}