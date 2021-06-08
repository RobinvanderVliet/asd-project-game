using Moq;
using Network;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
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
            _sut = new AttackHandler(_mockedClientController.Object, _mockedWorldService.Object,
                _mockedPlayerPocoDatabaseService.Object,
                _mockedPlayerItemPocoDatabaseService.Object, _mockedCreaturePocoDatabaseService.Object,
                _mockedMessageService.Object);
            _packetDTO = new PacketDTO();
            _attackDTO = new AttackDTO();
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
        public void Test_HandlePacket_HandleAttack(String direction)
        {
            //Arrange
            string GameGuid = Guid.NewGuid().ToString();
            string PlayerGuid = Guid.NewGuid().ToString();
            string AttackedPlayerGuid = Guid.NewGuid().ToString();

            PlayerPOCO playerPOCO = new PlayerPOCO
            {
                PlayerGuid = PlayerGuid,
                Health = 100,
                Stamina = 100,
                GameGuid = null,
                XPosition = 10,
                YPosition = 20
            };

            Player player = new Player("Gert", 10, 20, "#", PlayerGuid);

            PlayerPOCO attackedPlayerPOCO = new PlayerPOCO
            {
                PlayerGuid = AttackedPlayerGuid,
                Health = 100,
                Stamina = 100,
                GameGuid = null,
                XPosition = 26,
                YPosition = 11
            };

            Player attackedPlayer = new Player("Henk", 26, 11, "E", AttackedPlayerGuid);


            _attackDTO.Damage = 20;
            _attackDTO.Stamina = 100;
            _attackDTO.PlayerGuid = PlayerGuid;
            _attackDTO.AttackedPlayerGuid = AttackedPlayerGuid;
            
            _mockedClientController.Setup(x => x.IsBackupHost).Returns(true);
            _mockedClientController.Setup(x => x.GetOriginId()).Returns(PlayerGuid);
            _mockedClientController.Object.SetSessionId(GameGuid);

            _mockedWorldService.Setup(mock => mock.GetPlayer(player.Id)).Returns(player);
            _mockedWorldService.Setup(mock => mock.GetPlayer(attackedPlayer.Id)).Returns(attackedPlayer);


            var payload = JsonConvert.SerializeObject(_attackDTO);
            _packetDTO.Payload = payload;
            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO();
            packetHeaderDTO.OriginID = PlayerGuid;
            packetHeaderDTO.SessionID = null;
            packetHeaderDTO.PacketType = PacketType.Attack;
            packetHeaderDTO.Target = "host";
            _packetDTO.Header = packetHeaderDTO;

            List<Player> list = new List<Player>();
            list.Add(player);
            list.Add(attackedPlayer);


            _mockedWorldService.Setup(x => x.GetAllPlayers()).Returns(list);

            List<PlayerPOCO> playerPOCOList = new();
            playerPOCOList.Add(attackedPlayerPOCO);
            playerPOCOList.Add(playerPOCO);
            IEnumerable<PlayerPOCO> en = playerPOCOList;
            var task = Task.FromResult(en);

            _mockedPlayerPocoDatabaseService.Setup(mock => mock.GetAllAsync())
                .Returns(task);


            //Act
            var actualResult = _sut.HandlePacket(_packetDTO);

            //Assert
            _mockedWorldService.Verify(mock => mock.DisplayWorld(), Times.Once);
        }
    }
}