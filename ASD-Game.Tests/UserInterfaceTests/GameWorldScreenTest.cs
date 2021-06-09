using Moq;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace UserInterface.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    internal class GameWorldScreenTest
    {
        private GameWorldScreen _sut;
        private Mock<ScreenHandler> _mockedScreenHandler;
        private Mock<ConsoleHelper> _mockedConsoleHelper;

        private const int WORLD_HEIGHT = 13;
        private const int WORLD_WITH = 37;
        private const int WORLD_X = 0;
        private const int WORLD_Y = 0;

        [SetUp]
        public void Setup()
        {
            _sut = new GameWorldScreen(WORLD_X, WORLD_Y, WORLD_WITH, WORLD_HEIGHT);
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
        public void Test_UpdateWorld_DrawsWorldProperly()
        {
            //Arrange
            char[,] newMap = new char[13, 13] {
            {'A','.','~','~','~','~','~','~','~','~','~','~','B'},
             {'.','.','~','~','~','~','~','~','~','~','~','~','~'},
             {'.','.','~','~','~','~','~','~','~','~','~','~','~'},
             {'.','.','~','~','~','~','~','~','~','~','~','~','~'},
             {'.','.','~','~','~','~','~','~','~','~','~','~','~'},
             {'.','.','~','~','~','~','~','~','~','~','~','~','~'},
             {'.','.','~','~','~','~','~','~','~','~','~','~','~'},
             {'.','.','~','~','~','~','~','~','~','~','~','~','~'},
             {'.','.','~','~','~','~','~','~','~','~','~','~','~'},
             {'.','.','~','~','~','~','~','~','~','~','~','~','~'},
             {'.','.','~','~','~','~','☻','~','~','~','~','~','~'},
             {'.','.','~','~','~','~','~','~','~','~','~','~','~'},
             {'C','.','~','~','~','~','~','~','~','~','~','~','D'},
            };

            //Act
            _sut.UpdateWorld(newMap);
            //Assert
            _mockedConsoleHelper.Verify(mock => mock.Write("A"), Times.Exactly(1));
            _mockedConsoleHelper.Verify(mock => mock.Write("B"), Times.Exactly(1));
            _mockedConsoleHelper.Verify(mock => mock.Write("C"), Times.Exactly(1));
            _mockedConsoleHelper.Verify(mock => mock.Write("D"), Times.Exactly(1));
        }
    }
}