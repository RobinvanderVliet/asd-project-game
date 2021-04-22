using Chat.antlr.ast;
using Chat.antlr.ast.actions;
using Chat.antlr.transformer;
using NUnit.Framework;
using Player;

namespace Chat.Tests
{
    [TestFixture]
    class EvaluatorTests
    {

        private PlayerModel player;
        private Evaluator evaluator;

        [SetUp]
        public void Setup()
        {
            player = new PlayerModel();
            evaluator = new Evaluator(player);
        }

        [Test]
        public void TestTransformerEvaluatorRunsCorrectly()
        {
            AST ast = MoveAST(1, "up");
            
            try
            {
                evaluator.Apply(ast);
                Assert.IsTrue(true);
            } catch
            {
                Assert.IsTrue(false);
            }
            
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
