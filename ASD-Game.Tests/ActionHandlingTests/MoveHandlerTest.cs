using System;
using System.Diagnostics.CodeAnalysis;
using ASD_Game.ActionHandling;
using ASD_Game.ActionHandling.DTO;
using ASD_Game.DatabaseHandler.POCO;
using ASD_Game.DatabaseHandler.Services;
using ASD_Game.Messages;
using ASD_Game.Network;
using ASD_Game.Network.DTO;
using ASD_Game.Network.Enum;
using ASD_Game.World.Models.Characters;
using ASD_Game.World.Models.Interfaces;
using ASD_Game.World.Models.TerrainTiles;
using ASD_Game.World.Services;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace ASD_Game.Tests.ActionHandlingTests
{
    [ExcludeFromCodeCoverage]
    public class MoveHandlerTest
    {
        private MoveHandler _sut;
        private Mock<IClientController> _mockedClientController;
        private Mock<IWorldService> _mockedWorldService;
        private Mock<IDatabaseService<PlayerPOCO>> _mockedPlayerDatabaseService;
        private Mock<IMessageService> _mockedMessageService;

        [SetUp]
        public void Setup()
        {
            _mockedClientController = new Mock<IClientController>();
            _mockedWorldService = new Mock<IWorldService>();
            _mockedPlayerDatabaseService = new Mock<IDatabaseService<PlayerPOCO>>();
            _mockedMessageService = new Mock<IMessageService>();
            _sut = new MoveHandler(_mockedClientController.Object, _mockedWorldService.Object, _mockedPlayerDatabaseService.Object, _mockedMessageService.Object);
        }

        [TestCase("up")]
        [TestCase("down")]
        [TestCase("left")]
        [TestCase("right")]
        [Test]
        public void Test_SendMove_(string move)
        {
            // arrange
            var direction = move;
            int steps = 5;
            int x = 26;
            int y = 11;

            Player player = new Player("test", x, y, "#", "test2");

            _mockedWorldService.Setup(mock => mock.GetCurrentPlayer()).Returns(player);
            _mockedClientController.Setup(mock => mock.SendPayload(It.IsAny<string>(), PacketType.Move));
            
            // act
            _sut.SendMove(direction, steps);
            
            // assert
            _mockedClientController.Verify(mock => mock.SendPayload(It.IsAny<string>(), PacketType.Move), Times.Once);
        }

        [TestCase("up")]
        [TestCase("down")]
        [TestCase("left")]
        [TestCase("right")]
        [Test]
        public void Test_DeadMove_(string move)
        {
            // arrange
            var direction = move;
            int steps = 5;
            int x = 26;
            int y = 11;
            bool isded = true;

            Player player = new Player("test", x, y, "#", "test2");

            _mockedWorldService.Setup(mock => mock.GetCurrentPlayer()).Returns(player);
            _mockedWorldService.Setup(mock => mock.IsDead(player)).Returns(isded);

            // act
            _sut.SendMove(direction, steps);

            // assert
            _mockedMessageService.Verify(mock => mock.AddMessage("You can't move, you're dead!"), Times.Once);
        }

        [Test]
        public void Test_HandlePacket_CorrectHostPacket()
        {
            // arrange
            PacketDTO _packetDTO = new PacketDTO();
            MoveDTO _moveDTO = new MoveDTO();

            string GameGuid = Guid.NewGuid().ToString();
            string PlayerGuid = Guid.NewGuid().ToString();
            int _health = 100;
            int _stamina = 100;
            int _XPosition = 10;
            int _YPosition = 20;

            Player player = new Player("Gert", 10, 20, "#", PlayerGuid);

            _moveDTO.Stamina = 1;
            _moveDTO.UserId = PlayerGuid;
            _moveDTO.XPosition = _XPosition+1;
            _moveDTO.YPosition = _YPosition+1;

            _mockedClientController.Setup(x => x.GetOriginId()).Returns(PlayerGuid);
            _mockedClientController.Object.SetSessionId(GameGuid);
            _mockedWorldService.Setup(mock => mock.GetPlayer(player.Id)).Returns(player);

            ITile tile = new GrassTile(_XPosition, _YPosition);
            _mockedWorldService.Setup(mock => mock.GetTile(It.IsAny<int>(), It.IsAny<int>())).Returns(tile);

            var payload = JsonConvert.SerializeObject(_moveDTO);
            _packetDTO.Payload = payload;
            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO();
            packetHeaderDTO.OriginID = PlayerGuid;
            packetHeaderDTO.SessionID = null;
            packetHeaderDTO.PacketType = PacketType.Attack;
            packetHeaderDTO.Target = "host";
            _packetDTO.Header = packetHeaderDTO;

            // act
            _sut.HandlePacket(_packetDTO);

            // assert
            _mockedWorldService.Verify(mock => mock.DisplayWorld(), Times.Once);
        }

        [Test]
        public void Test_HandlePacket_PlayerHasNoStamina()
        {
            // arrange
            PacketDTO _packetDTO = new PacketDTO();
            MoveDTO _moveDTO = new MoveDTO();

            string GameGuid = Guid.NewGuid().ToString();
            string PlayerGuid = Guid.NewGuid().ToString();
            int _stamina = 1;
            int _XPosition = 10;
            int _YPosition = 20;

            Player player = new Player("Gert", _XPosition, _YPosition, "#", PlayerGuid);
            player.Stamina = _stamina;

            _moveDTO.Stamina = _stamina;
            _moveDTO.UserId = PlayerGuid;
            _moveDTO.XPosition = _XPosition + 5;

            _mockedClientController.Setup(x => x.GetOriginId()).Returns(PlayerGuid);
            _mockedClientController.Object.SetSessionId(GameGuid);
            _mockedWorldService.Setup(mock => mock.GetPlayer(player.Id)).Returns(player);

            ITile tile = new GrassTile(_XPosition, _YPosition);
            _mockedWorldService.Setup(mock => mock.GetTile(It.IsAny<int>(), It.IsAny<int>())).Returns(tile);

            var payload = JsonConvert.SerializeObject(_moveDTO);
            _packetDTO.Payload = payload;
            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO();
            packetHeaderDTO.OriginID = PlayerGuid;
            packetHeaderDTO.SessionID = null;
            packetHeaderDTO.PacketType = PacketType.Attack;
            packetHeaderDTO.Target = "host";
            _packetDTO.Header = packetHeaderDTO;

            var expectedResult = new HandlerResponseDTO(SendAction.ReturnToSender, "You do not have enough stamina to move!");

            // act
            var actualResult = _sut.HandlePacket(_packetDTO);

            // assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void Test_HandlePacket_DifferentPacket()
        {
            // arrange
            PacketDTO _packetDTO = new PacketDTO();
            MoveDTO _moveDTO = new MoveDTO();

            var payload = JsonConvert.SerializeObject(_moveDTO);
            _packetDTO.Payload = payload;
            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO();
            packetHeaderDTO.Target = "error";
            _packetDTO.Header = packetHeaderDTO;

            _mockedMessageService.Setup(mock => mock.AddMessage(null));
            _mockedClientController.Setup(mock => mock.IsHost()).Returns(true);
            _mockedClientController.Setup(mock => mock.IsBackupHost).Returns(true);
            _packetDTO.HandlerResponse = new HandlerResponseDTO(SendAction.Ignore, null);

            var expectedResult = new HandlerResponseDTO(SendAction.Ignore, null);

            // act
            var actualResult = _sut.HandlePacket(_packetDTO);

            // assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void Test_HandlePacket_AIPacket()
        {
            // arrange
            PacketDTO _packetDTO = new PacketDTO();
            MoveDTO _moveDTO = new MoveDTO();

            string GameGuid = Guid.NewGuid().ToString();
            string PlayerGuid = Guid.NewGuid().ToString();
            int _health = 100;
            int _stamina = 100;
            int _XPosition = 10;
            int _YPosition = 20;

            Player player = new Player("Gert", 10, 20, "#", PlayerGuid);

            _moveDTO.Stamina = 1;
            _moveDTO.UserId = PlayerGuid;
            _moveDTO.XPosition = _XPosition + 1;
            _moveDTO.YPosition = _YPosition + 1;

            _mockedClientController.Object.SetSessionId(GameGuid);
            _mockedWorldService.Setup(mock => mock.GetPlayer(player.Id)).Returns(player);

            ITile tile = new GrassTile(_XPosition, _YPosition);
            _mockedWorldService.Setup(mock => mock.GetTile(It.IsAny<int>(), It.IsAny<int>())).Returns(tile);

            var payload = JsonConvert.SerializeObject(_moveDTO);
            _packetDTO.Payload = payload;
            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO();
            packetHeaderDTO.OriginID = PlayerGuid;
            packetHeaderDTO.SessionID = null;
            packetHeaderDTO.PacketType = PacketType.Attack;
            packetHeaderDTO.Target = "host";
            _packetDTO.Header = packetHeaderDTO;

            // act
            _sut.HandlePacket(_packetDTO);

            // assert
            _mockedWorldService.Verify(mock => mock.DisplayWorld(), Times.Once);
        }

    }
}