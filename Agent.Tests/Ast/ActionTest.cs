using Agent.Antlr.Ast;
using System;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using Action = Agent.Antlr.Ast.Action;

namespace Agent.Tests.ast
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class ActionTest
    {
        private Action _action;
        private const string TYPE = "Action";

        [SetUp]
        public void Setup()
        {
            this._action = new Action("");
        }


        [Test]
        public void Test_GetNodeType_CorrectOutput()
        {
            //Arrange
            //Act
            var result = this._action.GetNodeType();
            //Assert
            Assert.AreEqual(TYPE, result);
        }


        [Test]
        public void Test_AddChild_AddConditionChild()
        {
            //Arrange
            var condition = new Condition();
            this._action.AddChild(condition);

            //Act
            var result = (this._action.GetChildren()[0])?.GetNodeType();

            //Assert
            Assert.AreEqual("Condition", result);
        }


        [Test]
        public void Test_AddChild_AddNodeChild()
        {
            //Arrange
            var node = new Node();
            this._action.AddChild(node);

            //Act
            var result = (this._action.GetChildren()[0])?.GetNodeType();

            //Assert
            Assert.AreEqual("Node", result);
        }


        [Test]
        public void Test_RemoveChild_RemoveConditionChild()
        {
            //Arrange
            var condition = new Condition();
            this._action.AddChild(condition);
            this._action.RemoveChild(condition);

            //Act
            var result = this._action.GetChildren().Count == 0;

            //Assert
            Assert.True(result);
        }

   
        [Test]
        public void Test_RemoveChild_RemoveNodeChild()
        {
            //Arrange
            var node = new Node();
            this._action.AddChild(node);
            this._action.RemoveChild(node);

            //Act
            var result = this._action.GetChildren().Count == 0;

            //Assert
            Assert.True(result);
        }
    }
}