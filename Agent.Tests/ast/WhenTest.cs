using Agent.antlr.ast.implementation;
using Agent.antlr.ast.implementation.comparables;
using NUnit.Framework;

namespace Agent.Tests.ast
{
    [TestFixture]
    public class WhenTest
    {
        private When when;
        private const string TYPE = "When";

        [SetUp]
        public void Setup()
        {
            when = new When();
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
            var result = when.GetNodeType();
            //Assert
            Assert.AreEqual(TYPE, result);
        }

        /*
         * AddComparableToChild()
         *
         * Test if a comparable is added to child of When
         * @author Abdul     
        */
        [Test]
        public void AddComparableToChild()
        {
            //Arrange
            var comparable = new Item("");
            when.AddChild(comparable);

            //Act
            var result = ((Comparable) when.GetChildren()[0])?.GetNodeType();

            //Assert
            Assert.AreEqual("Comparable", result);
        }
        
        /*
         * AddActionToChild()
         *
         * Test if a Action is added to child of When
         * @author Abdul     
        */
        [Test]
        public void AddActionToChild()
        {
            //Arrange
            var action = new Action("");
            when.AddChild(action);

            //Act
            var result = ((Action) when.GetChildren()[0])?.GetNodeType();

            //Assert
            Assert.AreEqual("Action", result);
        }
        
        /*
         * AddComparisonToChild()
         *
         * Test if a Comparison is added to child of When
         * @author Abdul     
        */
        [Test]
        public void AddComparisonToChild()
        {
            //Arrange
            var comparison = new Comparison("");
            when.AddChild(comparison);

            //Act
            var result = ((Comparison) when.GetChildren()[0])?.GetNodeType();

            //Assert
            Assert.AreEqual("Comparison", result);
        }
    }
}