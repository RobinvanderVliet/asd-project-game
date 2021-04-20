using Agent.antlr.ast.implementation;
using Agent.antlr.ast.implementation.comparables;
using Agent.antlr.ast.interfaces.comparables;
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
        public void GetNodeType()
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
        public void AddSubjectToChild()
        {
            //Arrange
            ISubject subject = new Subject("");
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
        public void AddItemToChild()
        {
            //Arrange
            var item = new Item("");
            actionReference.AddChild(item);

            //Act
            var result = ((Item) actionReference.GetChildren()[0])?.GetNodeType();

            //Assert
            Assert.AreEqual("Item", result);
        }
    }
}