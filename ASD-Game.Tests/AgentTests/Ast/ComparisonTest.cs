using System.Diagnostics.CodeAnalysis;
using Agent.Antlr.Ast;
using NUnit.Framework;

namespace Agent.Tests.Ast
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