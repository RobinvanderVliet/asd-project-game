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
        private Comparable _comparable;
        
        
     
        [Test]
        public void Test_GetNodeTypeComparable_CorrectOutput()
        {
            //Arrange
            this._comparable = new Comparable();
            //Act
            var result = this._comparable.GetNodeType();
            //Assert
            Assert.AreEqual("Comparable", result);
        }
        
        
    
        [Test]
        public void Test_GetNodeTypeItem_CorrectOutput()
        {
            //Arrange
            this._comparable = new Item("");
            //Act
            var result = this._comparable.GetNodeType();
            //Assert
            Assert.AreEqual("Item", result);
        }
        
    
        [Test]
        public void Test_GetNodeTypeInt_CorrectOutput()
        {
            //Arrange
            this._comparable = new Int(1);
            //Act
            var result = this._comparable.GetNodeType();
            //Assert
            Assert.AreEqual("Int", result);
        }
        
 
        [Test]
        public void Test_GetNodeTypeStat_CorrectOutput()
        {
            //Arrange
            this._comparable = new Stat("");
            //Act
            var result = this._comparable.GetNodeType();
            //Assert
            Assert.AreEqual("Stat", result);
        }
        

        [Test]
        public void Test_GetNodeTypeSubject_CorrectOutput()
        {
            //Arrange
            this._comparable = new Subject("");
            //Act
            var result = this._comparable.GetNodeType();
            //Assert
            Assert.AreEqual("Subject", result);
        }
    }
}