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
        private Mock<ClientController> _mockedClientController;
        private Mock<Session> _session;

        [SetUp]
        public void Setup()
        {
            _mockedClientController = new Mock<ClientController>();
            _session = new Mock<Session>();
        }

        [Test]
        public void Test_JoinSession()
        {
            Assert.Pass();
        }

        [Test]
        public void Test_CreateSession()
        {
            Assert.Pass();
        }

        [Test]
        public void Test_RequestSessions()
        {
            Assert.Pass();
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