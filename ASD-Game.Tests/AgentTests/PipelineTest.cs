using Agent.Antlr.Ast;
using Agent.Exceptions;
using Antlr4.Runtime;
using Moq;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;

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
            _sut = new Pipeline();
        }

        [Test]
        public void Test_ParseString_HappyPath()
        {
            //Arrange
            //Act
            _sut.ParseString(SCRIPT);
            var result = _sut.Ast;

            //Assert
            Assert.NotNull(result);
            Assert.IsEmpty(_sut.Errors);
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

        //Deze test moet getest worden zodra de checker is geimplementeerd
        //[Test]
        //public void Test_CheckAst()
        //{
        //    //Arrange
        //    AST ast = new AST();
        //    _sut.Ast = ast;

        //    Mock<Antlr.Checker.Checking> mockedChecker = new Mock<Antlr.Checker.Checking>(ast);
        //    mockedChecker.Setup(x => x.Check(ast)).Verifiable();

        //    _sut.Checking = mockedChecker.Object;

        //    //Act
        //    _sut.CheckAst();

        //    //Assert
        //    mockedChecker.Verify(x => x.Check(ast), Times.Once);
        //}

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