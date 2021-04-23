using Agent.antlr.ast;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace Agent.Tests.ast
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class ComparisonTest
    {
        private Comparison comparison;
        private const string TYPE = "Comparison";

        [SetUp]
        public void Setup()
        {
            comparison = new Comparison("");
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
            var result = comparison.GetNodeType();
            //Assert
            Assert.AreEqual(TYPE, result);
        }
    }
}