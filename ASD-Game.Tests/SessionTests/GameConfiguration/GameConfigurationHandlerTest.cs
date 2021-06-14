using System;
using System.Diagnostics.CodeAnalysis;
using ASD_Game.DatabaseHandler.POCO;
using ASD_Game.DatabaseHandler.Services;
using ASD_Game.Session.GameConfiguration;
using ASD_Game.UserInterface;
using Moq;
using NUnit.Framework;

namespace ASD_Game.Tests.SessionTests.GameConfiguration
{
    [ExcludeFromCodeCoverage]
    public class GameConfigurationHandlerTest
    {
        private GameConfigurationHandler _sut;
        private Mock<ScreenHandler> _mockScreenHandler;
        private Mock<IDatabaseService<GameConfigurationPOCO>> _mockedGameConfigServicesDb;

        [SetUp]
        public void Setup()
        {
            _mockScreenHandler = new Mock<ScreenHandler>();
            _mockedGameConfigServicesDb = new Mock<IDatabaseService<GameConfigurationPOCO>>();

            Mock<ConfigurationScreen> _screen = new Mock<ConfigurationScreen>();
            _mockScreenHandler.Object.Screen = _screen.Object;
            _screen.Setup(x => x.UpdateInputMessage(""));

            _sut = new GameConfigurationHandler(_mockScreenHandler.Object, _mockedGameConfigServicesDb.Object);
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

        [TestCase("1", MonsterDifficulty.Easy)]
        [TestCase("2", MonsterDifficulty.Medium)]
        [TestCase("3", MonsterDifficulty.Hard)]
        [TestCase("4", MonsterDifficulty.Impossible)]
        [Test]
        public void Test_UpdateMonsterDifficultyUpdatesMonsterDifficulty(string input, MonsterDifficulty difficulty)
        {
            //Arrange
            //Todo: Screen mocken voor update

            //Act
            _sut.UpdateMonsterDifficulty(input);
            
            //Assert
            Assert.AreEqual(difficulty, _sut.NewMonsterDifficulty);
            Assert.AreEqual(true, _sut.NextScreen);
        }
        
        [TestCase("6")]
        [TestCase("falseChoice")]
        [Test]
        public void Test_UpdateMonsterDifficultyDoesntExist(string input)
        {
            //Arrange


            //Act
            _sut.UpdateMonsterDifficulty(input);
            
            //Assert
            Assert.False(_sut.NextScreen);
        }
        
        [Test]
        public void Test_HandleAnswerSetsUsername()
        {
            //Arrange
            _sut.SetGameConfiguration();
            Mock<ConfigurationScreen> _screen = new Mock<ConfigurationScreen>();
            _mockScreenHandler.Object.Screen = _screen.Object;
            _screen.Setup(x => x.UpdateConfigurationScreen(_sut.ConfigurationHeader[_sut.OptionCounter], _sut.ConfigurationChoices[_sut.OptionCounter]));

            //Act
            _sut.HandleAnswer("1");
            _sut.HandleAnswer("1");
            _sut.HandleAnswer("1");
            _sut.HandleAnswer("Abdul");

            //Assert
            Assert.AreEqual("Abdul", _sut.Username);
        }
    }
}