using NUnit.Framework;
using Moq;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using System.Threading;

namespace Network.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class WebSocketConnectionTest
    {
        //Tests in this class are dependent on a working websocket.
        //Therefore these tests should not be regarded as unit tests but rather as integration tests.

        private WebSocketConnection _receivingSut;
        private WebSocketConnection _sendingSut;
        private Mock<IPacketListener> _mockedPacketListener;

        [SetUp]
        public void Setup()
        {
            _mockedPacketListener = new Mock<IPacketListener>();
            _receivingSut = new WebSocketConnection(_mockedPacketListener.Object);
            _sendingSut = new WebSocketConnection(_mockedPacketListener.Object);
            Thread.Sleep(500);
        }



        [Test]
        public void Test_Send_IsBeingReceived()
        {
            // Arrange
            PacketDTO packetToSend = new PacketBuilder().SetTarget("client").SetPayload("testPayload").SetPacketType(PacketType.Chat).SetOriginID("originId").Build();
            string serializedPacketToSend = JsonConvert.SerializeObject(packetToSend);

            PacketDTO receivedPacket = null;
            _mockedPacketListener.Setup(mock => mock.ReceivePacket(It.IsAny<PacketDTO>())).Callback<PacketDTO>(r => receivedPacket = r);

            // Act
            _sendingSut.Send(serializedPacketToSend);
            Thread.Sleep(300);

            // Assert
            Assert.AreEqual(packetToSend, receivedPacket);
        }
    }
}