using Agent.Mapper;
using Agent.Services;
using InputHandling;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using ASD_Game.Agent;
using ASD_Game.Agent.Services;
using ASD_Game.InputHandling;

namespace Agent.Tests.Services
{
    [ExcludeFromCodeCoverage]
    public class ConfigurationServiceTests
    {
        private ConfigurationService _sut;
        private FileHandler _handler;
        private Mock<FileHandler> _fileHandlerMock;
        private Mock<Pipeline> _pipelineMock;
        private Mock<InputHandler> _mockedRetriever;
        private Mock<IFileToConfigurationMapper> _mockedFileToConfigurationMapper;

        [SetUp]
        public void Setup()
        {
            _mockedFileToConfigurationMapper = new Mock<IFileToConfigurationMapper>();
            _sut = new ConfigurationService(_mockedFileToConfigurationMapper.Object);
        }

        
        [Test]
        [TestCase("agent")]
        [TestCase("gerrit")]
        public void Test_CreateConfiguration_CreatesConfiguration(string name)
        {
            //Arrange
            _fileHandlerMock = new Mock<FileHandler>();
            _sut.FileHandler = _fileHandlerMock.Object;
            List<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>();
            keyValuePairs.Add(new KeyValuePair<string, string>("combat", "offensive"));

            var path = String.Empty;
            if (name.Equals("agent"))
            {
                path = name + Path.DirectorySeparatorChar + name + "-config.cfg";
            }
            else
            {
                path = "npc" + Path.DirectorySeparatorChar + name + "-config.cfg";
            }

            _mockedFileToConfigurationMapper.Setup(mock => mock.MapFileToConfiguration(path)).Returns(keyValuePairs);
            _fileHandlerMock.Setup(mock => mock.FileExist(path)).Returns(false);
            _fileHandlerMock.Setup(mock => mock.GetBaseDirectory()).Returns("bram");
            
            //Act
            _sut.CreateConfiguration(name);

            //Assert
            _mockedFileToConfigurationMapper.Verify(mock => mock.MapFileToConfiguration("bram" + Path.DirectorySeparatorChar + "resource" + Path.DirectorySeparatorChar + path), Times.Once);
        }
    }
}
