using Agent.antlr.ast;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

namespace Agent.Tests.ast
{
    
    [TestFixture]
    public class NodeTest
    {

        private Node _sut;
        private const string Type = "Node";
        
        [SetUp]
        public void Setup()
        {
            this._sut = new Node();
        }
        
        /*
         * GetNodeType()
         *
         * Test of de juiste type terug gegeven wordt
         */
        [Test]
        public void GetNodeTypeTest()
        {
            //Arrange
            
            //Act
            var result = this._sut.GetNodeType();
            //Assert
            Assert.AreEqual(result, Type);
        }
        
        /*
         * GetChildren()
         *
         * Test of een arraylist gegeven wordt
         */
        [Test]
        public void GetChildrenTest()
        {
            //Arrange
            
            //Act
            var result = this._sut.GetChildren();
            //Assert
            Assert.IsInstanceOf(typeof(List<Node>), result);
        }
        
        
    }
}