using System.Diagnostics.CodeAnalysis;
using ASD_Game.UserInterface;
using Moq;
using NUnit.Framework;

namespace ASD_Game.Tests.UserInterfaceTests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    class GameStatScreenTest
    {

        private GameStatScreen _sut;
        private Mock<ScreenHandler> _mockedScreenHandler;
        private Mock<ConsoleHelper> _mockedConsoleHelper;

        private const int STAT_X = 0;
        private const int STAT_Y = 0;
        private const int STAT_WIDTH = 120;
        private const int STAT_HEIGHT = 5;

        [SetUp]
        public void Setup()
        {
            _sut = new GameStatScreen(STAT_X, STAT_Y, STAT_WIDTH, STAT_HEIGHT);
            _mockedScreenHandler = new Mock<ScreenHandler>();
            _mockedConsoleHelper = new Mock<ConsoleHelper>();
            var screenHandler = _mockedScreenHandler.Object;
            screenHandler.ConsoleHelper = _mockedConsoleHelper.Object;
            _sut.SetScreen(screenHandler);
        }

        [Test]
        public void Test_DrawScreen_DrawsScreen()
        {
            //Arrange            
            var ulCorner = "╔";
            var llCorner = "╚";
            var urCorner = "╗";
            var lrCorner = "╝";

            //Act
            _sut.DrawScreen();

            //Assert
            _mockedConsoleHelper.Verify(mock => mock.Write(ulCorner), Times.Exactly(1));
            _mockedConsoleHelper.Verify(mock => mock.Write(urCorner), Times.Exactly(1));
            _mockedConsoleHelper.Verify(mock => mock.Write(llCorner), Times.Exactly(1));
            _mockedConsoleHelper.Verify(mock => mock.Write(lrCorner), Times.Exactly(1));
        }

        [Test]
        public void Test_SetStatValues_DrawsStatsProperly()
        {
            //Arrange
            string name = "TestName";
            int score = 0;
            int health = 100;
            int stamina = 100;
            int armor = 10;
            int radiation = 100;
            string helm = "Bandana";
            string body = "Jacket";
            string weapon = "Knife";
            string slotOne = "Bandage";
            string slotTwo = "Morphine";
            string slotThree = "Empty";

            //Act
            _sut.SetStatValues(name, score, health, stamina, armor, radiation, helm, body, weapon, slotOne, slotTwo, slotThree);

            //Assert
            _mockedConsoleHelper.Verify(mock => mock.Write(name), Times.Exactly(1));
            _mockedConsoleHelper.Verify(mock => mock.Write("Score: " + score), Times.Exactly(1));
            _mockedConsoleHelper.Verify(mock => mock.Write("Health: " + health), Times.Exactly(1));
            _mockedConsoleHelper.Verify(mock => mock.Write("Stamina: " + stamina), Times.Exactly(1));
            _mockedConsoleHelper.Verify(mock => mock.Write("Armor: " + armor), Times.Exactly(1));
            _mockedConsoleHelper.Verify(mock => mock.Write("RPP: " + radiation), Times.Exactly(1));
            _mockedConsoleHelper.Verify(mock => mock.Write("Helmet: " + helm), Times.Exactly(1));
            _mockedConsoleHelper.Verify(mock => mock.Write("Body: " + body), Times.Exactly(1));
            _mockedConsoleHelper.Verify(mock => mock.Write("Weapon: " + weapon), Times.Exactly(1));
            _mockedConsoleHelper.Verify(mock => mock.Write("Slot 1: " + slotOne), Times.Exactly(1));
            _mockedConsoleHelper.Verify(mock => mock.Write("Slot 2: " + slotTwo), Times.Exactly(1));
            _mockedConsoleHelper.Verify(mock => mock.Write("Slot 3: " + slotThree), Times.Exactly(1));
        }
    }
}
