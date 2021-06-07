using System.Diagnostics.CodeAnalysis;
using ASD_Game.Agent.Antlr.Ast;
using NUnit.Framework;
using Action = ASD_Game.Agent.Antlr.Ast.Action;

namespace ASD_Game.Tests.AgentTests.Ast
{
    [ExcludeFromCodeCoverage]
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


        [Test]
        public void Test_GetNodeType_CorrectOutput()
        {
            //Arrange
            //Act
            var result = _setting.GetNodeType();
            //Assert
            Assert.AreEqual(result, TYPE);
        }


        [Test]
        public void Test_AddChild_Action()
        {
            //Arrange
            Action action = new Action("Action");
            _setting.AddChild(action);

            //Act

            var result = (_setting.GetChildren()[0])?.GetNodeType();

            //Assert
            Assert.AreEqual(result, "Action");
        }


        [Test]
        public void Test_AddChild_Condition()
        {
            //Arrange
            var condition = new Condition();
            _setting.AddChild(condition);

            //Act

            var result = (_setting.GetChildren()[0]).GetNodeType();

            //Assert
            Assert.AreEqual("Condition", result);
        }

        [Test]
        public void Test_AddChild_Node()
        {
            //Arrange
            var node = new Node();
            _setting.AddChild(node);

            //Act

            var result = (_setting.GetChildren()[0]).GetNodeType();

            //Assert
            Assert.AreEqual("Node", result);
        }
    }
}