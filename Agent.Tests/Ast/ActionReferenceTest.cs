using System.Diagnostics.CodeAnalysis;
using Agent.Antlr.Ast;
using Agent.Antlr.Ast.Comparables;
using NUnit.Framework;

namespace Agent.Tests.Ast
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class ActionReferenceTest
    {
        private ActionReference _actionReference;
        private const string TYPE = "ActionReference";

        [SetUp]
        public void Setup()
        {
            this._actionReference = new ActionReference("");
        }

    
        [Test]
        public void Test_GetNodeType_CorrectOutput()
        {
            //Arrange
            //Act
            var result = this._actionReference.GetNodeType();
            //Assert
            Assert.AreEqual(TYPE, result);
        }
        
       
        [Test]
        public void Test_AddChild_AddSubjectChild()
        {
            //Arrange
            Subject subject = new Subject("");
            this._actionReference.AddChild(subject);

            //Act
            var result = ((Subject) this._actionReference.GetChildren()[0])?.GetNodeType();

            //Assert
            Assert.AreEqual("Subject", result);
        }
        
  
        [Test]
        public void Test_AddChild_AddItemChild()
        {
            //Arrange
            var item = new Item("");
            this._actionReference.AddChild(item);

            //Act
            var result = ((Item) this._actionReference.GetChildren()[0])?.GetNodeType();

            //Assert
            Assert.AreEqual("Item", result);
        }
        
   
        [Test]
        public void Test_AddChild_AddNodeChild()
        {
            //Arrange
            var node = new Node();
            this._actionReference.AddChild(node);

            //Act
            var result = (this._actionReference.GetChildren()[0])?.GetNodeType();

            //Assert
            Assert.AreEqual("Node", result);
        }
    }
}