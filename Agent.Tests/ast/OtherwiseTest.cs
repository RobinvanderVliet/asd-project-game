
using Agent.antlr.ast;
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
        public void Test_GetNodeType_CorrectOutput()
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
        public void Test_AddChild_ActionReference()
        {
            //Arrange
            ActionReference actionReference = new ActionReference("action Reference");
            _otherwise.AddChild(actionReference);

            //Act

            var result = ( _otherwise.GetChildren()[0]).GetNodeType();

            //Assert
            Assert.AreEqual(result, "ActionReference");
        }
        
        /*
          * AddChild()
          *
          * Test if the node is added to Otherwise   
         */
        [Test]
        public void Test_AddChild_Node()
        {
            //Arrange
            var node = new Node();
            _otherwise.AddChild(node);

            //Act

            var result = ( _otherwise.GetChildren()[0]).GetNodeType();

            //Assert
            Assert.AreEqual("Node", result);
        }
    }
}