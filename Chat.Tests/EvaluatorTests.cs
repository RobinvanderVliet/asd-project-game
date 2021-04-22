using Chat.antlr.ast;
using Chat.antlr.ast.actions;
using Chat.antlr.transformer;
using Moq;
using NUnit.Framework;
using Player;

namespace Chat.Tests
{
    [TestFixture]
    class EvaluatorTests
    {
        private Evaluator sut;
        private Mock<IPlayerModel> mockedPlayerModel;


        [SetUp]
        public void Setup()
        {
            mockedPlayerModel = new Mock<IPlayerModel>();
            sut = new Evaluator(mockedPlayerModel.Object);
        }

        [Test]
        public void TestTransformerEvaluatorRunsCorrectly()
        {
            var ast = MoveAST(1, "up");

            mockedPlayerModel.Setup(x => x.HandleDirection("up", 1));
            sut.Apply(ast);
            mockedPlayerModel.VerifyAll();
        }

        public static AST MoveAST(int steps, string direction)
        {
            Input move = new Input();
            move.AddChild(new Move()
                .AddChild(new Direction(direction))
                .AddChild(new Step(steps)));
            return new AST(move);
        }
    }
}