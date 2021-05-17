using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using Network;
using Moq;
using Newtonsoft.Json;

namespace Network.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    class NetworkComponentTest
    {
        //Declaration and initialisation of constant variables

        //Declaration of variables
        private NetworkComponent _networkComponent;
        private PacketHeaderDTO _packetHeaderDTO;
        private PacketDTO _packetDTO;

        //Declaration of mocks
        private Mock<IPacketListener> _mockedHostController;
        private Mock<IPacketHandler> _mockedClientController;
        private Mock<IWebSocketConnection> _mockedWebSocketConnection;

        [SetUp]
        public void Setup()
        {
            _networkComponent = new NetworkComponent();
            _mockedHostController = new Mock<IPacketListener>();
            _mockedClientController = new Mock<IPacketHandler>();
            _mockedWebSocketConnection = new Mock<IWebSocketConnection>();
            _packetHeaderDTO = new PacketHeaderDTO();
            _packetDTO = new PacketDTO();
        }

        [Test]
        public void Test_ReceivePacket_SendPacketToHostControllerWithTargetHost()
        {
            //Arrange ---------
            _networkComponent.SetHostController(_mockedHostController.Object);       
            _packetHeaderDTO.Target = "host";
            _packetDTO.Header = _packetHeaderDTO;
            _mockedHostController.Setup(mock => mock.ReceivePacket(_packetDTO));

            //Act ---------
            _networkComponent.ReceivePacket(_packetDTO);

            //Assert ---------
            _mockedHostController.Verify(mock => mock.ReceivePacket(_packetDTO), Times.Once());
        }
        [Test]
        public void Test_ReceivePacket_DoNothingWhenNoHostControllerIsSetWithTargetHost()
        {
            //Arrange ---------
            _networkComponent.SetHostController(null);
            _packetHeaderDTO.Target = "host";
            _packetDTO.Header = _packetHeaderDTO;
            _mockedHostController.Setup(mock => mock.ReceivePacket(_packetDTO));

            //Act ---------
            _networkComponent.ReceivePacket(_packetDTO);

            //Assert ---------
            _mockedHostController.Verify(mock => mock.ReceivePacket(_packetDTO), Times.Never());
        }
        [Test]
        public void Test_ReceivePacket_SendPacketToClientControllerWithTargetClient()
        {
            //Arrange ---------
            _networkComponent.SetClientController(_mockedClientController.Object);
            _packetHeaderDTO.Target = "client";
            _packetDTO.Header = _packetHeaderDTO;
            _mockedClientController.Setup(mock => mock.HandlePacket(_packetDTO));

            //Act ---------
            _networkComponent.ReceivePacket(_packetDTO);

            //Assert ---------
            _mockedClientController.Verify(mock => mock.HandlePacket(_packetDTO), Times.Once());
        }

        [Test]
        public void Test_ReceivePacket_SendPacketToClientControllerWithTargetOriginID()
        {
            //Arrange ---------
            _networkComponent.SetClientController(_mockedClientController.Object);
            _packetHeaderDTO.Target = _networkComponent.GetOriginId();
            _packetDTO.Header = _packetHeaderDTO;
            _mockedClientController.Setup(mock => mock.HandlePacket(_packetDTO));

            //Act ---------
            _networkComponent.ReceivePacket(_packetDTO);

            //Assert ---------
            _mockedClientController.Verify(mock => mock.HandlePacket(_packetDTO), Times.Once());
        }

        [Test]
        public void Test_ReceivePacket_DoNothingWhenNoClientControllerIsSetWithTargetClient()
        {
            //Arrange ---------
            _networkComponent.SetClientController(null);
            _packetHeaderDTO.Target = "client";
            _packetDTO.Header = _packetHeaderDTO;
            _mockedClientController.Setup(mock => mock.HandlePacket(_packetDTO));

            //Act ---------
            _networkComponent.ReceivePacket(_packetDTO);

            //Assert ---------
            _mockedClientController.Verify(mock => mock.HandlePacket(_packetDTO), Times.Never());
        }

        [Test]
        public void Test_ReceivePacket_DoNothingWhenNoClientControllerIsSetWithTargetOriginId()
        {
            //Arrange ---------
            _networkComponent.SetClientController(null);
            _packetHeaderDTO.Target = _networkComponent.GetOriginId();
            _packetDTO.Header = _packetHeaderDTO;
            _mockedClientController.Setup(mock => mock.HandlePacket(_packetDTO));

            //Act ---------
            _networkComponent.ReceivePacket(_packetDTO);

            //Assert ---------
            _mockedClientController.Verify(mock => mock.HandlePacket(_packetDTO), Times.Never());
        }

        [Test]
        public void Test_SendPacket_SendsPacketToWebSocket()
        {
            //Arrange ---------
            _networkComponent.SetWebSocketConnection(_mockedWebSocketConnection.Object);
            _packetHeaderDTO.OriginID = _networkComponent.GetOriginId();
            _packetDTO.Header = _packetHeaderDTO;
            string serializedPacket = JsonConvert.SerializeObject(_packetDTO);
            _mockedWebSocketConnection.Setup(mock => mock.Send(serializedPacket));

            //Act ---------
            _networkComponent.SendPacket(_packetDTO);

            //Assert ---------
            _mockedWebSocketConnection.Verify(mock => mock.Send(serializedPacket), Times.Once());
        }
    }
}
