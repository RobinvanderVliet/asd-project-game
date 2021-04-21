using Agent.antlr.ast;
using NUnit.Framework;

namespace Agent.Tests.ast
{
    [TestFixture]
    public class ConditionTest
    {
        private Condition condition;
        private const string TYPE = "Condition";

        [SetUp]
        public void Setup()
        {
            condition = new Condition();
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
            var result = condition.GetNodeType();
            //Assert
            Assert.AreEqual(TYPE, result);
        }

        /*
         * AddWhenClauseToChild()
         *
         * Test if the whenclause is added to Settings
         * @author Abdul     
        */
        [Test]
        public void AddWhenClauseToChild()
        {
            //Arrange
            var whenclause = new When();
            condition.AddChild(whenclause);

            //Act
            var result = ((When) condition.GetChildren()[0])?.GetNodeType();

            //Assert
            Assert.AreEqual("When", result);
        }

        /*
         * AddOtherwiseClauseToChild()
         *
         * Test if the otherwiseClause is added to configuration
         * @author Abdul     
        */
        [Test]
        public void AddOtherwiseClauseToChild()
        {
            //Arrange
            var otherwiseClause = new Otherwise();
            condition.AddChild(otherwiseClause);

            //Act
            var result = ((Otherwise) condition.GetChildren()[0])?.GetNodeType();

            //Assert
            Assert.AreEqual("Otherwise", result);
        }
        
        /*
         * RemoveWhenClauseChild()
         *
         * Test if whenclause is deleted from condition
         * @author Abdul
        */
        [Test]
        public void RemoveWhenClauseChild()
        {
            //Arrange
            var whenclause = new When();
            condition.AddChild(whenclause);
            condition.RemoveChild(whenclause);

            //Act
            var result = condition.GetChildren().Count == 0;

            //Assert
            Assert.True(result);
        }

        /*
         * RemoveOtherWiseClauseChild()
         *
         * Test if otherwiseClause is deleted from condition
         * @author Abdul
        */
        [Test]
        public void RemoveOtherWiseClauseChild()
        {
            //Arrange
            var otherWise = new Otherwise();
            condition.AddChild(otherWise);
            condition.RemoveChild(otherWise);

            //Act
            var result = condition.GetChildren().Count == 0;

            //Assert
            Assert.True(result);
        }
    }
}