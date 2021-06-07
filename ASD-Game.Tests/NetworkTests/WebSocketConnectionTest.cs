using System.Diagnostics.CodeAnalysis;
using System.Threading;
using ASD_Game.Network;
using ASD_Game.Network.DTO;
using ASD_Game.Network.Enum;
using Newtonsoft.Json;
using NUnit.Framework;

namespace ASD_Game.Tests.NetworkTests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class WebSocketConnectionTest
    {
        //Tests in this class are dependent on a working websocket.
        //Therefore these tests should not be regarded as unit tests but rather as integration tests.

        private WebSocketConnection _receivingSut;
        private WebSocketConnection _sendingSut;
        private TestListener _mockedPacketListener;
        private const string SESSION_ID = "testing";

        [SetUp]
        public void Setup()
        {
            _mockedPacketListener = new TestListener();
            _receivingSut = new WebSocketConnection(_mockedPacketListener);
            _sendingSut = new WebSocketConnection(_mockedPacketListener);
            Thread.Sleep(500);
        }

        private class TestListener : IPacketListener
        {
            private PacketDTO _packetDTO;
            
            public void ReceivePacket(PacketDTO packet)
            {
                if (packet.Header.SessionID.Equals(SESSION_ID))
                {
                    _packetDTO = packet;
                }
            }

            public PacketDTO GetPacketDTO()
            {
                return _packetDTO;
            }
        }

        [Test]
        public void Test_Send_IsBeingReceived()
        {
            // Arrange
            PacketDTO packetToSend = new PacketBuilder()
                .SetTarget("client")
                .SetSessionID(SESSION_ID)
                .SetPayload("testPayload")
                .SetPacketType(PacketType.Chat)
                .SetOriginID("originId")
                .Build();
            string serializedPacketToSend = JsonConvert.SerializeObject(packetToSend);
            PacketDTO receivedPacket = null;

            // Act
            _sendingSut.Send(serializedPacketToSend);
            Thread.Sleep(300);

            // Assert
            receivedPacket = _mockedPacketListener.GetPacketDTO();
            Assert.AreEqual(packetToSend, receivedPacket);
        }
    }
}