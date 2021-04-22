using System;
using Agent.antlr.ast;
using NUnit.Framework;
using Action = Agent.antlr.ast.Action;

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
        public void Test_GetNodeType_CorrectOutput()
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
        public void Test_AddChild_Action()
        {
            //Arrange
            Action action = new Action("Action");
            _setting.AddChild(action);

            //Act

            var result = ( _setting.GetChildren()[0])?.GetNodeType();

            //Assert
            Assert.AreEqual(result, "Action");
        }

        /*
         * AddChild()
         *
         * Test if the condition is added to Setting   
        */
        [Test]
        public void Test_AddChild_Condition()
        {
            //Arrange
            var condition = new Condition();
            _setting.AddChild(condition);

            //Act

            var result = ( _setting.GetChildren()[0]).GetNodeType();

            //Assert
            Assert.AreEqual("Condition", result);
        }
        
        /*
         * AddChild()
         *
         * Test if the node is added to Setting
        */
        [Test]
        public void Test_AddChild_Node()
        {
            //Arrange
            var node = new Node();
            _setting.AddChild(node);

            //Act

            var result = ( _setting.GetChildren()[0]).GetNodeType();

            //Assert
            Assert.AreEqual( "Node", result);
        }
    }
}