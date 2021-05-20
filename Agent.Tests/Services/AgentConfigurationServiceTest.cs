using Agent.exceptions;
using Agent.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Agent.Exceptions;
using Agent.Mapper;
using Agent.Models;

namespace Agent.Tests
{
    [ExcludeFromCodeCoverage]
    public class AgentConfigurationServiceTests
    {
        private AgentConfigurationService _sut;
        private Mock<FileHandler> _fileHandlerMock;
        private Mock<Pipeline> _pipelineMock;

        [SetUp]
        public void Setup()
        {
            _sut = new AgentConfigurationService(new List<Configuration>(), new FileToDictionaryMapper());
            _fileHandlerMock = new Mock<FileHandler>();
            _sut.FileHandler = _fileHandlerMock.Object;
            _pipelineMock = new Mock<Pipeline>();
            _sut.Pipeline = _pipelineMock.Object;
        }

        [Test]
        public void Test_Configure_SyntaxError()
        {
            //Arrange
            var input = String.Format(Path.GetFullPath(Path.Combine
                        (AppDomain.CurrentDomain.BaseDirectory, @"..\\..\\..\\"))) + "resource\\AgentConfigurationTestFileParseException.txt";

            Mock<ConsoleRetriever> mockedRetriever = new();
            mockedRetriever.SetupSequence(x => x.GetConsoleLine()).Returns(input).Returns("cancel");

            _sut.ConsoleRetriever = mockedRetriever.Object;

            _fileHandlerMock.Setup(x => x.ImportFile(It.IsAny<String>())).Returns("wrong:wrong");

            //Act
            _sut.Configure();

            //Assert
            Assert.AreEqual("missing '=' at 'wrong'", _sut.LastError);
        }
        
        [Test]
        public void Test_Configure_CatchesSemanticError()
        {
            //Arrange
            var input = String.Format(Path.GetFullPath(Path.Combine
                (AppDomain.CurrentDomain.BaseDirectory, @"..\\..\\..\\"))) + "Resources\\AgentTestFileWrongExtension.txt";
            var error = "Semantic error";
            
            Mock<ConsoleRetriever> mockedRetriever = new();
            mockedRetriever.SetupSequence(x => x.GetConsoleLine()).Returns(input).Returns("cancel");
            _sut.ConsoleRetriever = mockedRetriever.Object;
            
            _fileHandlerMock.Setup(x => x.ImportFile(It.IsAny<String>())).Returns("explore=high");
            _pipelineMock.Setup(x => x.CheckAst()).Throws(new SemanticErrorException(error));

            //Act
            _sut.Configure();

            //Assert
            Assert.AreEqual(error, _sut.LastError);
        }
        
        [Test]
        public void Test_Configure_FileError()
        {
            //Arrange
            var input = String.Format(Path.GetFullPath(Path.Combine
                (AppDomain.CurrentDomain.BaseDirectory, @"..\\..\\..\\"))) + "Resources\\AgentTestFileWrongExtension.txt";
            var error = "File not found";
            
            Mock<ConsoleRetriever> mockedRetriever = new();
            mockedRetriever.SetupSequence(x => x.GetConsoleLine()).Returns(input).Returns("cancel");
            _sut.ConsoleRetriever = mockedRetriever.Object;
            _fileHandlerMock.Setup(x => x.ImportFile(It.IsAny<String>())).Throws(new FileException(error));

            //Act
            _sut.Configure();

            //Assert
            Assert.AreEqual(error, _sut.LastError);
        }

        [Test]
        public void Test_Configure_SavesFileInAgentFolder()
        {
            //Arrange
            var input = String.Format(Path.GetFullPath(Path.Combine
                (AppDomain.CurrentDomain.BaseDirectory, @"..\\..\\..\\"))) + "Resources\\AgentConfigurationTestFile.txt";

            Mock<ConsoleRetriever> mockedRetriever = new();
            mockedRetriever.SetupSequence(x => x.GetConsoleLine()).Returns(input);
            
            _sut.ConsoleRetriever = mockedRetriever.Object;

            _fileHandlerMock.Setup(x => x.ImportFile(It.IsAny<String>())).Returns("aggressiveness=high");

            //Act
            _sut.Configure();
            
            //Assert
            _fileHandlerMock.Verify( x => x.ExportFile(It.IsAny<String>(), It.IsAny<String>()), Times.Exactly(1));
        }
        
        [Test]
        public void Test_CreateNewAgentConfiguration_WithNewAgent()
        {
            //Arrange
            var filepath =
                String.Format(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\\..\\..\\"))) +
                "resource\\agent_test.cfg";

            //Act
            _sut.CreateConfiguration("Agent", filepath);

            //Assert
            Assert.True(_sut.GetConfigurations().Count > 0);
            Assert.AreEqual(_sut.GetConfigurations()[0].GetSetting("explore"), "random");
        }

        [Test]
        public void Test_Configure_Semantic()
        {
            //TODO not for this sprint bc of decision
        }
    }
}
