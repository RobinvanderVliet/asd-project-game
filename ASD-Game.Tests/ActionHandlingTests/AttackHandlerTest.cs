using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using ActionHandling.DTO;
using ASD_Game.ActionHandling;
using ASD_Game.DatabaseHandler.POCO;
using ASD_Game.DatabaseHandler.Services;
using ASD_Game.Items;
using ASD_Game.Messages;
using ASD_Game.Network;
using ASD_Game.Network.DTO;
using ASD_Game.Network.Enum;
using ASD_Game.Session;
using ASD_Game.World.Models.Characters;
using ASD_Game.World.Services;
using DatabaseHandler.POCO;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace ASD_Game.Tests.ActionHandlingTests
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

            _mockedClientController.Setup(ClientController => ClientController.GetOriginId())
                .Returns(PlayerGuid);
            var payload = JsonConvert.SerializeObject(_attackDTO);

            _mockedClientController.Setup(mock => mock.SendPayload(payload, PacketType.Attack));

            //Act

            _sut.SendAttack(direction);

            //Assert

            _mockedClientController.Verify(mock => mock.SendPayload(It.IsAny<String>(), PacketType.Attack),
                Times.Once());
            _mockedClientController.Verify(ClientController => ClientController.GetOriginId(), Times.Once);
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
                PlayerGUID = PlayerGuid,
                Health = 100,
                Stamina = 100,
                GameGUID = null,
                XPosition = 10,
                YPosition = 20
            };

            Player player = new Player("Gert", 10, 20, "#", PlayerGuid);

            PlayerPOCO attackedPlayerPOCO = new PlayerPOCO
            {
                PlayerGUID = AttackedPlayerGuid,
                Health = 100,
                Stamina = 100,
                GameGUID = null,
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


            var payload = JsonConvert.SerializeObject(_attackDTO);
            _packetDTO.Payload = payload;
            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO();
            packetHeaderDTO.OriginID = PlayerGuid;
            packetHeaderDTO.SessionID = null;
            packetHeaderDTO.PacketType = PacketType.Attack;
            packetHeaderDTO.Target = "host";
            _packetDTO.Header = packetHeaderDTO;

            Monster monster = new Monster("zombie", 10, 20, "T", "monst1");

            List<Character> characters = new List<Character>();
            characters.Add(player);
            characters.Add(attackedPlayer);
            characters.Add(monster);

            _mockedWorldService.Setup(x => x.GetAllCharacters()).Returns(characters);

            List<PlayerPOCO> playerPOCOList = new();
            playerPOCOList.Add(attackedPlayerPOCO);
            playerPOCOList.Add(playerPOCO);
            IEnumerable<PlayerPOCO> en = playerPOCOList;
            var task = Task.FromResult(en);

            _mockedPlayerPocoDatabaseService.Setup(mock => mock.GetAllAsync())
                .Returns(task);
            _mockedPlayerPocoDatabaseService.Setup(mock => mock.UpdateAsync(playerPOCO));

            var expectedResult = new HandlerResponseDTO(SendAction.ReturnToSender,
                "There is no enemy to attack");

            //Act

            var actualResult = _sut.HandlePacket(_packetDTO);

            //Assert

            _mockedClientController.Verify(x => x.IsBackupHost, Times.Once);
            _mockedClientController.Verify(x => x.GetOriginId(), Times.Exactly(2));
            _mockedWorldService.Verify(mock => mock.GetPlayer(player.Id), Times.Exactly(3));
            _mockedWorldService.Verify(x => x.GetAllCharacters(), Times.Once());
            _mockedPlayerPocoDatabaseService.Verify(mock => mock.GetAllAsync(), Times.Once);
            _mockedPlayerPocoDatabaseService.Verify(mock => mock.UpdateAsync(playerPOCO), Times.Once);


            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestCase("up")]
        [TestCase("down")]
        [TestCase("left")]
        [TestCase("right")]
        [Test]
        public void Test_HandlePacket_HandleAttack_PlayerIsAlreadyDead(String direction)
        {
            //Arrange

            string GameGuid = Guid.NewGuid().ToString();
            string PlayerGuid = Guid.NewGuid().ToString();
            string AttackedPlayerGuid = Guid.NewGuid().ToString();
            PlayerPOCO playerPOCO = new PlayerPOCO
            {
                PlayerGUID = PlayerGuid,
                Health = 100,
                Stamina = 100,
                GameGUID = null,
                XPosition = 10,
                YPosition = 20
            };

            Player player = new Player("Gert", 10, 20, "#", PlayerGuid);
            PlayerPOCO attackedPlayerPOCO = new PlayerPOCO
            {
                PlayerGUID = AttackedPlayerGuid,
                Health = 0,
                Stamina = 100,
                GameGUID = null,
                XPosition = 26,
                YPosition = 11
            };

            Player attackedPlayer = new Player("Henk", 26, 11, "E", AttackedPlayerGuid);
            attackedPlayer.Health = 0;

            _attackDTO.Damage = 20;
            _attackDTO.Stamina = 100;
            _attackDTO.PlayerGuid = PlayerGuid;
            _attackDTO.AttackedPlayerGuid = AttackedPlayerGuid;
            _attackDTO.XPosition = 26;
            _attackDTO.YPosition = 11;
            _mockedClientController.Setup(x => x.IsBackupHost).Returns(true);
            _mockedClientController.Setup(x => x.GetOriginId()).Returns(PlayerGuid);
            _mockedClientController.Object.SetSessionId(GameGuid);

            var payload = JsonConvert.SerializeObject(_attackDTO);
            _packetDTO.Payload = payload;
            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO();
            packetHeaderDTO.OriginID = PlayerGuid;
            packetHeaderDTO.SessionID = null;
            packetHeaderDTO.PacketType = PacketType.Attack;
            packetHeaderDTO.Target = "host";
            _packetDTO.Header = packetHeaderDTO;

            List<Character> characters = new List<Character>();
            characters.Add(player);
            characters.Add(attackedPlayer);

            _mockedWorldService.Setup(x => x.GetAllCharacters()).Returns(characters);

            List<PlayerPOCO> playerPOCOList = new();
            playerPOCOList.Add(attackedPlayerPOCO);
            playerPOCOList.Add(playerPOCO);
            IEnumerable<PlayerPOCO> en = playerPOCOList;
            var task = Task.FromResult(en);

            _mockedPlayerPocoDatabaseService.Setup(mock => mock.GetAllAsync())
                .Returns(task);
            _mockedPlayerPocoDatabaseService.Setup(mock => mock.UpdateAsync(playerPOCO));

            _mockedWorldService.Setup(x => x.GetPlayer(PlayerGuid)).Returns(player);

            var expectedResult = new HandlerResponseDTO(SendAction.Ignore, null);

            //Act

            var actualResult = _sut.HandlePacket(_packetDTO);

            //Assert

            _mockedMessageService.Verify(mock => mock.AddMessage("You can't attack this enemy, he is already dead."),
                Times.Once);
            _mockedClientController.Verify(x => x.IsBackupHost, Times.Once);
            _mockedClientController.Verify(x => x.GetOriginId(), Times.Exactly(2));

            _mockedWorldService.Verify(x => x.GetPlayer(PlayerGuid), Times.Exactly(3));

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestCase("down")]
        [TestCase("up")]
        [TestCase("left")]
        [TestCase("right")]
        [Test]
        public void Test_HandlePacket_HandleAttack_TakesDamageDown(String direction)
        {
            //Arrange

            string GameGuid = Guid.NewGuid().ToString();
            string PlayerGuid = Guid.NewGuid().ToString();
            string AttackedPlayerGuid = Guid.NewGuid().ToString();

            PlayerPOCO playerPOCO = new PlayerPOCO
            {
                PlayerGUID = PlayerGuid,
                Health = 100,
                Stamina = 100,
                GameGUID = null,
                XPosition = 10,
                YPosition = 20
            };

            Player player = new Player("Gert", 10, 20, "#", PlayerGuid);
            player.Inventory.Armor = ItemFactory.GetJacket();
            PlayerPOCO attackedPlayerPOCO = new PlayerPOCO
            {
                PlayerGUID = AttackedPlayerGuid,
                Health = 0,
                Stamina = 100,
                GameGUID = null,
                XPosition = 26,
                YPosition = 11
            };

            Player attackedPlayer = new Player("Henk", 26, 11, "E", AttackedPlayerGuid);

            _attackDTO.Damage = 20;
            _attackDTO.Stamina = 100;
            _attackDTO.PlayerGuid = PlayerGuid;
            _attackDTO.AttackedPlayerGuid = AttackedPlayerGuid;
            _attackDTO.XPosition = 26;
            _attackDTO.YPosition = 11;

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

            List<Player> players = new List<Player>();
            players.Add(player);
            players.Add(attackedPlayer);

            Monster monster = new Monster("zombie", 10, 20, "T", "monst1");

            List<Character> characters = new List<Character>();
            characters.Add(player);
            characters.Add(attackedPlayer);
            characters.Add(monster);

            _mockedWorldService.Setup(x => x.GetAllCharacters()).Returns(characters);

            List<PlayerPOCO> playerPOCOList = new();
            playerPOCOList.Add(attackedPlayerPOCO);
            playerPOCOList.Add(playerPOCO);
            IEnumerable<PlayerPOCO> en = playerPOCOList;
            var task = Task.FromResult(en);

            _mockedPlayerPocoDatabaseService.Setup(mock => mock.GetAllAsync())
                .Returns(task);
            _mockedPlayerPocoDatabaseService.Setup(mock => mock.UpdateAsync(attackedPlayerPOCO));
            _mockedPlayerPocoDatabaseService.Setup(mock => mock.UpdateAsync(playerPOCO));

            PlayerItemPOCO playerItemPOCO = new PlayerItemPOCO()
            {
                PlayerGUID = PlayerGuid,
                GameGUID = null,
                ItemName = "Bandana",
                ArmorPoints = 1
            };
            PlayerItemPOCO playerArmorPOCO = new PlayerItemPOCO()
            {
                PlayerGUID = PlayerGuid,
                GameGUID = null,
                ItemName = "Jacket",
                ArmorPoints = 20
            };

            List<PlayerItemPOCO> playerItemPOCOList = new();
            playerItemPOCOList.Add(playerItemPOCO);
            playerItemPOCOList.Add(playerArmorPOCO);
            IEnumerable<PlayerItemPOCO> enItem = playerItemPOCOList;
            var taskItem = Task.FromResult(enItem);

            _mockedPlayerItemPocoDatabaseService.Setup(mock => mock.GetAllAsync())
                .Returns(taskItem);
            _mockedPlayerItemPocoDatabaseService.Setup(mock => mock.UpdateAsync(playerItemPOCO));
            _mockedPlayerItemPocoDatabaseService.Setup(mock => mock.UpdateAsync(playerArmorPOCO));
            _mockedWorldService.Setup(mock => mock.GetAllPlayers()).Returns(players);

            var expectedResult = new HandlerResponseDTO(SendAction.SendToClients, null);

            //Act

            var actualResult = _sut.HandlePacket(_packetDTO);

            //Assert

            _mockedClientController.Verify(x => x.IsBackupHost, Times.Once);
            _mockedClientController.Verify(x => x.GetOriginId(), Times.Exactly(3));
            _mockedWorldService.Verify(mock => mock.GetPlayer(player.Id), Times.Exactly(4));
            _mockedWorldService.Verify(mock => mock.GetPlayer(attackedPlayer.Id), Times.Exactly(2));
            _mockedWorldService.Verify(x => x.GetAllPlayers(), Times.Exactly(1));
            _mockedWorldService.Verify(x => x.GetAllCharacters(), Times.Once);
            _mockedPlayerItemPocoDatabaseService.Verify(mock => mock.GetAllAsync(), Times.Exactly(2));
            _mockedPlayerItemPocoDatabaseService.Verify(mock => mock.UpdateAsync(playerItemPOCO), Times.Once);
            _mockedPlayerPocoDatabaseService.Verify(mock => mock.GetAllAsync(), Times.Exactly(2));
            _mockedPlayerPocoDatabaseService.Verify(mock => mock.UpdateAsync(playerPOCO), Times.Once);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestCase("down")]
        [TestCase("up")]
        [TestCase("left")]
        [TestCase("right")]
        [Test]
        public void Test_HandlePacket_HandleAttack_does_damage_to_health(String direction)
        {
            //Arrange

            string GameGuid = Guid.NewGuid().ToString();
            string PlayerGuid = Guid.NewGuid().ToString();
            string AttackedPlayerGuid = Guid.NewGuid().ToString();

            PlayerPOCO playerPOCO = new PlayerPOCO
            {
                PlayerGUID = PlayerGuid,
                Health = 100,
                Stamina = 100,
                GameGUID = null,
                XPosition = 10,
                YPosition = 20
            };

            Player player = new Player("Gert", 10, 20, "#", PlayerGuid);
            PlayerPOCO attackedPlayerPOCO = new PlayerPOCO
            {
                PlayerGUID = AttackedPlayerGuid,
                Health = 0,
                Stamina = 100,
                GameGUID = null,
                XPosition = 26,
                YPosition = 11
            };

            Player attackedPlayer = new Player("Henk", 26, 11, "E", AttackedPlayerGuid);

            _attackDTO.Damage = 20;
            _attackDTO.Stamina = 100;
            _attackDTO.PlayerGuid = PlayerGuid;
            _attackDTO.AttackedPlayerGuid = AttackedPlayerGuid;
            _attackDTO.XPosition = 26;
            _attackDTO.YPosition = 11;

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

            List<Player> players = new List<Player>();
            players.Add(player);
            players.Add(attackedPlayer);

            Monster monster = new Monster("zombie", 10, 20, "T", "monst1");

            List<Character> characters = new List<Character>();
            characters.Add(player);
            characters.Add(attackedPlayer);
            characters.Add(monster);

            _mockedWorldService.Setup(x => x.GetAllCharacters()).Returns(characters);
            _mockedWorldService.Setup(x => x.GetAllPlayers()).Returns(players);

            List<PlayerPOCO> playerPOCOList = new();
            playerPOCOList.Add(attackedPlayerPOCO);
            playerPOCOList.Add(playerPOCO);
            IEnumerable<PlayerPOCO> en = playerPOCOList;
            var task = Task.FromResult(en);

            _mockedPlayerPocoDatabaseService.Setup(mock => mock.GetAllAsync())
                .Returns(task);

            _mockedPlayerPocoDatabaseService.Setup(mock => mock.UpdateAsync(attackedPlayerPOCO));
            _mockedPlayerPocoDatabaseService.Setup(mock => mock.UpdateAsync(playerPOCO));

            PlayerItemPOCO playerItemPOCO = new PlayerItemPOCO()
            {
                PlayerGUID = PlayerGuid,
                GameGUID = null,
                ItemName = "Bandana",
                ArmorPoints = 1
            };

            List<PlayerItemPOCO> playerItemPOCOList = new();
            playerItemPOCOList.Add(playerItemPOCO);
            IEnumerable<PlayerItemPOCO> enItem = playerItemPOCOList;
            var taskItem = Task.FromResult(enItem);

            _mockedPlayerItemPocoDatabaseService.Setup(mock => mock.GetAllAsync())
                .Returns(taskItem);

            _mockedPlayerPocoDatabaseService.Setup(mock => mock.UpdateAsync(attackedPlayerPOCO));
            _mockedPlayerItemPocoDatabaseService.Setup(mock => mock.UpdateAsync(playerItemPOCO));

            var expectedResult = new HandlerResponseDTO(SendAction.SendToClients, null);

            //Act

            var actualResult = _sut.HandlePacket(_packetDTO);


            //Assert

            _mockedClientController.Verify(x => x.IsBackupHost, Times.Once);
            _mockedClientController.Verify(x => x.GetOriginId(), Times.Exactly(3));
            _mockedWorldService.Verify(mock => mock.GetPlayer(player.Id), Times.Exactly(4));
            _mockedWorldService.Verify(mock => mock.GetPlayer(attackedPlayer.Id), Times.Exactly(2));
            _mockedWorldService.Verify(x => x.GetAllCharacters(), Times.Once());
            _mockedWorldService.Verify(x => x.GetAllPlayers(), Times.Once());
            _mockedPlayerItemPocoDatabaseService.Verify(mock => mock.GetAllAsync(), Times.Once);
            _mockedPlayerItemPocoDatabaseService.Verify(mock => mock.UpdateAsync(playerItemPOCO), Times.Once);
            _mockedPlayerPocoDatabaseService.Verify(mock => mock.GetAllAsync(), Times.Exactly(2));
            _mockedPlayerPocoDatabaseService.Verify(mock => mock.UpdateAsync(playerPOCO), Times.Once);
            _mockedPlayerPocoDatabaseService.Verify(mock => mock.UpdateAsync(attackedPlayerPOCO), Times.Once);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestCase("down")]
        [TestCase("up")]
        [TestCase("left")]
        [TestCase("right")]
        [Test]
        public void Test_HandlePacket_HandleAttack_Destroyed_Armor(String direction)
        {
            //Arrange

            string GameGuid = Guid.NewGuid().ToString();
            string PlayerGuid = Guid.NewGuid().ToString();
            string AttackedPlayerGuid = Guid.NewGuid().ToString();

            PlayerPOCO playerPOCO = new PlayerPOCO
            {
                PlayerGUID = PlayerGuid,
                Health = 100,
                Stamina = 100,
                GameGUID = null,
                XPosition = 10,
                YPosition = 20
            };

            Player player = new Player("Gert", 10, 20, "#", PlayerGuid);
            PlayerPOCO attackedPlayerPOCO = new PlayerPOCO
            {
                PlayerGUID = AttackedPlayerGuid,
                Health = 0,
                Stamina = 100,
                GameGUID = null,
                XPosition = 26,
                YPosition = 11
            };

            Player attackedPlayer = new Player("Henk", 26, 11, "E", AttackedPlayerGuid);
            var item = ItemFactory.GetJacket();
            item.ArmorProtectionPoints = 1;
            attackedPlayer.Inventory.Armor = item;

            _attackDTO.Damage = 20;
            _attackDTO.Stamina = 100;
            _attackDTO.PlayerGuid = PlayerGuid;
            _attackDTO.AttackedPlayerGuid = AttackedPlayerGuid;
            _attackDTO.XPosition = 26;
            _attackDTO.YPosition = 11;

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

            Monster monster = new Monster("zombie", 10, 20, "T", "monst1");

            List<Player> players = new List<Player>();
            players.Add(player);
            players.Add(attackedPlayer);

            List<Character> characters = new List<Character>();
            characters.Add(player);
            characters.Add(attackedPlayer);
            characters.Add(monster);

            _mockedWorldService.Setup(x => x.GetAllCharacters()).Returns(characters);

            List<PlayerPOCO> playerPOCOList = new();
            playerPOCOList.Add(attackedPlayerPOCO);
            playerPOCOList.Add(playerPOCO);
            IEnumerable<PlayerPOCO> en = playerPOCOList;
            var task = Task.FromResult(en);

            _mockedPlayerPocoDatabaseService.Setup(mock => mock.GetAllAsync())
                .Returns(task);
            _mockedPlayerPocoDatabaseService.Setup(mock => mock.UpdateAsync(playerPOCO));
            _mockedWorldService.Setup(mock => mock.GetAllPlayers()).Returns(players);

            PlayerItemPOCO playerItemPOCO = new PlayerItemPOCO()
            {
                PlayerGUID = PlayerGuid,
                GameGUID = null,
                ItemName = "Bandana",
                ArmorPoints = 1
            };
            PlayerItemPOCO playerArmorPOCO = new PlayerItemPOCO()
            {
                PlayerGUID = PlayerGuid,
                GameGUID = null,
                ItemName = "Jacket",
                ArmorPoints = 1
            };

            List<PlayerItemPOCO> playerItemPOCOList = new();
            playerItemPOCOList.Add(playerItemPOCO);
            playerItemPOCOList.Add(playerArmorPOCO);
            IEnumerable<PlayerItemPOCO> enItem = playerItemPOCOList;
            var taskItem = Task.FromResult(enItem);

            _mockedPlayerItemPocoDatabaseService.Setup(mock => mock.GetAllAsync())
                .Returns(taskItem);
            _mockedPlayerItemPocoDatabaseService.Setup(mock => mock.UpdateAsync(playerItemPOCO));
            var expectedResult = new HandlerResponseDTO(SendAction.SendToClients, null);

            //Act

            var actualResult = _sut.HandlePacket(_packetDTO);

            //Assert

            _mockedClientController.Verify(x => x.IsBackupHost, Times.Once);
            _mockedClientController.Verify(x => x.GetOriginId(), Times.Exactly(3));
            _mockedWorldService.Verify(mock => mock.GetPlayer(player.Id), Times.Exactly(4));
            _mockedWorldService.Verify(mock => mock.GetPlayer(attackedPlayer.Id), Times.Exactly(2));
            _mockedWorldService.Verify(x => x.GetAllCharacters(), Times.Once());
            _mockedPlayerItemPocoDatabaseService.Verify(mock => mock.GetAllAsync(), Times.Exactly(1));
            _mockedPlayerItemPocoDatabaseService.Verify(mock => mock.UpdateAsync(playerItemPOCO), Times.Once);
            _mockedPlayerPocoDatabaseService.Verify(mock => mock.GetAllAsync(), Times.Exactly(2));
            _mockedPlayerPocoDatabaseService.Verify(mock => mock.UpdateAsync(playerPOCO), Times.Once);


            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestCase("down")]
        [TestCase("up")]
        [TestCase("left")]
        [TestCase("right")]
        [Test]
        public void Test_HandlePacket_HandleAttack_Attack_Creature(String direction)
        {
            //Arrange

            string GameGuid = Guid.NewGuid().ToString();
            string PlayerGuid = Guid.NewGuid().ToString();
            string AttackedPlayerGuid = Guid.NewGuid().ToString();

            PlayerPOCO playerPOCO = new PlayerPOCO
            {
                PlayerGUID = PlayerGuid,
                Health = 100,
                Stamina = 100,
                GameGUID = null,
                XPosition = 10,
                YPosition = 20
            };

            Player player = new Player("Gert", 10, 20, "#", PlayerGuid);
            PlayerPOCO attackedPlayerPOCO = new PlayerPOCO
            {
                PlayerGUID = AttackedPlayerGuid,
                Health = 0,
                Stamina = 100,
                GameGUID = null,
                XPosition = 26,
                YPosition = 11
            };

            Player attackedPlayer = new Player("Henk", 20, 11, "E", AttackedPlayerGuid);
            var item = ItemFactory.GetJacket();
            item.ArmorProtectionPoints = 1;
            attackedPlayer.Inventory.Armor = item;

            _attackDTO.Damage = 20;
            _attackDTO.Stamina = 100;
            _attackDTO.PlayerGuid = PlayerGuid;
            _attackDTO.AttackedPlayerGuid = AttackedPlayerGuid;
            _attackDTO.XPosition = 26;
            _attackDTO.YPosition = 11;

            _mockedClientController.Setup(x => x.IsBackupHost).Returns(true);
            _mockedClientController.Setup(x => x.GetOriginId()).Returns(PlayerGuid);
            _mockedClientController.Object.SetSessionId(GameGuid);

            _mockedWorldService.Setup(mock => mock.GetPlayer(player.Id)).Returns(player);


            var payload = JsonConvert.SerializeObject(_attackDTO);
            _packetDTO.Payload = payload;
            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO();
            packetHeaderDTO.OriginID = PlayerGuid;
            packetHeaderDTO.SessionID = null;
            packetHeaderDTO.PacketType = PacketType.Attack;
            packetHeaderDTO.Target = "host";
            _packetDTO.Header = packetHeaderDTO;

            List<Player> playerList = new List<Player>();
            playerList.Add(player);
            playerList.Add(attackedPlayer);

            Monster monster = new Monster("zombie", 26, 11, "T", "monst1");

            List<Character> characters = new List<Character>();
            characters.Add(player);
            characters.Add(attackedPlayer);
            characters.Add(monster);

            _mockedWorldService.Setup(x => x.GetAllCharacters()).Returns(characters);

            List<PlayerPOCO> playerPOCOList = new();
            playerPOCOList.Add(attackedPlayerPOCO);
            playerPOCOList.Add(playerPOCO);
            IEnumerable<PlayerPOCO> en = playerPOCOList;
            var task = Task.FromResult(en);

            _mockedPlayerPocoDatabaseService.Setup(mock => mock.GetAllAsync())
                .Returns(task);
            _mockedPlayerPocoDatabaseService.Setup(mock => mock.UpdateAsync(playerPOCO));
            _mockedWorldService.Setup(mock => mock.GetAI(AttackedPlayerGuid)).Returns(monster);

            var expectedResult = new HandlerResponseDTO(SendAction.SendToClients, null);

            //Act

            var actualResult = _sut.HandlePacket(_packetDTO);


            //Assert

            _mockedClientController.Verify(x => x.IsBackupHost, Times.Once);
            _mockedClientController.Verify(x => x.GetOriginId(), Times.Exactly(2));
            _mockedWorldService.Verify(mock => mock.GetPlayer(player.Id), Times.Exactly(4));
            _mockedWorldService.Verify(x => x.GetAllCharacters(), Times.Once());
            _mockedWorldService.Verify(mock => mock.GetAI(AttackedPlayerGuid), Times.Once());
            _mockedPlayerPocoDatabaseService.Verify(mock => mock.GetAllAsync(), Times.Once);
            _mockedPlayerPocoDatabaseService.Verify(mock => mock.UpdateAsync(playerPOCO), Times.Once);


            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestCase("up")]
        [TestCase("down")]
        [TestCase("left")]
        [TestCase("right")]
        [Test]
        public void Test_HandlePacket_HandleAttack_CreatureIsAlreadyDead(String direction)
        {
            //Arrange

            string GameGuid = Guid.NewGuid().ToString();
            string PlayerGuid = Guid.NewGuid().ToString();
            string AttackedPlayerGuid = Guid.NewGuid().ToString();


            Player attackedPlayer = new Player("Henk", 14, 11, "E", AttackedPlayerGuid);

            _attackDTO.Damage = 20;
            _attackDTO.Stamina = 100;
            _attackDTO.PlayerGuid = PlayerGuid;
            _attackDTO.AttackedPlayerGuid = AttackedPlayerGuid;
            _attackDTO.XPosition = 26;
            _attackDTO.YPosition = 11;

            _mockedClientController.Setup(x => x.IsBackupHost).Returns(true);
            _mockedClientController.Setup(x => x.GetOriginId()).Returns(PlayerGuid);
            _mockedClientController.Object.SetSessionId(GameGuid);

            var payload = JsonConvert.SerializeObject(_attackDTO);
            _packetDTO.Payload = payload;
            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO();
            packetHeaderDTO.OriginID = PlayerGuid;
            packetHeaderDTO.SessionID = null;
            packetHeaderDTO.PacketType = PacketType.Attack;
            packetHeaderDTO.Target = "host";
            _packetDTO.Header = packetHeaderDTO;

            PlayerPOCO playerPOCO = new PlayerPOCO
            {
                PlayerGUID = PlayerGuid,
                Health = 100,
                Stamina = 100,
                GameGUID = null,
                XPosition = 10,
                YPosition = 20
            };

            Player player = new Player("Gert", 10, 20, "#", PlayerGuid);
            PlayerPOCO attackedPlayerPOCO = new PlayerPOCO
            {
                PlayerGUID = AttackedPlayerGuid,
                Health = 0,
                Stamina = 100,
                GameGUID = null,
                XPosition = 26,
                YPosition = 11
            };

            Monster monster = new Monster("zombie", 10, 20, "T", "monst1");
            monster.Health = 0;
            List<Character> characters = new List<Character>();
            characters.Add(player);
            characters.Add(attackedPlayer);
            characters.Add(monster);

            _mockedWorldService.Setup(x => x.GetAllCharacters()).Returns(characters);

            List<PlayerPOCO> playerPOCOList = new();
            playerPOCOList.Add(attackedPlayerPOCO);
            playerPOCOList.Add(playerPOCO);
            IEnumerable<PlayerPOCO> en = playerPOCOList;
            var task = Task.FromResult(en);

            _mockedPlayerPocoDatabaseService.Setup(mock => mock.GetAllAsync())
                .Returns(task);
            _mockedPlayerPocoDatabaseService.Setup(mock => mock.UpdateAsync(playerPOCO));

            _mockedWorldService.Setup(x => x.GetPlayer(PlayerGuid)).Returns(player);

            var expectedResult = new HandlerResponseDTO(SendAction.ReturnToSender,
                "There is no enemy to attack");

            //Act

            var actualResult = _sut.HandlePacket(_packetDTO);

            //Assert

            _mockedClientController.Verify(x => x.IsBackupHost, Times.Once);
            _mockedClientController.Verify(x => x.GetOriginId(), Times.Exactly(2));
            _mockedWorldService.Verify(x => x.GetAllCharacters(), Times.Once());
            _mockedWorldService.Verify(x => x.GetPlayer(PlayerGuid), Times.Exactly(3));

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}