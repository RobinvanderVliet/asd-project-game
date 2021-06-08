using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Moq;
using NUnit.Framework;

namespace UserInterface.Tests
{
    [ExcludeFromCodeCoverage]
    public class ScreenHandlerTest
    {
        private IScreenHandler _sut;
        private Mock<StartScreen> _mockedStartScreen;
        private Mock<LoadScreen> _mockedLoadScreen;
        private Mock<ConsoleHelper> _mockedConsoleHelper;

        [SetUp]
        public void Setup()
        {
            _sut = new ScreenHandler();
            _mockedStartScreen = new Mock<StartScreen>();
            _mockedLoadScreen = new Mock<LoadScreen>();
            _mockedConsoleHelper = new Mock<ConsoleHelper>();
            _sut.ConsoleHelper = _mockedConsoleHelper.Object;
        }

        [Test]
        public void Test_TransitionTo_ChangesAndDrawsScreen()
        {
            //Arrange
            var startScreen = _mockedStartScreen.Object;
            //Act
            _sut.TransitionTo(startScreen);
            //Assert
            Assert.True(_sut.Screen.Equals(startScreen));
            _mockedStartScreen.Verify(mock => mock.DrawScreen(), Times.Once);
        }
        
        [Test]
        public void Test_UpdateInputMessage_CallsUpdateSavedSessionsListOnScreens()
        {
            //Arrange
            var loadScreen = _mockedLoadScreen.Object;
            _sut.Screen = loadScreen;
            List<string[]> savedSessions = new List<string[]>();
            savedSessions.Add(new []{"Code1", "Session 1"});
            savedSessions.Add(new []{"Code2", "Session 2"});
            savedSessions.Add(new []{"Code3", "Session 3"});
            
            //Act
            _sut.UpdateSavedSessionsList(savedSessions);
            
            //Assert
            _mockedLoadScreen.Verify(mock => mock.UpdateSavedSessionsList(savedSessions), Times.Once);
        }
        
        [Test]
        public void Test_UpdateInputMessage_CallsUpdateInputMessageOnScreens()
        {
            //Arrange
            var testMessage = "Test message";
            var loadScreen = _mockedLoadScreen.Object;
            _sut.Screen = loadScreen;
            
            //Act
            _sut.UpdateInputMessage(testMessage);
            
            //Assert
            _mockedLoadScreen.Verify(mock => mock.UpdateInputMessage(testMessage), Times.Once);
        }
        
        [Test]
        public void Test_GetSessionByPosition_CallsGetSessionByPositionOnScreens()
        {
            //Arrange
            var loadScreen = _mockedLoadScreen.Object;
            _sut.Screen = loadScreen;
            
            //Act
            _sut.GetSessionByPosition(1);
            
            //Assert
            _mockedLoadScreen.Verify(mock => mock.GetSessionByPosition(1), Times.Once);
        }
    }
}

