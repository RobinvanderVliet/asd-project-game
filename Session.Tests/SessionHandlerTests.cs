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
        private Mock<Session> _session;


        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}