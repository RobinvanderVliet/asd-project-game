using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ASD_Game.Agent;
using ASD_Game.Agent.Exceptions;
using ASD_Game.Agent.Mapper;
using ASD_Game.Agent.Models;
using ASD_Game.Agent.Services;
using ASD_Game.InputHandling;
using Moq;
using NUnit.Framework;

namespace ASD_Game.Tests.AgentTests.Services
{
    [ExcludeFromCodeCoverage]
    public class AgentConfigurationServiceTests
    {
        private AgentConfigurationService _sut;
        private FileHandler _handler;
        private Mock<FileHandler> _fileHandlerMock;
        private Mock<Pipeline> _pipelineMock;
        private const string INPUT = "aggressiveness=high";

        [SetUp]
        public void Setup()
        {
            _sut = new AgentConfigurationService(new List<Configuration>(), new FileToSettingListMapper());
            _fileHandlerMock = new Mock<FileHandler>();
            _sut.FileHandler = _fileHandlerMock.Object;
            _pipelineMock = new Mock<Pipeline>();
            _sut.Pipeline = _pipelineMock.Object;
            _handler = new FileHandler();
        }

        [Test]
        public void Test_Configure_SyntaxErrors()
        {
            //Arrange
            List<string> errors = new()
            {
                "error"
            };
            _pipelineMock.Setup(mock => mock.GetErrors()).Returns(errors);
            
            //Act
            var result = _sut.Configure(INPUT);
            
            //Assert
            Assert.AreEqual(errors,result);
            _pipelineMock.Verify(x => x.ParseString(INPUT),Times.Once);
            _pipelineMock.Verify(x => x.CheckAst(), Times.Never);
            _pipelineMock.Verify(x=> x.GenerateAst(), Times.Never);
            _fileHandlerMock.Verify( x => x.ExportFile(It.IsAny<string>(), It.IsAny<string>()), Times.Never);    
        }
        
        [Test]
        public void Test_Configure_SemanticErrors()
        {
            //Arrange
            List<string> errors = new()
            {
                "error"
            };
            _pipelineMock.SetupSequence(mock => mock.GetErrors()).Returns(new List<string>()).Returns(errors).Returns(errors);
            
            //Act
            var result = _sut.Configure(INPUT);
            
            //Assert
            Assert.AreEqual(errors,result);
            _pipelineMock.Verify(x => x.ParseString(INPUT),Times.Once);
            _pipelineMock.Verify(x => x.CheckAst(), Times.Once);
            _pipelineMock.Verify(x=> x.GenerateAst(), Times.Never);
            _fileHandlerMock.Verify( x => x.ExportFile(It.IsAny<string>(), It.IsAny<string>()), Times.Never);    
        }
        
        [Test]
        public void Test_Configure_Correct()
        {
            //Arrange
            _pipelineMock.Setup(mock => mock.GetErrors()).Returns(new List<string>());
            
            //Act
            var result = _sut.Configure(INPUT);
            
            //Assert
            Assert.IsEmpty(result);
            _pipelineMock.Verify(x => x.ParseString(INPUT),Times.Once);
            _pipelineMock.Verify(x => x.CheckAst(), Times.Once);
            _pipelineMock.Verify(x=> x.GenerateAst(), Times.Once);
            _fileHandlerMock.Verify( x => x.ExportFile(It.IsAny<string>(), It.IsAny<string>()), Times.Once);    
        }
        

        [Test]
        public void Test_Configure_FileError()
        {
            //Arrange
            var error = "File not found";
            _fileHandlerMock.Setup(x => x.ExportFile(It.IsAny<String>(),It.IsAny<String>())).Throws(new FileException(error));
            _pipelineMock.Setup(mock => mock.GetErrors()).Returns(new List<string>());
            
            //Act
            _sut.Configure("gerrit");

            //Assert
            Assert.AreEqual(error, _sut.LastError);
        }

        [Test]
        public void Test_Configure_SavesFileInAgentFolder()
        {
            //Arrange
            _pipelineMock.Setup(mock => mock.GetErrors()).Returns(new List<string>());
            //Act
            _sut.Configure("aggressiveness=high");
            
            //Assert
            _fileHandlerMock.Verify(x => x.ExportFile(It.IsAny<String>(), It.IsAny<String>()), Times.Exactly(1));
        }

        [Test]
        public void Test_CreateNewAgentConfiguration_WithNewAgent()
        {
            //Arrange
            var filepath = _handler.GetBaseDirectory() + "/Resource/agent_test.cfg";

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
