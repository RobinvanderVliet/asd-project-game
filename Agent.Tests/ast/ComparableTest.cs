using System;
using Agent.antlr.ast;
using Agent.antlr.ast.comparables;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace Agent.Tests.ast
{
    [ExcludeFromCodeCoverage]
    
    [TestFixture]
    public class ComparableTest
    {
        private Comparable comparable;

        [SetUp]
        public void Setup()
        {
            // comparable = new 
        }


        /*
         * GetNodeType() of Item
         *
         * Test If Comparable is a comparable    
        */
        [Test]
        public void Test_GetNodeTypeComparable_CorrectOutput()
        {
            //Arrange
            comparable = new Comparable();
            //Act
            var result = comparable.GetNodeType();
            //Assert
            Assert.AreEqual("Comparable", result);
        }
        
        
        /*
         * GetNodeType() of Item
         *
         * Test If Item is a comparable
         * @author Abdul     
        */
        [Test]
        public void Test_GetNodeTypeItem_CorrectOutput()
        {
            //Arrange
            comparable = new Item("");
            //Act
            var result = comparable.GetNodeType();
            //Assert
            Assert.AreEqual("Item", result);
        }
        
        /*
         * GetNodeTypeInt() of Item
         *
         * Test If Int is a comparable
         * @author Abdul     
        */
        [Test]
        public void Test_GetNodeTypeInt_CorrectOutput()
        {
            //Arrange
            comparable = new Int(1);
            //Act
            var result = comparable.GetNodeType();
            //Assert
            Assert.AreEqual("Int", result);
        }
        
        /*
         * GetNodeTypeOfStat() of Item
         *
         * Test If Stat is a comparable
         * @author Abdul     
        */
        [Test]
        public void Test_GetNodeTypeStat_CorrectOutput()
        {
            //Arrange
            comparable = new Stat("");
            //Act
            var result = comparable.GetNodeType();
            //Assert
            Assert.AreEqual("Stat", result);
        }
        
        /*
        * GetNodeTypeOfSubject() of Item
        *
        * Test If Stat is a comparable
        * @author Abdul     
       */
        [Test]
        public void Test_GetNodeTypeSubject_CorrectOutput()
        {
            //Arrange
            comparable = new Subject("");
            //Act
            var result = comparable.GetNodeType();
            //Assert
            Assert.AreEqual("Subject", result);
        }
    }
}