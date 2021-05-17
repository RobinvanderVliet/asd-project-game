using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using Network;
using Network.DTO;
using Moq;
using Newtonsoft.Json;

namespace Network.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    class HostControllerTest
    {
        private HostController _hostController;
        private Mock<INetworkComponent> _mockedNetworkComponent;
        private Mock<IPacketHandler> _mockedClientController;
        private string _sessionId = "TestSession";
        private PacketHeaderDTO _packetHeaderDTO;
        private HandlerResponseDTO _handlerResponseDTO;
        private PacketDTO _packetDTO;
       

        [SetUp]
        public void Setup()
        {
            _mockedNetworkComponent = new Mock<INetworkComponent>();
            _mockedClientController = new Mock<IPacketHandler>();
            _hostController = new HostController(_mockedNetworkComponent.Object, _mockedClientController.Object, _sessionId);
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
            _hostController.ReceivePacket(_packetDTO);

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
            _hostController.ReceivePacket(_packetDTO);

            //Assert ---------
            _mockedNetworkComponent.Verify(mock => mock.SendPacket(_packetDTO));
        }
    }
}
