using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Agent.exceptions;
using Agent.Mapper;
using Agent.Models;
using Agent.Services;
using Agent.Services.interfaces;
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
            _sut._fileHandler = _fileHandlerMock.Object;
            _pipelineMock = new Mock<Pipeline>();
            _sut._pipeline = _pipelineMock.Object;
        }
        
        [Test]
        public void Test_CreateNewNpcConfiguration_WithNewNpc()
        {
            //Arrange
            var filepath = String.Format(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\\..\\..\\"))) + "resource\\npcFileTest.txt";
            
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

            _sut._consoleRetriever = mockedRetriever.Object;

            //Act
            _sut.Configure();

            //Assert
            Assert.AreEqual("missing '=' at 'code'", _sut.lastError);
        }
        
        [Test]
        public void Test_Configure_CatchesSemanticError()
        {
            //Arrange
            var code = "explore=high";
            var error = "Semantic error";
            
            Mock<ConsoleRetriever> mockedRetriever = new();
            mockedRetriever.SetupSequence(x => x.GetConsoleLine()).Returns("zombie").Returns(code).Returns("cancel");
            _sut._consoleRetriever = mockedRetriever.Object;
            
            _fileHandlerMock.Setup(x => x.ImportFile(It.IsAny<String>())).Returns(code);
            _pipelineMock.Setup(x => x.CheckAst()).Throws(new SemanticErrorException(error));

            //Act
            _sut.Configure();

            //Assert
            Assert.AreEqual(error, _sut.lastError);
        }
        
        [Test]
        public void Test_Configure_SavesFileInNpcFolder()
        {
            Mock<ConsoleRetriever> mockedRetriever = new();
            mockedRetriever.SetupSequence(x => x.GetConsoleLine()).Returns("zombie").Returns("aggressiveness=high");
            
            _sut._consoleRetriever = mockedRetriever.Object;
            
            //Act
            _sut.Configure();
            
            //Assert
            _fileHandlerMock.Verify( x => x.ExportFile(It.IsAny<String>(), It.IsAny<String>()), Times.Exactly(1));
        }
    }
}