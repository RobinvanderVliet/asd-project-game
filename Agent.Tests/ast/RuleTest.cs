using Agent.antlr.ast;
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
        public void Test_GetNodeType_CorrectOutput()
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
        public void Test_AddChild_Setting()
        {
            //Arrange
            Setting setting = new Setting(null);
            _rule.AddChild(setting);

            //Act


            var result = ( _rule.GetChildren()[0])?.GetNodeType();

            //Assert
            Assert.AreEqual(result, "Setting");
        }

        /*
         * AddChild()
         *
         * Test of de Node toegevoegd wordt aan de Rule
        */
        [Test]
        public void Test_AddChild_Node()
        {
            //Arrange
            var node = new Node();
            _rule.AddChild(node);

            //Act


            var result = ( _rule.GetChildren()[0]).GetNodeType();

            //Assert
            Assert.AreEqual(result, "Node");
        }
        
        /*
         * RemoveChild()
         *
         * Test of de Setting verwijderd wordt van de Rule
         * @author Abdul
        */
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
        
        /*
        * RemoveChild()
         *
         * Test of de node verwijderd wordt van de Rule
        */
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