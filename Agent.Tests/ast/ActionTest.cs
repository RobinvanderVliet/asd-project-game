using System;
using Agent.antlr.ast.implementation;
using NUnit.Framework;
using Action = Agent.antlr.ast.implementation.Action;

namespace Agent.Tests.ast
{
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

        /*
         * GetNodeType()
         *
         * Test of de juiste type terug gegeven wordt
         * @author Abdul     
        */
        [Test]
        public void GetNodeType()
        {
            //Arrange
            //Act
            var result = action.GetNodeType();
            //Assert
            Assert.AreEqual(TYPE, result);
        }

        /*
         * AddConditionToChild()
         *
         * Test if the condition is added to Action
         * @author Abdul     
        */
        [Test]
        public void AddConditionToChild()
        {
            //Arrange
            var condition = new Condition();
            action.AddChild(condition);

            //Act
            var result = ((Condition) action.GetChildren()[0])?.GetNodeType();

            //Assert
            Assert.AreEqual("Condition", result);
        }
        
        /*
         * RemoveConditionFromChild()
         *
         * Test if condition is deleted from action
         * @author Abdul
        */
        [Test]
        public void RemoveConditionFromChild()
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
    }
}