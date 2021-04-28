using System;
using Agent.antlr.ast;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using Action = Agent.antlr.ast.Action;

namespace Agent.Tests.ast
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class ActionTest
    {
        private Action action;
        private const string TYPE = "Action";

        [SetUp]
        public void Setup()
        {
            action = new Action("");
        }


        [Test]
        public void Test_GetNodeType_CorrectOutput()
        {
            //Arrange
            //Act
            var result = action.GetNodeType();
            //Assert
            Assert.AreEqual(TYPE, result);
        }


        [Test]
        public void Test_AddChild_AddConditionChild()
        {
            //Arrange
            var condition = new Condition();
            action.AddChild(condition);

            //Act
            var result = (action.GetChildren()[0])?.GetNodeType();

            //Assert
            Assert.AreEqual("Condition", result);
        }


        [Test]
        public void Test_AddChild_AddNodeChild()
        {
            //Arrange
            var node = new Node();
            action.AddChild(node);

            //Act
            var result = (action.GetChildren()[0])?.GetNodeType();

            //Assert
            Assert.AreEqual("Node", result);
        }


        [Test]
        public void Test_RemoveChild_RemoveConditionChild()
        {
            //Arrange
            var condition = new Condition();
            action.AddChild(condition);
            action.RemoveChild(condition);

            //Act
            var result = action.GetChildren().Count == 0;

            //Assert
            Assert.True(result);
        }

   
        [Test]
        public void Test_RemoveChild_RemoveNodeChild()
        {
            //Arrange
            var node = new Node();
            action.AddChild(node);
            action.RemoveChild(node);

            //Act
            var result = action.GetChildren().Count == 0;

            //Assert
            Assert.True(result);
        }
    }
}