using Agent.Antlr.Ast;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace Agent.Tests.ast
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class ComparisonTest
    {
        private Comparison _comparison;
        private const string TYPE = "Comparison";

        [SetUp]
        public void Setup()
        {
            _comparison = new Comparison("");
        }

        [Test]
        public void Test_GetNodeType_CorrectOutput()
        {
            //Arrange
            //Act
            var result = _comparison.GetNodeType();
            //Assert
            Assert.AreEqual(TYPE, result);
        }
    }
}