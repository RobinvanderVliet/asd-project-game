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
        
        // [Test]
        // public void Test_HandleSayActionIsCalled()
        // {
        //     var ast = Say();
        //     mockedPlayer.Setup(mockedPlayer => mockedPlayer.HandleSayAction("test"));
        //
        //    sut.Apply(ast);
        //     
        //     mockedPlayer.Verify(mockedPlayer => mockedPlayer.HandleSayAction("test"), Times.Once);
        // }
    }
}