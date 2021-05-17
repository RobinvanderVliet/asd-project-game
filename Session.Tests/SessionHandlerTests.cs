using Moq;
using Network;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace Session.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class SessionHandlerTests
    {
        //Declaration and initialisation of constant variables
        SessionHandler _sessionHandler;

        //Declaration of mocks
        private Mock<IClientController> _mockedClientController;

        [SetUp]
        public void Setup()
        {
            _mockedClientController = new Mock<IClientController>();
            _sessionHandler = new SessionHandler(_mockedClientController.Object);
        }

        [Test]
        public void Test_JoinSession()
        {
            Assert.Pass();
        }

        [Test]
        public void Test_CreateSession()
        {
            // Arrange ------------
            string testSessionName = "testSessionName";

            _mockedClientController.Setup(mock => mock.CreateHostController());

            // Act ----------------
            _sessionHandler.CreateSession(testSessionName);

            // Assert -------------
            _mockedClientController.Verify(mock => mock.CreateHostController(), Times.Once());
        }

        [Test]
        public void Test_RequestSessions()
        {
            // Arrange ------------
            string testSessionName = "testSessionName";

            _mockedClientController.Setup(mock => mock.CreateHostController());

            // Act ----------------
            _sessionHandler.CreateSession(testSessionName);

            // Assert -------------
            _mockedClientController.Verify(mock => mock.CreateHostController(), Times.Once());
        }

        [Test]
        public void Test_SendSessionDTO()
        {
            Assert.Pass();
        }

        [Test]
        public void Test_HandlePacket()
        {
            Assert.Pass();
        }
    }
}