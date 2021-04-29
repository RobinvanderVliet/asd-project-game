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
        private Pipeline sut;

        [SetUp]
        public void Setup()
        {
            sut = new Pipeline();
        }

        /*
        * ParseString()
        *
        * Parse incoming string
        * @author Abdul     
       */
        [Test]
        public void Test_ParseString_HappyPath()
        {
            //Arrange
            //Act
            sut.ParseString(SCRIPT);
            var result = sut.Ast;

            //Assert
            Assert.NotNull(result);
            Assert.IsEmpty(sut.Errors);
        }

        [Test]
        public void Test_ParseString_SadPath()
        {
            //Arrange
            Mock<Configuration> mockedAst = new();
            mockedAst.Setup(x => x.GetChildren()).Throws(new Exception());

            sut.ParseString(SCRIPT);
            sut.Ast.SetRoot(mockedAst.Object);

            //Act
            var result = sut.GenerateAst();

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(result, new Exception().Message);
            Assert.IsEmpty(sut.Errors);
        }

        [Test]
        public void Test_CheckAst()
        {
            //Arrange
            AST ast = new AST();
            sut.Ast = ast;

            Mock<Checker> mockedChecker = new Mock<Checker>(ast);
            mockedChecker.Setup(x => x.Check(ast)).Verifiable();

            sut.Checker = mockedChecker.Object;

            //Act
            sut.CheckAst();

            //Assert
            mockedChecker.Verify(x => x.Check(ast), Times.Once);
        }

        [Test]
        public void Test_Pipeline_Exception()
        {
            //Arrange

            //Act
            //Assert
            Assert.Throws<SyntaxErrorException>(() => sut.SyntaxError(It.IsAny<IRecognizer>(), It.IsAny<IToken>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<String>(), It.IsAny<RecognitionException>()));
        }

        [Test]
        public void Test_ClearErrors1()
        {
            //Arrange
            sut.Errors.Add("error 1");
            sut.Errors.Add("error 2");
            sut.Errors.Add("error 3");
            //Extra check voor input, voor het wissen;
            Assert.AreEqual(sut.Errors.Count, 3);

            //Act
            sut.ClearErrors();

            //Assert
            Assert.AreEqual(sut.Errors.Count, 0);

        }
    }
}