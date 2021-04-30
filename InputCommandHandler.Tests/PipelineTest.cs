using System.Diagnostics.CodeAnalysis;
using InputCommandHandler.Antlrr;
using InputCommandHandler.Antlr.Ast;
using InputCommandHandler.Antlr.Ast.Actions;
using InputCommandHandler.Antlr.Transformer;
using InputCommandHandler.Exceptions;
using Moq;
using NUnit.Framework;
using Player.Model;

namespace InputCommandHandler.Tests
{
    [ExcludeFromCodeCoverage]
    public class PipelineTest
    {
        private Mock<IPlayerModel> mockedPlayer;
        private Mock<Evaluator> mockedEvaluator;
        private Pipeline sut;

        [SetUp]
        public void SetUp()
        {
            sut = new Pipeline();
        }

        [Test]
        public void Test_ParseCommand_ThrowsSyntaxErrorWhenCommandNotRecognised()
        {
            Assert.Throws<CommandSyntaxException>(() => sut.ParseCommand("me forward"));
        }

        [Test]
        public void Test_ParseCommand_ParsingACommandWorksAsExpected()
        {
            sut.ParseCommand("move forward");
            AST ast = sut.Ast;
            AST exp = MoveCommand(1, "forward");

            Assert.AreEqual(exp, ast);
        }

        public static AST MoveCommand(int steps, string direction)
        {
            Input moveForward = new Input();


            moveForward.AddChild(new Move()
                .AddChild(new Direction(direction))
                .AddChild(new Step(steps)));

            return new AST(moveForward);
        }
    }
}