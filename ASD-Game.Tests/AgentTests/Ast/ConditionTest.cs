using System.Diagnostics.CodeAnalysis;
using ASD_Game.Agent.Antlr.Ast;
using NUnit.Framework;

namespace ASD_Game.Tests.AgentTests.Ast
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class ConditionTest
    {
        private Condition _condition;
        private const string TYPE = "Condition";

        [SetUp]
        public void Setup()
        {
            _condition = new Condition();
        }


        [Test]
        public void Test_GetNodeType_CorrectOutput()
        {
            //Arrange
            //Act
            var result = _condition.GetNodeType();
            //Assert
            Assert.AreEqual(TYPE, result);
        }

        [Test]
        public void Test_AddChild_When()
        {
            //Arrange
            var whenclause = new When();
            _condition.AddChild(whenclause);

            //Act
            var result = ((When)_condition.GetChildren()[0])?.GetNodeType();

            //Assert
            Assert.AreEqual("When", result);
        }



        [Test]
        public void Test_AddChild_Otherwise()
        {
            //Arrange
            var otherwiseClause = new Otherwise();
            _condition.AddChild(otherwiseClause);

            //Act
            var result = ((Otherwise)_condition.GetChildren()[0])?.GetNodeType();

            //Assert
            Assert.AreEqual("Otherwise", result);
        }

        [Test]
        public void Test_AddChild_Node()
        {
            //Arrange
            var node = new Node();
            _condition.AddChild(node);

            //Act
            var result = (_condition.GetChildren()[0]).GetNodeType();

            //Assert
            Assert.AreEqual(result, "Node");
        }

        [Test]
        public void Test_Remove_When()
        {
            //Arrange
            var whenclause = new When();
            _condition.AddChild(whenclause);
            _condition.RemoveChild(whenclause);

            //Act
            var result = _condition.GetChildren().Count == 0;

            //Assert
            Assert.True(result);
        }


        [Test]
        public void Test_Remove_Otherwise()
        {
            //Arrange
            var otherWise = new Otherwise();
            _condition.AddChild(otherWise);
            _condition.RemoveChild(otherWise);

            //Act
            var result = _condition.GetChildren().Count == 0;

            //Assert
            Assert.True(result);
        }


        [Test]
        public void Test_Remove_Node()
        {
            //Arrange
            var node = new Node();
            _condition.AddChild(node);
            _condition.RemoveChild(node);

            //Act
            var result = _condition.GetChildren().Count == 0;

            //Assert
            Assert.True(result);
        }
    }
}