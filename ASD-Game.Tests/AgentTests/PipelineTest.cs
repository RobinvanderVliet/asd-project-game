using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Antlr4.Runtime;
using ASD_Game.Agent;
using ASD_Game.Agent.Antlr.Ast;
using ASD_Game.Agent.Antlr.Checker;
using ASD_Game.Agent.Exceptions;
using Moq;
using NUnit.Framework;

namespace ASD_Game.Tests.AgentTests
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
            //Act
            _sut.ParseString("TEST =TEST");

            //Assert
            Assert.IsNotEmpty(_sut.Errors);
        }

        [Test]
        public void Test_CheckAst_NoError()
        {
            //Arrange
            Mock<AST> mockedAst = new Mock<AST>();
            mockedAst.Setup(x => x.GetErrors()).Returns(new List<ASTError>());
            
            Mock<Checking> mockedChecker = new Mock<Checking>();
            mockedChecker.Setup(x => x.Check(It.IsAny<Node>())).Verifiable();
            
            _sut.Ast = mockedAst.Object;
            _sut.Checking = mockedChecker.Object;

            //Act
            _sut.CheckAst();

            //Assert
            mockedChecker.Verify(x => x.Check(It.IsAny<Node>()), Times.Once);
            Assert.IsEmpty(_sut.Errors);
        }
        
        
        [Test]
        public void Test_CheckAst_Error()
        {
            //Arrange
            Mock<AST> mockedAst = new Mock<AST>();
            mockedAst.Setup(x => x.GetErrors()).Returns(new List<ASTError>{new ("TEST")});
            
            Mock<Checking> mockedChecker = new Mock<Checking>();
            mockedChecker.Setup(x => x.Check(It.IsAny<Node>())).Verifiable();
            
            _sut.Ast = mockedAst.Object;
            _sut.Checking = mockedChecker.Object;

            //Act
            _sut.CheckAst();

            //Assert
            mockedChecker.Verify(x => x.Check(It.IsAny<Node>()), Times.Once);
            Assert.IsNotEmpty(_sut.Errors);
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