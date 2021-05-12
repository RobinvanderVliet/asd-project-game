using System;
using Moq;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using Agent.antlr.ast;
using Agent.antlr.checker;
using Antlr4.Runtime;
using Agent.exceptions;

namespace Agent.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class PipelineTest
    {
        private static string SCRIPT = "combat{when player nearby player then attack}";
        private Pipeline _sut;

        [SetUp]
        public void Setup()
        {
            this._sut = new Pipeline();
        }
        
        [Test]
        public void Test_ParseString_HappyPath()
        {
            //Arrange
            //Act
            this._sut.ParseString(SCRIPT);
            var result = this._sut.Ast;

            //Assert
            Assert.NotNull(result);
            Assert.IsEmpty(this._sut.Errors);
        }

        [Test]
        public void Test_ParseString_SadPath()
        {
            //Arrange
            Mock<Configuration> mockedAst = new();
            mockedAst.Setup(x => x.GetChildren()).Throws(new Exception());

            _sut.ParseString(SCRIPT);
            _sut.Ast.SetRoot(mockedAst.Object);

            //Act
            var result = _sut.GenerateAst();

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(result, new Exception().Message);
            Assert.IsEmpty(_sut.Errors);
        }

        [Test]
        public void Test_CheckAst()
        {
            //Arrange
            AST ast = new AST();
            _sut.Ast = ast;

            Mock<Checker> mockedChecker = new Mock<Checker>(ast);
            mockedChecker.Setup(x => x.Check(ast)).Verifiable();

            _sut.Checker = mockedChecker.Object;

            //Act
            _sut.CheckAst();

            //Assert
            mockedChecker.Verify(x => x.Check(ast), Times.Once);
        }

        [Test]
        public void Test_Pipeline_Exception()
        {
            //Arrange

            //Act & Assert
            Assert.Throws<SyntaxErrorException>(() => _sut.SyntaxError(It.IsAny<IRecognizer>(), It.IsAny<IToken>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<String>(), It.IsAny<RecognitionException>()));
        }

        [Test]
        public void Test_ClearErrors1()
        {
            //Arrange
            _sut.Errors.Add("error 1");
            _sut.Errors.Add("error 2");
            _sut.Errors.Add("error 3");

            //Act
            _sut.ClearErrors();

            //Assert
            Assert.AreEqual(_sut.Errors.Count, 0);

        }
    }
}