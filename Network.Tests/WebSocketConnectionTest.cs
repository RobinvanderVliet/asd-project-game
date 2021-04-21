using NUnit.Framework;
using Moq;
using WebSocketSharp;

namespace Network.Tests
{
    [TestFixture]
    public class WebSocketConnectionTest
    {
        private WebSocketConnection _subject;
        private WebSocket _mockedWebsocket;
        private Mock<WebSocket> _websocketMock;

        [SetUp]
        public void Setup()
        {
            _websocketMock = new Mock<WebSocket>("ws://localhost:9999");
            _mockedWebsocket = _websocketMock.Object;
            _subject = new WebSocketConnection(_mockedWebsocket); 
        }



        [Test]
        public void SendTest1()
        {
            // Arrange
            string messageTest = "test";
            Mock.Get(_mockedWebsocket).Setup(x => x.Send(It.IsAny<string>()));

            // Act
            _subject.Send(messageTest);

            // Assert
            Mock.Get(_mockedWebsocket).Verify(x => x.Send(messageTest), Times.Once);
        }
    }
}