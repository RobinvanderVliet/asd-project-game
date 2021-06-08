using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Moq;
using NUnit.Framework;

namespace UserInterface.Tests
{
    [ExcludeFromCodeCoverage]
    public class LoadScreenTest
    {
        private LoadScreen _sut;
        private Mock<ScreenHandler> _mockedScreenHandler;
        private Mock<ConsoleHelper> _mockedConsoleHelper;
        
        [SetUp]
        public void Setup()
        {
            _sut = new LoadScreen();
            _mockedScreenHandler = new Mock<ScreenHandler>();
            _mockedConsoleHelper = new Mock<ConsoleHelper>();
            var screenHandler = _mockedScreenHandler.Object;
            screenHandler.ConsoleHelper = _mockedConsoleHelper.Object;
            _sut.SetScreen(screenHandler);
        }

        [Test]
        public void Test_DrawScreen_DrawsLoadScreen()
        {
            //Arrange
            var headerText = "Load a saved session";

            //Act
            _sut.DrawScreen();
            
            //Assert
            _mockedConsoleHelper.Verify(mock => mock.Write(headerText), Times.Once);
        }

        [Test]
        public void Test_UpdateSavedSessionsList_DrawsList()
        {
            //Arrange
            List<string[]> savedSessions = new List<string[]>();
            savedSessions.Add(new []{"Code1", "Session 1"});
            savedSessions.Add(new []{"Code2", "Session 2"});
            savedSessions.Add(new []{"Code3", "Session 3"});
            
            //Act
            _sut.UpdateSavedSessionsList(savedSessions);
            
            //Assert
            Assert.AreEqual(savedSessions, _sut.Sessions);
        }
        
        [Test]
        public void Test_GetSessionByPosition_ReturnsPosition()
        {
            //Arrange
            List<string[]> savedSessions = new List<string[]>();
            savedSessions.Add(new []{"Code1", "Session 1"});
            savedSessions.Add(new []{"Code2", "Session 2"});
            savedSessions.Add(new []{"Code3", "Session 3"});
            _sut.UpdateSavedSessionsList(savedSessions);
            
            //Act
            var result = _sut.GetSessionByPosition(0);
            
            //Assert
            Assert.AreEqual(savedSessions[0][0], result);
        }
        
        [Test]
        public void Test_GetSessionByPosition_ReturnsNull()
        {
            //Arrange
            List<string[]> savedSessions = new List<string[]>();
            _sut.UpdateSavedSessionsList(savedSessions);
            
            //Act
            var result = _sut.GetSessionByPosition(0);
            
            //Assert
            Assert.IsNull(result);
        }
    }
}