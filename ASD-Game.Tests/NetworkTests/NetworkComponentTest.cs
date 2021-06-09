using System.Diagnostics.CodeAnalysis;
using ASD_Game.Network;
using ASD_Game.Network.DTO;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace ASD_Game.Tests.NetworkTests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    class NetworkComponentTest
    {
        //Declaration and initialisation of constant variables

        //Declaration of variables
        private NetworkComponent _sut;
        private PacketHeaderDTO _packetHeaderDTO;
        private PacketDTO _packetDTO;

        //Declaration of mocks
        private Mock<IPacketListener> _mockedHostController;
        private Mock<IPacketHandler> _mockedClientController;
        private Mock<IWebSocketConnection> _mockedWebSocketConnection;

        [SetUp]
        public void Setup()
        {
            _mockedHostController = new Mock<IPacketListener>();
            _mockedClientController = new Mock<IPacketHandler>();
            _mockedWebSocketConnection = new Mock<IWebSocketConnection>();
            _sut = new NetworkComponent(_mockedWebSocketConnection.Object);
            _sut.SetClientController(_mockedClientController.Object);
            _packetHeaderDTO = new PacketHeaderDTO();
            _packetDTO = new PacketDTO();
        }

        [Test]
        public void Test_ReceivePacket_SendPacketToHostControllerWithTargetHost()
        {
            //Arrange ---------
            _sut.SetHostController(_mockedHostController.Object);       
            _packetHeaderDTO.Target = "host";
            _packetDTO.Header = _packetHeaderDTO;
            _mockedHostController.Setup(mock => mock.ReceivePacket(_packetDTO));

            //Act ---------
            _sut.ReceivePacket(_packetDTO);

            //Assert ---------
            _mockedHostController.Verify(mock => mock.ReceivePacket(_packetDTO), Times.Once());
        }

        [Test]
        public void Test_ReceivePacket_DoNothingWhenNoHostControllerIsSetWithTargetHost()
        {
            //Arrange ---------
            _sut.SetHostController(null);
            _packetHeaderDTO.Target = "host";
            _packetDTO.Header = _packetHeaderDTO;
            _mockedHostController.Setup(mock => mock.ReceivePacket(_packetDTO));

            //Act ---------
            _sut.ReceivePacket(_packetDTO);

            //Assert ---------
            _mockedHostController.Verify(mock => mock.ReceivePacket(_packetDTO), Times.Never());
        }

        [Test]
        public void Test_ReceivePacket_SendPacketToClientControllerWithTargetClient()
        {
            //Arrange ---------
            _sut.SetClientController(_mockedClientController.Object);
            _packetHeaderDTO.Target = "client";
            _packetDTO.Header = _packetHeaderDTO;
            _mockedClientController.Setup(mock => mock.HandlePacket(_packetDTO));

            //Act ---------
            _sut.ReceivePacket(_packetDTO);

            //Assert ---------
            _mockedClientController.Verify(mock => mock.HandlePacket(_packetDTO), Times.Once());
        }

        [Test]
        public void Test_ReceivePacket_SendPacketToClientControllerWithTargetOriginID()
        {
            //Arrange ---------
            _sut.SetClientController(_mockedClientController.Object);
            _packetHeaderDTO.Target = _sut.GetOriginId();
            _packetDTO.Header = _packetHeaderDTO;
            _mockedClientController.Setup(mock => mock.HandlePacket(_packetDTO));

            //Act ---------
            _sut.ReceivePacket(_packetDTO);

            //Assert ---------
            _mockedClientController.Verify(mock => mock.HandlePacket(_packetDTO), Times.Once());
        }

        [Test]
        public void Test_ReceivePacket_DoNothingWhenNoClientControllerIsSetWithTargetClient()
        {
            //Arrange ---------
            _sut.SetClientController(null);
            _packetHeaderDTO.Target = "client";
            _packetDTO.Header = _packetHeaderDTO;
            _mockedClientController.Setup(mock => mock.HandlePacket(_packetDTO));

            //Act ---------
            _sut.ReceivePacket(_packetDTO);

            //Assert ---------
            _mockedClientController.Verify(mock => mock.HandlePacket(_packetDTO), Times.Never());
        }

        [Test]
        public void Test_ReceivePacket_DoNothingWhenNoClientControllerIsSetWithTargetOriginId()
        {
            //Arrange ---------
            _sut.SetClientController(null);
            _packetHeaderDTO.Target = _sut.GetOriginId();
            _packetDTO.Header = _packetHeaderDTO;
            _mockedClientController.Setup(mock => mock.HandlePacket(_packetDTO));

            //Act ---------
            _sut.ReceivePacket(_packetDTO);

            //Assert ---------
            _mockedClientController.Verify(mock => mock.HandlePacket(_packetDTO), Times.Never());
        }

        [Test]
        public void Test_SendPacket_SendsPacketToWebSocket()
        {
            //Arrange ---------
            _sut.SetWebSocketConnection(_mockedWebSocketConnection.Object);
            _packetHeaderDTO.OriginID = _sut.GetOriginId();
            _packetDTO.Header = _packetHeaderDTO;
            string serializedPacket = JsonConvert.SerializeObject(_packetDTO);
            _mockedWebSocketConnection.Setup(mock => mock.Send(serializedPacket));

            //Act ---------
            _sut.SendPacket(_packetDTO);

            //Assert ---------
            _mockedWebSocketConnection.Verify(mock => mock.Send(serializedPacket), Times.Once());
        }
    }
}
