using Agent.Antlr.Ast;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace Agent.Tests.Ast
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class ASTTest
    {


        [Test]
        public void Test_Constructor_NoParameters()
        {
            //Arrange
            //Act
            var result = new AST();
            //Assert
            Assert.IsInstanceOf(typeof(Configuration), result.Root);
        }

        [Test]
        public void Test_Constructor_WithParameters()
        {
            //Arrange
            var configuration = new Configuration();
            //Act
            var result = new AST(configuration);
            //Assert
            Assert.AreEqual(configuration,result.Root);
        }

    }
}