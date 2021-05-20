using Agent.Antlr.Ast;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace Agent.Tests.ast
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
            this._condition = new Condition();
        }

     
        [Test]
        public void Test_GetNodeType_CorrectOutput()
        {
            //Arrange
            //Act
            var result = this._condition.GetNodeType();
            //Assert
            Assert.AreEqual(TYPE, result);
        }

        [Test]
        public void Test_AddChild_When()
        {
            //Arrange
            var whenclause = new When();
            this._condition.AddChild(whenclause);

            //Act
            var result = ((When) this._condition.GetChildren()[0])?.GetNodeType();

            //Assert
            Assert.AreEqual("When", result);
        }

        
    
        [Test]
        public void Test_AddChild_Otherwise()
        {
            //Arrange
            var otherwiseClause = new Otherwise();
            this._condition.AddChild(otherwiseClause);

            //Act
            var result = ((Otherwise) this._condition.GetChildren()[0])?.GetNodeType();

            //Assert
            Assert.AreEqual("Otherwise", result);
        }
   
        [Test]
        public void Test_AddChild_Node()
        {
            //Arrange
            var node = new Node();
            this._condition.AddChild(node);

            //Act
            var result = ( this._condition.GetChildren()[0]).GetNodeType();

            //Assert
            Assert.AreEqual(result, "Node");
        }
     
        [Test]
        public void Test_Remove_When()
        {
            //Arrange
            var whenclause = new When();
            this._condition.AddChild(whenclause);
            this._condition.RemoveChild(whenclause);

            //Act
            var result = this._condition.GetChildren().Count == 0;

            //Assert
            Assert.True(result);
        }

     
        [Test]
        public void Test_Remove_Otherwise()
        {
            //Arrange
            var otherWise = new Otherwise();
            this._condition.AddChild(otherWise);
            this._condition.RemoveChild(otherWise);

            //Act
            var result = this._condition.GetChildren().Count == 0;

            //Assert
            Assert.True(result);
        }
        
        
        [Test]
        public void Test_Remove_Node()
        {
            //Arrange
            var node = new Node();
            this._condition.AddChild(node);
            this._condition.RemoveChild(node);

            //Act
            var result = this._condition.GetChildren().Count == 0;

            //Assert
            Assert.True(result);
        }
    }
}