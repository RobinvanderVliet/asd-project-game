using System;
using Agent.antlr.ast.implementation;
using NUnit.Framework;
using Action = Agent.antlr.ast.implementation.Action;

namespace Agent.Tests.ast
{
    /*
     * 
     * @author Abdul     
    */
    [TestFixture]
    public class SettingTest
    {
        private Setting _setting;
        private const string TYPE = "Setting";

        [SetUp]
        public void Setup()
        {
            _setting = new Setting("Setting");
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
            var result = _setting.GetNodeType();
            //Assert
            Assert.AreEqual(result, TYPE);
        }

        /*
         * AddChild()
         *
         * Test if the action is added to Setting
         * @author Abdul     
        */
        [Test]
        public void AddChild()
        {
            //Arrange
            Action action = new Action("Action");
            _setting.AddChild(action);

            //Act

            var result = ((Action) _setting.GetChildren()[0])?.GetNodeType();

            //Assert
            Assert.AreEqual(result, "Action");
        }

          }
}