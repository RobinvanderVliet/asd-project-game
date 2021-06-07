using System.Diagnostics.CodeAnalysis;
using ASD_Game.InputHandling.Antlr;
using ASD_Game.InputHandling.Antlr.Ast;
using ASD_Game.InputHandling.Antlr.Ast.Actions;
using ASD_Game.InputHandling.Antlr.Transformer;
using ASD_Game.InputHandling.Exceptions;
using Moq;
using NUnit.Framework;

namespace ASD_Game.Tests.InputHandlingTests
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