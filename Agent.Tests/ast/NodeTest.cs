using Agent.antlr.ast;
using NUnit.Framework;

namespace Agent.Tests.ast
{
    
    [TestFixture]
    public class NodeTest
    {

        private INode sut;
        private const string TYPE = "Node";
        
        [SetUp]
        public void Setup()
        {
            sut = new Node();
        }
        
        /*
         * GetNodeType()
         *
         * Test of de juiste type terug gegeven wordt
         */
        [Test]
        public void GetNodeType()
        {
            //Arrange
            
            //Act
            var result = this.sut.GetNodeType();
            //Assert
            Assert.AreEqual(result, TYPE);
        }
        
    }
}