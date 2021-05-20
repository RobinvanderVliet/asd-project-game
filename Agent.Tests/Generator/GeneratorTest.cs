using Agent.Antlr.Ast;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using Moq;

namespace Agent.Tests.generator
{
    [ExcludeFromCodeCoverage]
    public class GeneratorTest
    {

        private Generator _sut;
        private Fixtures _fix;

        [SetUp]
        public void Setup()
        {
            _sut = new Generator();
            _fix = new Fixtures();
        }

        [Test]
        [TestCase("test1.txt")]
        [TestCase("test2.txt")]
        [TestCase("test3.txt")]
        public void Test_Generator_Level1(String input)
        {
            //Arrange
            AST ast = _fix.GetFixture(input);

            //Act
            var result = _sut.Execute(ast);

            //Assert
            Assert.True(result.Length != 0);
            Assert.True(result.Contains("combat_default_player_comparable=player"));
            Assert.True(result.Contains("combat_default_player_comparision=nearby"));
            Assert.True(result.Contains("combat_default_player_comparision_true=attack"));
        }

        [Test]
        [TestCase("test3.txt")]
        public void Test_Generator_Exception(String input)
        {
            //Arrange
            AST ast = _fix.GetFixture(input);

            Mock<Configuration> mockedNode = new();
            mockedNode.Setup(x => x.GetChildren()).Throws(new Exception());

            ast.SetRoot(mockedNode.Object);

            //Act
            var result = _sut.Execute(ast);
            //Assert
            Assert.AreEqual(result, new Exception().Message);
        }
    }
}
