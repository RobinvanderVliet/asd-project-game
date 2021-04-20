
using Agent.antlr.ast.implementation;
using NUnit.Framework;


namespace Agent.Tests.ast
{
    /*
     * 
     * @author Abdul     
    */
    [TestFixture]
    public class OtherwiseTest
    {
        private Otherwise _otherwise;
        private const string TYPE = "Otherwise";

        [SetUp]
        public void Setup()
        {
            _otherwise = new Otherwise();
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
            var result = _otherwise.GetNodeType();
            //Assert
            Assert.AreEqual(result, TYPE);
        }

        /*
         * AddChild()
         *
         * Test if the Actionreference is added to Otherwise
         * @author Abdul     
        */
        [Test]
        public void AddChild()
        {
            //Arrange
            ActionReference actionReference = new ActionReference("action Reference");
            _otherwise.AddChild(actionReference);

            //Act

            var result = ((ActionReference) _otherwise.GetChildren()[0])?.GetNodeType();

            //Assert
            Assert.AreEqual(result, "ActionReference");
        }
    }
}