using System.Diagnostics.CodeAnalysis;
using ASD_Game.Network;
using ASD_Game.Network.DTO;
using ASD_Game.Network.Enum;
using Moq;
using NUnit.Framework;

namespace ASD_Game.Tests.NetworkTests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    class HostControllerTest
    {
        //Declaration and initialisation of constant variables

        //Declaration of variables
        private HostController _sut;
        private string _sessionId = "TestSession";
        private PacketHeaderDTO _packetHeaderDTO;
        private HandlerResponseDTO _handlerResponseDTO;
        private PacketDTO _packetDTO;

        //Declaration of mocks
        private Mock<INetworkComponent> _mockedNetworkComponent;
        private Mock<IPacketHandler> _mockedClientController;

        [SetUp]
        public void Setup()
        {
            _mockedNetworkComponent = new Mock<INetworkComponent>();
            _mockedClientController = new Mock<IPacketHandler>();
            _sut = new HostController(_mockedNetworkComponent.Object, _mockedClientController.Object, _sessionId);
            _packetHeaderDTO = new PacketHeaderDTO();
            _packetDTO = new PacketDTO();
        }

        [Test]
        public void Test_ReceivePacket_SendPacketToClients()
        {
            //Arrange ---------
            _packetHeaderDTO.SessionID = "TestSession";          
            _handlerResponseDTO = new HandlerResponseDTO(SendAction.SendToClients, null);
            _packetDTO.Header = _packetHeaderDTO;           
            _mockedClientController.Setup(mock => mock.HandlePacket(_packetDTO)).Returns(_handlerResponseDTO);
            _packetHeaderDTO.Target = "client";
            _mockedNetworkComponent.Setup(mock => mock.SendPacket(_packetDTO));

            //Act ---------
            _sut.ReceivePacket(_packetDTO);

            //Assert ---------
            _mockedNetworkComponent.Verify(mock => mock.SendPacket(_packetDTO));
        }

        [Test]
        public void Test_ReceivePacket_SendPacketToOriginId()
        {
            //Arrange ---------
            _packetHeaderDTO.SessionID = "TestSession";           
            _packetHeaderDTO.OriginID = "TestOriginId";
            _handlerResponseDTO = new HandlerResponseDTO(SendAction.ReturnToSender, null);
            _packetDTO.Header = _packetHeaderDTO;
            _mockedClientController.Setup(mock => mock.HandlePacket(_packetDTO)).Returns(_handlerResponseDTO);
            _mockedNetworkComponent.Setup(mock => mock.SendPacket(_packetDTO));
            //Act ---------
            _sut.ReceivePacket(_packetDTO);

            //Assert ---------
            _mockedNetworkComponent.Verify(mock => mock.SendPacket(_packetDTO));
        }

        [Test]
        public void Test_ReceivePacket_NotSameSessionIdAndNotPacketTypeSession()
        {
            //Arrange ---------
            _packetHeaderDTO.SessionID = "NotTestSession";
            _packetHeaderDTO.Target = "host";
            _packetHeaderDTO.PacketType = PacketType.Chat;
            _packetDTO.Header = _packetHeaderDTO;
            _handlerResponseDTO = new HandlerResponseDTO(SendAction.SendToClients, null);
            _mockedClientController.Setup(mock => mock.HandlePacket(_packetDTO)).Returns(_handlerResponseDTO);
            _mockedNetworkComponent.Setup(mock => mock.SendPacket(_packetDTO));

            //Act ---------
            _sut.ReceivePacket(_packetDTO);

            //Assert ---------
            _mockedClientController.Verify(mock => mock.HandlePacket(_packetDTO), Times.Never);
            _mockedNetworkComponent.Verify(mock => mock.SendPacket(_packetDTO), Times.Never);
        }
    }
}
