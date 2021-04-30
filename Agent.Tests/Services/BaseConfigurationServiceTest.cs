using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Agent.Services;
using Moq;
using NUnit.Framework;

namespace Agent.Tests.Services
{
    [ExcludeFromCodeCoverage]
    public class BaseConfigurationServiceTest
    {
        private BaseConfigurationService _sut;
        private Mock<FileHandler> _fileHandlerMock;

        [SetUp]
        public void Setup()
        {
            _sut = new BaseConfigurationService();
            _fileHandlerMock = new Mock<FileHandler>();
            _sut.FileHandler = _fileHandlerMock.Object;
        }

        [Test]
        public void Test_StartConfiguration_ReturnsNullWhenCanceled()
        {
            //Arrange
            Console.SetOut(new StringWriter());
            Console.SetIn(new StringReader("cancel"));

            //Act
            _sut.StartConfiguration(ConfigurationType.Agent);

            //Assert
            _fileHandlerMock.Verify( x => x.ImportFile(It.IsAny<String>()), Times.Never);
        }
    }
}