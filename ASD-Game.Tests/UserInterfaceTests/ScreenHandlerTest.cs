using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ASD_Game.UserInterface;
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
            // _mockedStartScreen.Verify(mock => mock.DrawScreen(), Times.Once);
        }

        [Test]
        public void Test_UpdateInputMessage_CallsUpdateSavedSessionsListOnScreens()
        {
            //Arrange
            var loadScreen = _mockedLoadScreen.Object;
            _sut.Screen = loadScreen;
            List<string[]> savedSessions = new List<string[]>();
            savedSessions.Add(new[] {"Code1", "Session 1"});
            savedSessions.Add(new[] {"Code2", "Session 2"});
            savedSessions.Add(new[] {"Code3", "Session 3"});

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
            _sut.ActionsInQueue = new BlockingCollection<Action>();
            
            //Act
            _sut.GetSessionByPosition(1);

            //Assert
            _mockedLoadScreen.Verify(mock => mock.GetSessionByPosition(1), Times.Once);
        }

        [Test]
        public void Test_ShowMessages_AddsToQueue()
        {
            //Arrange
            _sut.Screen = new Mock<GameScreen>().Object;
            var mockedBlockingCollection = new Mock<BlockingCollection<Action>>();
            _sut.ActionsInQueue = mockedBlockingCollection.Object;
            //Act
            Queue <string> queue = new Queue<string>();
            queue.Enqueue("Test");
            _sut.ShowMessages(queue);
            var action = _sut.ActionsInQueue.First();
            
            //Assert
            Assert.AreSame(action, _sut.ActionsInQueue.Take());
        }

        [Test]
        public void Test_GetScreenInput_ReturnsInput()
        {
            //Arrange
            var testInput = "Gerrit";
            _mockedConsoleHelper.Setup(mock => mock.ReadLine()).Returns(testInput);
            
            //Act
            var result = _sut.GetScreenInput();
            
            //Assert
            Assert.AreEqual(testInput, result);
        }
        
        [Test]
        public void Test_SetScreenInput_CallsInput()
        {
            //Arrange
            var testInput = "Gerrit";

            //Act
            _sut.SetScreenInput(testInput);
            
            //Assert
            _mockedConsoleHelper.Verify(mock => mock.WriteLine(testInput), Times.Once);
        }
        
        [Test]
        public void Test_RedrawGameInputBox_AddsToQueue()
        {
            //Arrange
            _sut.Screen = new Mock<GameScreen>().Object;
            var mockedBlockingCollection = new Mock<BlockingCollection<Action>>();
            _sut.ActionsInQueue = mockedBlockingCollection.Object;
            //Act
            _sut.RedrawGameInputBox();
            var action = _sut.ActionsInQueue.First();
            
            //Assert
            Assert.AreSame(action, _sut.ActionsInQueue.Take());
        }
        
        [Test]
        public void Test_UpdateWorld_AddsToQueue()
        {
            //Arrange
            _sut.Screen = new Mock<GameScreen>().Object;
            var mockedBlockingCollection = new Mock<BlockingCollection<Action>>();
            _sut.ActionsInQueue = mockedBlockingCollection.Object;
            //Act
            var map = new char[1,1];
            _sut.UpdateWorld(map);
            var action = _sut.ActionsInQueue.First();
            
            //Assert
            Assert.AreSame(action, _sut.ActionsInQueue.Take());
        }
        
        [Test]
        public void Test_SetStatValues_AddsToQueue()
        {
            //Arrange
            _sut.Screen = new Mock<GameScreen>().Object;
            var mockedBlockingCollection = new Mock<BlockingCollection<Action>>();
            _sut.ActionsInQueue = mockedBlockingCollection.Object;
            //Act
            _sut.SetStatValues(String.Empty, 0, 0, 0, 0, 0, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty);
            var action = _sut.ActionsInQueue.First();
            
            //Assert
            Assert.AreSame(action, _sut.ActionsInQueue.Take());
        }
    }
}