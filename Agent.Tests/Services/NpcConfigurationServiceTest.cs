using Agent.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Agent.Mapper;
using Agent.Models;
using Agent.Services;
using Moq;
using NUnit.Framework;

namespace Agent.Tests.Services
{
    [ExcludeFromCodeCoverage]
    public class NpcConfigurationServiceTest
    {
        private NpcConfigurationService _sut;
        private Mock<FileHandler> _fileHandlerMock;
        private Mock<Pipeline> _pipelineMock;
        
        [SetUp]
        public void Setup()
        {
            _sut = new NpcConfigurationService(new List<Configuration>(), new FileToDictionaryMapper());
            _fileHandlerMock = new Mock<FileHandler>();
            _sut.FileHandler = _fileHandlerMock.Object;
            _pipelineMock = new Mock<Pipeline>();
            _sut.Pipeline = _pipelineMock.Object;
        }
        
        [Test]
        public void Test_CreateNewNpcConfiguration_WithNewNpc()
        {
            //Arrange
            var filepath = string.Format(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\\..\\..\\"))) + "resource\\npcFileTest.txt";
            
            //Act
            _sut.CreateConfiguration("TestNPCName", filepath);
            
            //Assert
            Assert.True(_sut.GetConfigurations().Count > 0);
            Assert.AreEqual(_sut.GetConfigurations()[0].GetSetting("aggressiveness"), "high");
        }

        [Test]
        public void Test_Configure_CatchesSyntaxError()
        {
            //Arrange
            Mock<ConsoleRetriever> mockedRetriever = new();
            mockedRetriever.SetupSequence(x => x.GetConsoleLine()).Returns("zombie").Returns("incorrect:code").Returns("cancel");

            _sut.ConsoleRetriever = mockedRetriever.Object;

            //Act
            _sut.Configure();

            //Assert
            Assert.AreEqual("missing '=' at 'code'", _sut.LastError);
        }
        
        [Test]
        public void Test_Configure_CatchesSemanticError()
        {
            //Arrange
            var code = "explore=high";
            var error = "Semantic error";
            
            Mock<ConsoleRetriever> mockedRetriever = new();
            mockedRetriever.SetupSequence(x => x.GetConsoleLine()).Returns("zombie").Returns(code).Returns("cancel");
            _sut.ConsoleRetriever = mockedRetriever.Object;
            
            _fileHandlerMock.Setup(x => x.ImportFile(It.IsAny<String>())).Returns(code);
            _pipelineMock.Setup(x => x.CheckAst()).Throws(new SemanticErrorException(error));

            //Act
            _sut.Configure();

            //Assert
            Assert.AreEqual(error, _sut.LastError);
        }
        
        [Test]
        public void Test_Configure_SavesFileInNpcFolder()
        {
            Mock<ConsoleRetriever> mockedRetriever = new();
            mockedRetriever.SetupSequence(x => x.GetConsoleLine()).Returns("zombie").Returns("aggressiveness=high");
            
            _sut.ConsoleRetriever = mockedRetriever.Object;
            
            //Act
            _sut.Configure();
            
            //Assert
            _fileHandlerMock.Verify( x => x.ExportFile(It.IsAny<String>(), It.IsAny<String>()), Times.Exactly(1));
        }
    }
}