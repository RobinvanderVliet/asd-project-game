using Agent.exceptions;
using Agent.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agent.Mapper;
using Agent.Models;

namespace Agent.Tests
{
    class AgentConfigurationServiceTests
    {
        AgentConfigurationService _sut;
        private Mock<FileHandler> _fileHandlerMock;

        [SetUp]
        public void Setup()
        {
            _sut = new AgentConfigurationService(new List<Configuration>(), new FileToDictionaryMapper());
            _fileHandlerMock = new Mock<FileHandler>();
            _sut._fileHandler = _fileHandlerMock.Object;
        }

        [Test]
        public void Test_StartConfiguration_SyntaxError()
        {
            //Arrange
            var input = String.Format(Path.GetFullPath(Path.Combine
                        (AppDomain.CurrentDomain.BaseDirectory, @"..\\..\\..\\"))) + "resource\\AgentConfigurationTestFileParseException.txt";

            Mock<ConsoleRetriever> mockedRetriever = new();
            mockedRetriever.SetupSequence(x => x.GetConsoleLine()).Returns(input).Returns("cancel");

            _sut.consoleRetriever = mockedRetriever.Object;

            _fileHandlerMock.Setup(x => x.ImportFile(It.IsAny<String>())).Returns("wrong:wrong");

            //Act
            _sut.StartConfiguration(ConfigurationType.Agent);

            //Assert
            Assert.AreEqual("missing '=' at 'wrong'", _sut.testVar);
        }

        [Test]
        public void Test_StartConfiguration_SavesFileInAgentFolder()
        {
            //Arrange
            var input = String.Format(Path.GetFullPath(Path.Combine
                (AppDomain.CurrentDomain.BaseDirectory, @"..\\..\\..\\"))) + "Resources\\AgentConfigurationTestFile.txt";

            Mock<ConsoleRetriever> mockedRetriever = new();
            mockedRetriever.SetupSequence(x => x.GetConsoleLine()).Returns(input);
            
            _sut.consoleRetriever = mockedRetriever.Object;

            _fileHandlerMock.Setup(x => x.ImportFile(It.IsAny<String>())).Returns("aggressiveness=high");

            //Act
            _sut.StartConfiguration(ConfigurationType.Agent);
            
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
        public void Test_StartConfiguration_Semantic()
        {
            //TODO not for this sprint bc of decision
        }
    }
}
