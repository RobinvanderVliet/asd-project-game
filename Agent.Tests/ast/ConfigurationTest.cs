using Agent.antlr.ast;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace Agent.Tests.ast
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
            var result = _config.GetNodeType();
            //Assert
            Assert.AreEqual(result, TYPE);
        }

        /*
         * AddChild()
         *
         * Test of de Rule toegevoegd wordt aan de Configuration
         * @author Abdul     
        */
        [Test]
        public void Test_AddChild()
        {
            //Arrange
            Rule rule = new Rule(null,null);
            _config.AddChild(rule);

            //Act
            var result = ((Rule) _config.GetChildren()[0])?.GetNodeType();

            //Assert
            Assert.AreEqual(result, "Rule");
        }

        /*
         * RemoveChild()
         *
         * Test of de Rule is verwijderd van de Node
         * @author Abdul
        */
        [Test]
        public void Test_RemoveChild()
        {
            //Arrange
            Rule rule = new Rule(null,null);
            _config.AddChild(rule);
            _config.RemoveChild(rule);

            //Act
            var result = _config.GetChildren().Count == 0;

            //Assert
            Assert.True(result);
        }
    }
}