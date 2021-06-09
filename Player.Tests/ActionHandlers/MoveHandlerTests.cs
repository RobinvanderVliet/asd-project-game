using System;
using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Chat;
using Chat.DTO;
using DataTransfer.DTO.Character;
using Moq;
using Network;
using Network.DTO;
using Newtonsoft.Json;
using NUnit.Framework;
using Player.ActionHandlers;
using Player.DTO;
using WorldGeneration;

namespace Player.Tests.ActionHandlers
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    
    public class MoveHandlerTests
    {
        //Declaration and initialisation of constant variables

        //Declaration of variables
        private MoveHandler _sut;
        private PacketDTO _packetDTO;
        private MoveDTO _moveDTO;
        private MapCharacterDTO _mapCharacterDTO;

        //Declaration of mocks
        private Mock<IClientController> _mockedClientController;
        private Mock<IMapper> _mockedMapper;
        private Mock<IWorldService> _mockedWorldService;
        private Mock<INetworkComponent> _mockedNetworkComponent;

        [SetUp]
        public void Setup()
        {
            _mockedClientController = new Mock<IClientController>();
            _mockedMapper = new Mock<IMapper>();
            _mockedWorldService = new Mock<IWorldService>();
            _sut = new MoveHandler(_mockedMapper.Object,_mockedClientController.Object, _mockedWorldService.Object);
            _packetDTO = new PacketDTO();
            _mockedNetworkComponent = new Mock<INetworkComponent>();
        }

        [Test]
        public void Test_SendMove_SendsTheMessageAndPacketTypeToClientController()
        {
            //Arrange ---------
            string playerGuid = new Guid().ToString();
            string GameGuid = new Guid().ToString();
            _mapCharacterDTO = new MapCharacterDTO(10,10, playerGuid, GameGuid);
            _moveDTO = new MoveDTO(_mapCharacterDTO);
            var payload = JsonConvert.SerializeObject(_moveDTO);
            _mockedClientController.Setup(mock => mock.SendPayload(payload, PacketType.Move));

            //Act ---------
            _sut.SendMove(_mapCharacterDTO);

            //Assert ---------
            _mockedClientController.Verify(mock => mock.SendPayload(payload, PacketType.Move), Times.Once());
        }

        [Test]
        public void Test_HandlePacket_HandleMoveProperly()
        {
            //Arrange ---------
            string playerGuid = new Guid().ToString();
            string GameGuid = new Guid().ToString();

            _mapCharacterDTO = new MapCharacterDTO(10, 10, playerGuid, GameGuid);
            _moveDTO = new MoveDTO(_mapCharacterDTO);
            var payload = JsonConvert.SerializeObject(_moveDTO);
            _packetDTO.Payload = payload;
            _packetDTO.Header.Target = playerGuid;

            //Act ---------
            HandlerResponseDTO actualResult = _sut.HandlePacket(_packetDTO);

            //Assert ---------
            HandlerResponseDTO ExpectedResult = new HandlerResponseDTO(SendAction.SendToClients, null);
            Assert.AreEqual(ExpectedResult, actualResult);
        }
    }
}