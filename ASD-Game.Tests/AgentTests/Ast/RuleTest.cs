using System.Diagnostics.CodeAnalysis;
using ASD_Game.Agent.Antlr.Ast;
using NUnit.Framework;

namespace ASD_Game.Tests.AgentTests.Ast
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class RuleTest
    {
        private Rule _rule;
        private const string TYPE = "Rule";

        [SetUp]
        public void Setup()
        {
            _rule = new Rule(null, null);
        }


        [Test]
        public void Test_GetNodeType_CorrectOutput()
        {
            //Arrange
            //Act
            var result = _rule.GetNodeType();
            //Assert
            Assert.AreEqual(result, TYPE);
        }


        [Test]
        public void Test_AddChild_Setting()
        {
            //Arrange
            Setting setting = new Setting(null);
            _rule.AddChild(setting);

            //Act

            var result = (_rule.GetChildren()[0])?.GetNodeType();

            //Assert
            Assert.AreEqual(result, "Setting");
        }

        [Test]
        public void Test_AddChild_Node()
        {
            //Arrange
            var node = new Node();
            _rule.AddChild(node);

            //Act


            var result = (_rule.GetChildren()[0]).GetNodeType();

            //Assert
            Assert.AreEqual(result, "Node");
        }


        [Test]
        public void Test_RemoveChild_Setting()
        {
            //Arrange
            Setting setting = new Setting(null);
            _rule.AddChild(setting);
            _rule.RemoveChild(setting);


            //Act
            var result = _rule.GetChildren().Count == 0;

            //Assert
            Assert.True(result);
        }


        [Test]
        public void Test_RemoveChild_Node()
        {
            //Arrange
            var node = new Node();
            _rule.AddChild(node);
            _rule.RemoveChild(node);


            //Act
            var result = _rule.GetChildren().Count == 0;

            //Assert
            Assert.True(result);
        }
    }
}