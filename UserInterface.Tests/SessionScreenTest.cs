using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace UserInterface.Tests
{
    [ExcludeFromCodeCoverage]
    public class SessionScreenTest
    {
        private SessionScreen _sut;
        [SetUp]
        public void Setup()
        {
            _sut = new SessionScreen();
        }

        [Test]
        public void Test_UpdateWithNewSession_AddsNewSession()
        {
            //Arrange
            var sessionInfo = new[] {"1", "TestSession", "Gerrit", "2"};

            //Act
            _sut.UpdateWithNewSession(sessionInfo);
            
            //Assert
            Assert.True(_sut.SessionInfoList.Count == 1);
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
    }
}