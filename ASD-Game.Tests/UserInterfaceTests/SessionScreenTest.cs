using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ASD_Game.Session.DTO;
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
            var sessionInfo = new List<SessionDTO>(); 
            var session = new SessionDTO();
            session.Name = "Test session";
            session.SessionStarted = true;
            session.SessionId = "session";
            session.Clients = new List<string[]>();
            session.Clients.Add(new []{"HostId", "HostName"});
            sessionInfo.Add(session);
        
            //Act
            _sut.UpdateWithNewSession(sessionInfo);
        
            //Assert
            _mockedConsoleHelper.Verify(mock => mock.Write("(1) " + sessionInfo[0].Name + " | Created by: " + sessionInfo[0].Clients[0][1] + " | Players: " + sessionInfo[0].Clients.Count + "/8 | Status: In progress"), Times.Once);
        }

        [Test]
        public void Test_GetSessionIdByVisualNumber_ReturnsSession()
        {
            //Arrange
            var sessionInfo = new List<SessionDTO>(); 
            var session = new SessionDTO();
            session.Name = "Test session";
            session.SessionStarted = true;
            session.SessionId = "session";
            session.Clients = new List<string[]>();
            session.Clients.Add(new []{"HostId", "HostName"});
            sessionInfo.Add(session);
            _sut.UpdateWithNewSession(sessionInfo);
            
            //Act
            var returnedSessionId = _sut.GetSessionIdByVisualNumber(0);
            
            //Assert
            Assert.AreEqual(session.SessionId, returnedSessionId);
        }
        
        [Test]
        public void Test_GetSessionIdByVisualNumber_ReturnsNull()
        {
            //Arrange
            var sessionsInfoList = new List<SessionDTO>();
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