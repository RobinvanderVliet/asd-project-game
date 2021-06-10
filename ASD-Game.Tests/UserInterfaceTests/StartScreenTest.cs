using System.Diagnostics.CodeAnalysis;
using ASD_Game.UserInterface;
using Moq;
using NUnit.Framework;

namespace ASD_Game.Tests.UserInterfaceTests
{
    [ExcludeFromCodeCoverage]
    public class StartScreenTest
    {
        private StartScreen _sut;
        private Mock<ScreenHandler> _mockedScreenHandler;
        private Mock<ConsoleHelper> _mockedConsoleHelper;

        [SetUp]
        public void Setup()
        {
            _sut = new StartScreen();
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
            var headerText = "Welcome to The Apocalypse We Wanted! Pick an option to continue...";
            var optionText = "1: Host a new session";
            var ulCorner = "╔";
            var llCorner = "╚";
            var urCorner = "╗";
            var lrCorner = "╝";
        
            //Act
            _sut.DrawScreen();
            
            //Assert
            _mockedConsoleHelper.Verify(mock => mock.Write(headerText), Times.Once);
            _mockedConsoleHelper.Verify(mock => mock.Write(ulCorner), Times.Exactly(3));
            _mockedConsoleHelper.Verify(mock => mock.Write(urCorner), Times.Exactly(3));
            _mockedConsoleHelper.Verify(mock => mock.Write(llCorner), Times.Exactly(3));
            _mockedConsoleHelper.Verify(mock => mock.Write(lrCorner), Times.Exactly(3));
            _mockedConsoleHelper.Verify(mock => mock.Write(optionText), Times.Once);
        }

        [Test]
        public void Test_UpdateInputMessage_UpdatesMessage()
        {
            //Arrange
            var newHeaderText = "New header";
            //Act
            _sut.UpdateInputMessage(newHeaderText);
            //Assert
            _mockedConsoleHelper.Verify(mock => mock.Write(newHeaderText));
        }
    }
}