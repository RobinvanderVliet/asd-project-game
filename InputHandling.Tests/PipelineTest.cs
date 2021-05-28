using System.Diagnostics.CodeAnalysis;
using InputHandling.Antlr;
using InputHandling.Antlr.Ast;
using InputHandling.Antlr.Ast.Actions;
using InputHandling.Antlr.Transformer;
using InputHandling.Exceptions;
using Moq;
using NUnit.Framework;

namespace InputHandling.Tests
{
    [ExcludeFromCodeCoverage]
    public class PipelineTest
    {
        private Mock<IEvaluator> _mockedEvaluator;
        private Pipeline _sut;

        [SetUp]
        public void SetUp()
        {
            _mockedEvaluator = new Mock<IEvaluator>();
            _sut = new Pipeline(_mockedEvaluator.Object);
        }

        [Test]
        public void Test_ParseCommand_ThrowsSyntaxErrorWhenCommandNotRecognised()
        {
            //act
            //arrange&assert
            Assert.Throws<CommandSyntaxException>(() => _sut.ParseCommand("me forward"));
        }

        [Test]
        public void Test_ParseCommand_ParsingACommandWorksAsExpected()
        {
            //act
            _sut.ParseCommand("move forward");
            AST ast = _sut.Ast;
            //arrange
            AST exp = MoveCommand(1, "forward");
            //assert
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