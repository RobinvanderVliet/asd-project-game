using Agent.antlr.ast.implementation;
using NUnit.Framework;

namespace Agent.Tests.ast
{
    /*
     * 
     * @author Abdul     
    */
    [TestFixture]
    public class RuleTest
    {
        private Rule _rule;
        private const string TYPE = "Rule";

        [SetUp]
        public void Setup()
        {
            _rule = new Rule(null,null);
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
            var result = _rule.GetNodeType();
            //Assert
            Assert.AreEqual(result, TYPE);
        }

        /*
         * AddChild()
         *
         * Test of de Setting toegevoegd wordt aan de Rule
         * @author Abdul     
        */
        [Test]
        public void AddChild()
        {
            //Arrange
            Setting setting = new Setting(null);
            _rule.AddChild(setting);

            //Act


            var result = ((Setting) _rule.GetChildren()[0])?.GetNodeType();

            //Assert
            Assert.AreEqual(result, "Setting");
        }

        /*
         * RemoveChild()
         *
         * Test of de Setting verwijderd wordt van de Rule
         * @author Abdul
        */
        [Test]
        public void RemoveChild()
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
    }
}