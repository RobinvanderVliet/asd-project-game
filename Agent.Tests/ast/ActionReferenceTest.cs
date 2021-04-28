using Agent.antlr.ast;
using Agent.antlr.ast.comparables;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace Agent.Tests.ast
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class ActionReferenceTest
    {
        private ActionReference actionReference;
        private const string TYPE = "ActionReference";

        [SetUp]
        public void Setup()
        {
            actionReference = new ActionReference("");
        }

    
        [Test]
        public void Test_GetNodeType_CorrectOutput()
        {
            //Arrange
            //Act
            var result = actionReference.GetNodeType();
            //Assert
            Assert.AreEqual(TYPE, result);
        }
        
       
        [Test]
        public void Test_AddChild_AddSubjectChild()
        {
            //Arrange
            Subject subject = new Subject("");
            actionReference.AddChild(subject);

            //Act
            var result = ((Subject) actionReference.GetChildren()[0])?.GetNodeType();

            //Assert
            Assert.AreEqual("Subject", result);
        }
        
  
        [Test]
        public void Test_AddChild_AddItemChild()
        {
            //Arrange
            var item = new Item("");
            actionReference.AddChild(item);

            //Act
            var result = ((Item) actionReference.GetChildren()[0])?.GetNodeType();

            //Assert
            Assert.AreEqual("Item", result);
        }
        
   
        [Test]
        public void Test_AddChild_AddNodeChild()
        {
            //Arrange
            var node = new Node();
            actionReference.AddChild(node);

            //Act
            var result = (actionReference.GetChildren()[0])?.GetNodeType();

            //Assert
            Assert.AreEqual("Node", result);
        }
    }
}