using Moq;
using Network;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ActionHandling.DTO;
using DatabaseHandler.POCO;
using DatabaseHandler.Services;
using Messages;
using Network.DTO;
using Session;
using WorldGeneration;

namespace ActionHandling.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class AttackHandlerTests
    {
        //Declaration and initialisation of constant variables
        //Declaration of variables
        private AttackHandler _sut;
        private AttackDTO _attackDTO;

        private PacketDTO _packetDTO;

        //Declaration of mocks
        private Mock<IClientController> _mockedClientController;
        private Mock<IWorldService> _mockedWorldService;
        private Mock<Player> _mockedPlayer;
        private Mock<List<Player>> _mockedPlayers;
        private Mock<IDatabaseService<PlayerPOCO>> _mockedPlayerPocoDatabaseService;
        private Mock<IDatabaseService<PlayerItemPOCO>> _mockedPlayerItemPocoDatabaseService;
        private Mock<IDatabaseService<CreaturePOCO>> _mockedCreaturePocoDatabaseService;
        private Mock<ISessionHandler> _mockedSessionHandler;
        private Mock<IMessageService> _mockedMessageService;

        [SetUp]
        public void Setup()
        {
            _mockedClientController = new Mock<IClientController>();
            _mockedWorldService = new Mock<IWorldService>();
            _mockedPlayerPocoDatabaseService = new Mock<IDatabaseService<PlayerPOCO>>();
            _mockedPlayerItemPocoDatabaseService = new Mock<IDatabaseService<PlayerItemPOCO>>();
            _mockedCreaturePocoDatabaseService = new Mock<IDatabaseService<CreaturePOCO>>();
            _mockedMessageService = new Mock<IMessageService>();
            _sut = new AttackHandler(_mockedClientController.Object, _mockedWorldService.Object, _mockedPlayerPocoDatabaseService.Object,
                _mockedPlayerItemPocoDatabaseService.Object, _mockedCreaturePocoDatabaseService.Object,
                _mockedMessageService.Object);
        }

        [TestCase("up")]
        [TestCase("down")]
        [TestCase("left")]
        [TestCase("right")]
        [Test]
        public void Test_SendAttack_SendsTheMessageAndPacketTypeToClientController(String direction)
        {
            //Arrange
            int x = 26;
            int y = 11;
            string PlayerGuid = Guid.NewGuid().ToString();
            //string AttackGuid = Guid.NewGuid().ToString();
            Player player = new Player("test", x, y, "#", PlayerGuid);

            _mockedWorldService.Setup(WorldService => WorldService.GetCurrentPlayer())
                .Returns(player);
            _attackDTO = new AttackDTO();
            _attackDTO.XPosition = 26;
            _attackDTO.YPosition = 11;
            _attackDTO.Damage = 20;
            _attackDTO.PlayerGuid = PlayerGuid;
            //attackDTO.AttackedPlayerGuid = AttackGuid;

            _mockedClientController.Setup(ClientController => ClientController.GetOriginId())
                .Returns(PlayerGuid);
            var payload = JsonConvert.SerializeObject(_attackDTO);

            _mockedClientController.Setup(mock => mock.SendPayload(payload, PacketType.Attack));
            //Act ---------
            _sut.SendAttack(direction);
            //Assert ---------
            _mockedClientController.Verify(mock => mock.SendPayload(It.IsAny<String>(), PacketType.Attack),
                Times.Once());
        }


        [TestCase("up")]
        [TestCase("down")]
        [TestCase("left")]
        [TestCase("right")]
        [Test]
        public void TestHandlePacket_HandleAttack(String direction)
        {
            //Arrange
            string GameGuid = Guid.NewGuid().ToString();
            string PlayerGuid = Guid.NewGuid().ToString();
            string AttackedPlayerGuid = Guid.NewGuid().ToString();

            List<string> allClients = new List<string>();
            allClients.Add("a");
            allClients.Add("b");

            Dictionary<string, int[]> players = new Dictionary<string, int[]>();


            int playerX = 26; // spawn position
            int playerY = 11; // spawn position
            foreach (string element in allClients)
            {
                int[] playerPosition = new int[2];
                playerPosition[0] = playerX;
                playerPosition[1] = playerY;
                players.Add(element, playerPosition);
                playerX += 2; // spawn position + 2 each client
                playerY += 2; // spawn position + 2 each client
            }


            PlayerPOCO player = new PlayerPOCO
            {
                PlayerGuid = PlayerGuid,
                Health = 100,
                Stamina = 100,
                GameGuid = GameGuid,
                XPosition = 10,
                YPosition = 20
            };

            PlayerPOCO attackedPlayer = new PlayerPOCO
            {
                PlayerGuid = AttackedPlayerGuid,
                Health = 100,
                Stamina = 100,
                GameGuid = GameGuid,
                XPosition = 10,
                YPosition = 21
            };

            AttackDTO attackDto = new AttackDTO();
            var attackDTO = new AttackDTO();
            attackDTO.Damage = 20;
            attackDTO.Stamina = 100;
            attackDTO.PlayerGuid = PlayerGuid;
            

            PacketDTO packetDto = new PacketDTO();
            packetDto.Header.Target = "host";
            packetDto.Header.PacketType = PacketType.Attack;
            packetDto.Header.OriginID = PlayerGuid;
            packetDto.Header.SessionID = GameGuid;
            packetDto.Payload = JsonConvert.SerializeObject(attackDto);

            attackDto = JsonConvert.DeserializeObject<AttackDTO>(packetDto.Payload);
            _mockedClientController.Setup(x => x.IsBackupHost).Returns(true);
            
           
            //_packetDTO.Payload = payload;
            // PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO();
            // packetHeaderDTO.OriginID = PlayerGuid;
            // packetHeaderDTO.SessionID = null;
            // packetHeaderDTO.PacketType = PacketType.Attack;
            // packetHeaderDTO.Target = "host";
            // _packetDTO.Header = packetHeaderDTO;


            //Act
            var actualResult = _sut.HandlePacket(_packetDTO);

           // payload = JsonConvert.SerializeObject(_attackDTO);

//            _mockedClientController.Setup(mock => mock.SendPayload(payload, PacketType.Attack));


            //Assert
            HandlerResponseDTO ExpectedResult = new HandlerResponseDTO(SendAction.SendToClients, null);
            Assert.AreEqual(ExpectedResult, actualResult);
        }


        //     [Test]
        // public void Test_HandlePacket_HandleMoveProperly()
        // {
        //     //Arrange ---------
        //     string playerGuid = new Guid().ToString();
        //     string GameGuid = new Guid().ToString();
        //     //Arrange ---------
        //     _mapCharacterDTO = new MapCharacterDTO(10, 10, playerGuid, GameGuid);
        //     _moveDTO = new MoveDTO(_mapCharacterDTO);
        //     var payload = JsonConvert.SerializeObject(_moveDTO);
        //     _packetDTO.Payload = payload;
        //     _packetDTO.Header.Target = playerGuid;
        //     //_mockedNetworkComponent.
        //     {
        //         //Act ---------
        //         HandlerResponseDTO actualResult = _sut.HandlePacket(_packetDTO);
        //
        //         //Assert ---------
        //         HandlerResponseDTO ExpectedResult = new HandlerResponseDTO(SendAction.SendToClients, null);
        //         Assert.AreEqual(ExpectedResult, actualResult);
        //     }
        // }
    }
}