using System.Diagnostics.CodeAnalysis;
using Moq;
using NUnit.Framework;
using Session.GameConfiguration;
using UserInterface;

namespace Agent.Tests.GameConfiguration
{
    [ExcludeFromCodeCoverage]
    public class GameConfigurationHandlerTest
    {
        private GameConfigurationHandler _sut;
        private Mock<ScreenHandler> _mockScreenHandler;

        [SetUp]
        public void Setup()
        {
            _mockScreenHandler = new Mock<ScreenHandler>();
            _sut = new GameConfigurationHandler(_mockScreenHandler.Object);
        }

        [Test]
        public void Test_SetConfigurationResetsCounter()
        {
            //Arrange
            

            //Act
            _sut.SetGameConfiguration();
            
            //Assert
            Assert.AreEqual(0, _sut.OptionCounter);
        }
        
        [Test]
        public void Test_SetConfigurationSetsNextScreenOnFalse()
        {
            //Arrange
            

            //Act
            _sut.SetGameConfiguration();
            
            //Assert
            Assert.AreEqual(false, _sut.NextScreen);
        }
        
        [Test]
        public void Test_SetConfigurationSetsConfigurationHeaders()
        {
            //Arrange
            

            //Act
            _sut.SetGameConfiguration();
            
            //Assert
            Assert.IsNotEmpty(_sut.ConfigurationHeader);
        }
        
        [Test]
        public void Test_SetConfigurationSetsConfigurationChoices()
        {
            //Arrange
            

            //Act
            _sut.SetGameConfiguration();
            
            //Assert
            Assert.IsNotEmpty(_sut.ConfigurationChoices);
        }
    }
}