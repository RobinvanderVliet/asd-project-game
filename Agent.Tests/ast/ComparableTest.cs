using System;
using Agent.antlr.ast.implementation;
using Agent.antlr.ast.implementation.comparables;
using NUnit.Framework;

namespace Agent.Tests.ast
{
    
    [TestFixture]
    public class ComparableTest
    {
        private Comparable comparable;
        private const string TYPE = "Comparable";

        [SetUp]
        public void Setup()
        {
            // comparable = new 
        }

        /*
         * GetNodeType() of Item
         *
         * Test If Item is a comparable
         * @author Abdul     
        */
        [Test]
        public void GetNodeTypeOfItem()
        {
            //Arrange
            comparable = new Item("");
            //Act
            var result = comparable.GetNodeType();
            //Assert
            Assert.AreEqual(TYPE, result);
        }
        
        /*
         * GetNodeTypeInt() of Item
         *
         * Test If Int is a comparable
         * @author Abdul     
        */
        [Test]
        public void GetNodeTypeOfInt()
        {
            //Arrange
            comparable = new Int(1);
            //Act
            var result = comparable.GetNodeType();
            //Assert
            Assert.AreEqual(TYPE, result);
        }
        
        /*
         * GetNodeTypeOfStat() of Item
         *
         * Test If Stat is a comparable
         * @author Abdul     
        */
        [Test]
        public void GetNodeTypeOfStat()
        {
            //Arrange
            comparable = new Stat("");
            //Act
            var result = comparable.GetNodeType();
            //Assert
            Assert.AreEqual(TYPE, result);
        }
        
        /*
        * GetNodeTypeOfSubject() of Item
        *
        * Test If Stat is a comparable
        * @author Abdul     
       */
        [Test]
        public void GetNodeTypeOfSubject()
        {
            //Arrange
            comparable = new Subject("");
            //Act
            var result = comparable.GetNodeType();
            //Assert
            Assert.AreEqual(TYPE, result);
        }
    }
}