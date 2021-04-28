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