using System.Diagnostics.CodeAnalysis;
using Moq;
using NUnit.Framework;
using Session.GameConfiguration;
using UserInterface;

namespace Session.Tests.GameConfiguration
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

        [Test]
        public void Test_UpdateMonsterDifficultyDefault()
        {
            //Arrange
            

            //Act
            _sut.UpdateMonsterDifficulty("0");
            
            //Assert
            Assert.AreEqual(false, _sut.NextScreen);
        }
    }
}