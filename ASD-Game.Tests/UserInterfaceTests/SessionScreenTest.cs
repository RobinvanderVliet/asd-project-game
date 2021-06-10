using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ASD_Game.UserInterface;
using Moq;
using NUnit.Framework;

namespace ASD_Game.Tests.UserInterfaceTests
{
    [ExcludeFromCodeCoverage]
    public class SessionScreenTest
    {
        private SessionScreen _sut;
        private Mock<ScreenHandler> _mockedScreenHandler;
        private Mock<ConsoleHelper> _mockedConsoleHelper;
        
        [SetUp]
        public void Setup()
        {
            _sut = new SessionScreen();
            _mockedScreenHandler = new Mock<ScreenHandler>();
            _mockedConsoleHelper = new Mock<ConsoleHelper>();
            var screenHandler = _mockedScreenHandler.Object;
            screenHandler.ConsoleHelper = _mockedConsoleHelper.Object;
            _sut.SetScreen(screenHandler);
        }

        [Test]
        public void Test_UpdateWithNewSession_AddsNewSession()
        {
            //Arrange
            var sessionInfo = new[] {"1", "TestSession", "Gerrit", "2"};

            //Act
            _sut.UpdateWithNewSession(sessionInfo);
            
            //Assert
            _mockedConsoleHelper.Verify(mock => mock.Write("(1) " + sessionInfo[1] + " | Created by: " + sessionInfo[2] + " | Players: " + sessionInfo[3] + "/8"), Times.Once);
        }

        [Test]
        public void Test_GetSessionIdByVisualNumber_ReturnsSession()
        {
            //Arrange
            var sessionId = "2212";
            var sessionsInfoList = new List<string[]>();
            sessionsInfoList.Add(new[] {sessionId, "TestSession", "Gerrit", "2"});
            _sut.SessionInfoList = sessionsInfoList;
            
            //Act
            var returnedSessionId = _sut.GetSessionIdByVisualNumber(0);
            
            //Assert
            Assert.AreEqual(sessionId, returnedSessionId);
        }
        
        [Test]
        public void Test_GetSessionIdByVisualNumber_ReturnsNull()
        {
            //Arrange
            var sessionsInfoList = new List<string[]>();
            _sut.SessionInfoList = sessionsInfoList;
            
            //Act
            var returnedSessionId = _sut.GetSessionIdByVisualNumber(0);
            
            //Assert
            Assert.AreEqual(null, returnedSessionId);
        }
        
        [Test] 
        public void Test_DrawScreen_DrawsScreen()
        {
            //Arrange
            var headerText = "There are currently 0 sessions you can join";

            //Act
            _sut.DrawScreen();
            
            //Assert
            _mockedConsoleHelper.Verify(mock => mock.Write(headerText), Times.Once);
        }
    }
}