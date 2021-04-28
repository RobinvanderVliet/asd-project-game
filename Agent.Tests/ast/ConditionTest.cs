using Agent.antlr.ast;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace Agent.Tests.ast
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class ConditionTest
    {
        private Condition condition;
        private const string TYPE = "Condition";

        [SetUp]
        public void Setup()
        {
            condition = new Condition();
        }

     
        [Test]
        public void Test_GetNodeType_CorrectOutput()
        {
            //Arrange
            //Act
            var result = condition.GetNodeType();
            //Assert
            Assert.AreEqual(TYPE, result);
        }

        [Test]
        public void Test_AddChild_When()
        {
            //Arrange
            var whenclause = new When();
            condition.AddChild(whenclause);

            //Act
            var result = ((When) condition.GetChildren()[0])?.GetNodeType();

            //Assert
            Assert.AreEqual("When", result);
        }

        
    
        [Test]
        public void Test_AddChild_Otherwise()
        {
            //Arrange
            var otherwiseClause = new Otherwise();
            condition.AddChild(otherwiseClause);

            //Act
            var result = ((Otherwise) condition.GetChildren()[0])?.GetNodeType();

            //Assert
            Assert.AreEqual("Otherwise", result);
        }
   
        [Test]
        public void Test_AddChild_Node()
        {
            //Arrange
            var node = new Node();
            condition.AddChild(node);

            //Act
            var result = ( condition.GetChildren()[0]).GetNodeType();

            //Assert
            Assert.AreEqual(result, "Node");
        }
     
        [Test]
        public void Test_Remove_When()
        {
            //Arrange
            var whenclause = new When();
            condition.AddChild(whenclause);
            condition.RemoveChild(whenclause);

            //Act
            var result = condition.GetChildren().Count == 0;

            //Assert
            Assert.True(result);
        }

     
        [Test]
        public void Test_Remove_Otherwise()
        {
            //Arrange
            var otherWise = new Otherwise();
            condition.AddChild(otherWise);
            condition.RemoveChild(otherWise);

            //Act
            var result = condition.GetChildren().Count == 0;

            //Assert
            Assert.True(result);
        }
        
        
        [Test]
        public void Test_Remove_Node()
        {
            //Arrange
            var node = new Node();
            condition.AddChild(node);
            condition.RemoveChild(node);

            //Act
            var result = condition.GetChildren().Count == 0;

            //Assert
            Assert.True(result);
        }
    }
}