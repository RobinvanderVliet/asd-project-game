using System;
using System.Diagnostics.CodeAnalysis;
using Agent.Antlr.Ast;
using NUnit.Framework;

namespace Agent.Tests.Generator
{
    [ExcludeFromCodeCoverage]
    public class GeneratorTest
    {

        private Agent.Generator _sut;
        private Fixtures _fix;

        [SetUp]
        public void Setup()
        {
            this._sut = new Agent.Generator();
            this._fix = new Fixtures();
        }

        [Test]
        [TestCase("test1.txt")]
        [TestCase("test2.txt")]
        [TestCase("test3.txt")]
        public void Test_Generator_Level1(String input)
        {
            //Arrange
            AST ast = this._fix.GetFixture(input);

            //Act
            var result = this._sut.Execute(ast);

            //Assert
            Assert.True(true);
        }


    }
}
