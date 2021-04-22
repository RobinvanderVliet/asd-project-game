using Agent.antlr.ast;
using Agent.antlr.ast.comparables;
using NUnit.Framework;

namespace Agent.Tests.ast
{
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
            var result = actionReference.GetNodeType();
            //Assert
            Assert.AreEqual(TYPE, result);
        }
        
        /*
         * AddSubjectToChild()
         *
         * Test if the subject is added to ActionReference
         * @author Abdul     
        */
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
        
        /*
         * AddItemToChild()
         *
         * Test if the item is added to ActionReference
         * @author Abdul     
        */
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
        
        /*
         * AddItemToChild()
         *
         * Test if the item is added to ActionReference
        */
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