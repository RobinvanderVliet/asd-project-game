using System.Diagnostics.CodeAnalysis;
using ASD_Game.Agent.Antlr.Ast;
using NUnit.Framework;

namespace ASD_Game.Tests.AgentTests.Ast
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class ConfigurationTest
    {
        private Configuration _config;
        private const string TYPE = "Configuration";

        [SetUp]
        public void Setup()
        {
            _config = new Configuration();
        }


        [Test]
        public void Test_GetNodeType_CorrectOutput()
        {
            //Arrange
            //Act
            var result = _config.GetNodeType();
            //Assert
            Assert.AreEqual(result, TYPE);
        }


        [Test]
        public void Test_AddChild()
        {
            //Arrange
            Rule rule = new Rule(null, null);
            _config.AddChild(rule);

            //Act
            var result = ((Rule)_config.GetChildren()[0])?.GetNodeType();

            //Assert
            Assert.AreEqual(result, "Rule");
        }


        [Test]
        public void Test_RemoveChild()
        {
            //Arrange
            Rule rule = new Rule(null, null);
            _config.AddChild(rule);
            _config.RemoveChild(rule);

            //Act
            var result = _config.GetChildren().Count == 0;

            //Assert
            Assert.True(result);
        }
    }
}