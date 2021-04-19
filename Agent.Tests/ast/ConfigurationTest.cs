using Agent.antlr.ast;
using NUnit.Framework;

namespace Agent.Tests.ast
{
    /*
     * 
     * @author Abdul     
    */
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
        public void GetNodeType()
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
        public void AddChild()
        {
            //Arrange
            Rule rule = new Rule();
            _config.AddChild(rule);

            //Act
            var result = ((Rule) _config.GetChildren()[0])?.GetNodeType();

            //Assert
            Assert.AreEqual(result, "Rule");
        }

        /*
         * AddChild()
         *
         * Test of de Rule is verwijderd van de Node
         * @author Abdul
        */
        [Test]
        public void RemoveChild()
        {
            //Arrange
            Rule rule = new Rule();
            _config.AddChild(rule);
            _config.RemoveChild(rule);

            //Act
            var result = _config.GetChildren().Count == 0;

            //Assert
            Assert.True(result);
        }
    }
}