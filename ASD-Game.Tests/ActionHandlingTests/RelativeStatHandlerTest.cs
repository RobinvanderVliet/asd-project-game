using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using ASD_Game.ActionHandling;
using ASD_Game.ActionHandling.DTO;
using ASD_Game.DatabaseHandler.POCO;
using ASD_Game.DatabaseHandler.Services;
using ASD_Game.Messages;
using ASD_Game.Network;
using ASD_Game.Network.DTO;
using ASD_Game.Network.Enum;
using ASD_Game.World.Models.Characters;
using ASD_Game.World.Models.HazardousTiles;
using ASD_Game.World.Services;

namespace ASD_Game.Tests.ActionHandlingTests
{
    [ExcludeFromCodeCoverage]
    public class RelativeStatHandlerTest
    {
        private Mock<IClientController> _mockedClientController;
        private Mock<IWorldService> _mockedWorldService;
        private Mock<IDatabaseService<PlayerPOCO>> _mockedPlayerDatabaseService;
        private Mock<IMessageService> _mockedMessageService;
        private RelativeStatHandler _sut;

        [SetUp]
        public void Setup()
        {
            _mockedClientController = new Mock<IClientController>();
            _mockedWorldService = new Mock<IWorldService>();
            _mockedPlayerDatabaseService = new Mock<IDatabaseService<PlayerPOCO>>();
            _mockedMessageService = new Mock<IMessageService>();

            _sut = new RelativeStatHandler(_mockedClientController.Object, _mockedWorldService.Object, _mockedPlayerDatabaseService.Object, _mockedMessageService.Object);
        }

        [Test]
        public void Test_SendStat_SendsPayloadCorrectly()
        {
            //Arrange
            var dto = new RelativeStatDTO();
            dto.Id = "testId";
            dto.Stamina = 5;

            var payload = JsonConvert.SerializeObject(dto);

            _mockedClientController.Setup(mock => mock.GetOriginId()).Returns("testId");
            _mockedClientController.Setup(mock => mock.SendPayload(payload, PacketType.RelativeStat));

            //Act
            _sut.SendStat(dto);

            //Assert
            _mockedClientController.Verify(mock => mock.GetOriginId(), Times.Once);
            _mockedClientController.Verify(mock => mock.SendPayload(payload, PacketType.RelativeStat), Times.Once);
        }

        [Test]
        public void Test_SetCurrentPlayer_SetsCurrentPlayerCorrectly()
        {
            //arrange
            var player = new Player("f", 0, 42, "#", "f");
            _mockedWorldService.Setup(mock => mock.GetCurrentPlayer()).Returns(player);

            //act
            _sut.SetCurrentPlayer(player);

            //assert
            _mockedWorldService.Verify(mock => mock.GetCurrentPlayer(), Times.Once);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void Test_CheckStaminaTimer_SetsStatDTO(bool isDead)
        {
            //arrange
            var player = new Player("f", 0, 42, "#", "f");
            player.Stamina = 90;
            _mockedWorldService.Setup(mock => mock.GetCurrentPlayer()).Returns(player);
            _mockedWorldService.Setup(mock => mock.IsDead(player)).Returns(isDead);
            _sut.SetCurrentPlayer(player);

            var dto = new RelativeStatDTO();
            dto.Id = player.Id;
            dto.Stamina = 1;

            var payload = JsonConvert.SerializeObject(dto);

            _mockedClientController.Setup(mock => mock.GetOriginId()).Returns("f");
            _mockedClientController.Setup(mock => mock.SendPayload(payload, PacketType.RelativeStat));

            //act
            _sut.CheckStaminaTimer();
            Thread.Sleep(1100);

            //assert
            _mockedWorldService.Setup(mock => mock.IsDead(player));
            _mockedWorldService.Verify(mock => mock.GetCurrentPlayer(), Times.Exactly(2));
            
            if (!isDead)
            {
                _mockedClientController.Verify(mock => mock.GetOriginId(), Times.Once);
                _mockedClientController.Verify(mock => mock.SendPayload(payload, PacketType.RelativeStat), Times.Once);
            }
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void Test_CheckRadiationTimer_SetsStatDTOWithRadiation(bool isDead)
        {
            //arrange
            var player = new Player("f", 0, 42, "#", "f");
            player.RadiationLevel = 10;
            _mockedWorldService.Setup(mock => mock.GetCurrentPlayer()).Returns(player);
            _mockedWorldService.Setup(mock => mock.IsDead(player)).Returns(isDead);
            _sut.SetCurrentPlayer(player);
            var tile = new GasTile(player.XPosition, player.YPosition);

            _mockedWorldService.Setup(mock => mock.GetTile(player.XPosition, player.YPosition)).Returns(tile);

            var dto = new RelativeStatDTO();
            dto.Id = player.Id;
            dto.RadiationLevel = -1;

            var payload = JsonConvert.SerializeObject(dto);

            _mockedClientController.Setup(mock => mock.GetOriginId()).Returns("f");
            _mockedClientController.Setup(mock => mock.SendPayload(payload, PacketType.RelativeStat));

            if (isDead)
            {
                _mockedMessageService.Setup(mock => mock.AddMessage("You died"));
                _mockedWorldService.Setup(mock => mock.DisplayWorld());
            }

            //act
            _sut.CheckRadiationTimer();
            Thread.Sleep(1100);

            //assert
            _mockedWorldService.Setup(mock => mock.IsDead(player));
            
            if (!isDead)
            {
                _mockedWorldService.Verify(mock => mock.GetCurrentPlayer(), Times.Exactly(4));
                _mockedWorldService.Verify(mock => mock.GetTile(player.XPosition, player.YPosition), Times.Once);
                _mockedClientController.Verify(mock => mock.GetOriginId(), Times.Once);
                _mockedClientController.Verify(mock => mock.SendPayload(payload, PacketType.RelativeStat), Times.Once);
            }
            else
            {
                _mockedMessageService.Setup(mock => mock.AddMessage("You died"));
                _mockedWorldService.Setup(mock => mock.DisplayWorld());
                _mockedWorldService.Verify(mock => mock.GetCurrentPlayer(), Times.Exactly(2));
            }
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void Test_CheckRadiationTimer_SetsStatDTOWithHealth(bool isDead)
        {
            //arrange
            var player = new Player("f", 0, 42, "#", "f");
            _mockedWorldService.Setup(mock => mock.GetCurrentPlayer()).Returns(player);
            _mockedWorldService.Setup(mock => mock.IsDead(player)).Returns(isDead);
            _sut.SetCurrentPlayer(player);
            var tile = new GasTile(player.XPosition, player.YPosition);

            _mockedWorldService.Setup(mock => mock.GetTile(player.XPosition, player.YPosition)).Returns(tile);

            var dto = new RelativeStatDTO();
            dto.Id = player.Id;
            dto.Health = -1;

            var payload = JsonConvert.SerializeObject(dto);

            _mockedClientController.Setup(mock => mock.GetOriginId()).Returns("f");
            _mockedClientController.Setup(mock => mock.SendPayload(payload, PacketType.RelativeStat));

            if (isDead)
            {
                _mockedMessageService.Setup(mock => mock.AddMessage("You died"));
                _mockedWorldService.Setup(mock => mock.DisplayWorld());
            }
            
            //act
            _sut.CheckRadiationTimer();
            Thread.Sleep(1100);

            //assert
            _mockedWorldService.Setup(mock => mock.IsDead(player));
            
            if(!isDead) 
            {
                _mockedWorldService.Verify(mock => mock.GetCurrentPlayer(), Times.Exactly(4));
                _mockedWorldService.Verify(mock => mock.GetTile(player.XPosition, player.YPosition), Times.Once);
                _mockedClientController.Verify(mock => mock.GetOriginId(), Times.Once);
                _mockedClientController.Verify(mock => mock.SendPayload(payload, PacketType.RelativeStat), Times.Once);
            }
            else
            {
                _mockedMessageService.Setup(mock => mock.AddMessage("You died"));
                _mockedWorldService.Setup(mock => mock.DisplayWorld());
                _mockedWorldService.Verify(mock => mock.GetCurrentPlayer(), Times.Exactly(2));
            }
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void Test_Handlepacket_WithStamina(bool isHost)
        {
            //arrange
            var player = new Mock<Player>("f", 0, 42, "#", "f");
            player.Object.Stamina = 90;

            var playerPOCO = new PlayerPOCO() { GameGUID = "gameguid", PlayerGUID = "f"};
            List<PlayerPOCO> playerPOCOs = new();
            playerPOCOs.Add(playerPOCO);
            IEnumerable<PlayerPOCO> enumerable = playerPOCOs;

            _mockedClientController.Setup(mock => mock.IsHost()).Returns(isHost);
            _mockedClientController.Setup(mock => mock.IsBackupHost).Returns(false);

            var relativeStatDTO = new RelativeStatDTO() {Id = player.Object.Id, Stamina = 1};

            _mockedWorldService.Setup(mock => mock.GetPlayer(relativeStatDTO.Id)).Returns(player.Object);
            player.Setup(mock => mock.AddStamina(relativeStatDTO.Stamina));

            _mockedClientController.Setup(mock => mock.GetOriginId()).Returns(relativeStatDTO.Id);
            _mockedWorldService.Setup(mock => mock.DisplayStats());
            _mockedClientController.Setup(mock => mock.SessionId).Returns("gameguid");

            var packetHeaderDTO = new PacketHeaderDTO();

            if (isHost)
            {
                _mockedPlayerDatabaseService.Setup(mock => mock.GetAllAsync()).Returns(Task.FromResult(enumerable));
                _mockedPlayerDatabaseService.Setup(mock => mock.UpdateAsync(playerPOCO));
                packetHeaderDTO.Target = "host";
            }
            else
            {
                packetHeaderDTO.Target = "client";
            }

            var payload = JsonConvert.SerializeObject(relativeStatDTO);
            var packetDTO = new PacketDTO() {Payload = payload, Header = packetHeaderDTO};

            var expectedHandlerResponseDTO = new HandlerResponseDTO(SendAction.SendToClients, null);

            //act
            var handlerResponseDTO = _sut.HandlePacket(packetDTO);

            //assert
            Assert.AreEqual(expectedHandlerResponseDTO, handlerResponseDTO);
            _mockedClientController.Verify(mock => mock.IsHost(), Times.Once);
            _mockedWorldService.Verify(mock => mock.GetPlayer(player.Object.Id), Times.Once);
            player.Verify(mock => mock.AddStamina(1));
            _mockedClientController.Verify(mock => mock.GetOriginId(), Times.Once);
            _mockedWorldService.Verify(mock => mock.DisplayStats(), Times.Once);
            if (isHost)
            {
                _mockedClientController.Verify(mock => mock.SessionId, Times.Once);
                _mockedPlayerDatabaseService.Verify(mock => mock.GetAllAsync(), Times.Once);
                _mockedPlayerDatabaseService.Verify(mock => mock.UpdateAsync(playerPOCO), Times.Once);
            }
            else
            {
                _mockedClientController.Verify(mock => mock.IsBackupHost, Times.Once);
            }
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void Test_Handlepacket_WithRadiationLevel(bool isHost)
        {
            //arrange
            var player = new Mock<Player>("f", 0, 42, "#", "f");
            player.Object.RadiationLevel = 1;

            var playerPOCO = new PlayerPOCO() { GameGUID = "gameguid", PlayerGUID = "f" };
            List<PlayerPOCO> playerPOCOs = new();
            playerPOCOs.Add(playerPOCO);
            IEnumerable<PlayerPOCO> enumerable = playerPOCOs;

            _mockedClientController.Setup(mock => mock.IsHost()).Returns(isHost);
            _mockedClientController.Setup(mock => mock.IsBackupHost).Returns(false);

            var relativeStatDTO = new RelativeStatDTO() {Id = player.Object.Id, RadiationLevel = -1};

            _mockedWorldService.Setup(mock => mock.GetPlayer(relativeStatDTO.Id)).Returns(player.Object);
            player.Setup(mock => mock.AddRadiationLevel(relativeStatDTO.RadiationLevel));

            _mockedClientController.Setup(mock => mock.GetOriginId()).Returns(relativeStatDTO.Id);
            _mockedWorldService.Setup(mock => mock.DisplayStats());
            _mockedClientController.Setup(mock => mock.SessionId).Returns("gameguid");

            var packetHeaderDTO = new PacketHeaderDTO();

            if (isHost)
            {
                _mockedPlayerDatabaseService.Setup(mock => mock.GetAllAsync()).Returns(Task.FromResult(enumerable));
                _mockedPlayerDatabaseService.Setup(mock => mock.UpdateAsync(playerPOCO));
                packetHeaderDTO.Target = "host";
            }
            else
            {
                packetHeaderDTO.Target = "client";
            }

            var payload = JsonConvert.SerializeObject(relativeStatDTO);
            var packetDTO = new PacketDTO() {Payload = payload, Header = packetHeaderDTO};

            var expectedHandlerResponseDTO = new HandlerResponseDTO(SendAction.SendToClients, null);

            //act
            var handlerResponseDTO = _sut.HandlePacket(packetDTO);

            //assert
            Assert.AreEqual(expectedHandlerResponseDTO, handlerResponseDTO);
            _mockedClientController.Verify(mock => mock.IsHost(), Times.Once);
            _mockedWorldService.Verify(mock => mock.GetPlayer(player.Object.Id), Times.Once);
            player.Verify(mock => mock.AddRadiationLevel(-1));
            _mockedClientController.Verify(mock => mock.GetOriginId(), Times.Once);
            _mockedWorldService.Verify(mock => mock.DisplayStats(), Times.Once);
            if (isHost)
            {
                _mockedClientController.Verify(mock => mock.SessionId, Times.Once);
                _mockedPlayerDatabaseService.Verify(mock => mock.GetAllAsync(), Times.Once);
                _mockedPlayerDatabaseService.Verify(mock => mock.UpdateAsync(playerPOCO), Times.Once);
            }
            else
            {
                _mockedClientController.Verify(mock => mock.IsBackupHost, Times.Once);
            }
        }
        
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void Test_Handlepacket_WithHealth(bool isHost)
        {
            //arrange
            var player = new Mock<Player>("f", 0, 42, "#", "f");
            player.Object.Health = 90;

            var playerPOCO = new PlayerPOCO() { GameGUID = "gameguid", PlayerGUID = "f" };
            List<PlayerPOCO> playerPOCOs = new();
            playerPOCOs.Add(playerPOCO);
            IEnumerable<PlayerPOCO> enumerable = playerPOCOs;

            _mockedClientController.Setup(mock => mock.IsHost()).Returns(isHost);
            _mockedClientController.Setup(mock => mock.IsBackupHost).Returns(false);

            var relativeStatDTO = new RelativeStatDTO() {Id = player.Object.Id, Health = -1};

            _mockedWorldService.Setup(mock => mock.GetPlayer(relativeStatDTO.Id)).Returns(player.Object);
            player.Setup(mock => mock.AddHealth(relativeStatDTO.Health));

            _mockedClientController.Setup(mock => mock.GetOriginId()).Returns(relativeStatDTO.Id);
            _mockedWorldService.Setup(mock => mock.DisplayStats());
            _mockedClientController.Setup(mock => mock.SessionId).Returns("gameguid");

            var packetHeaderDTO = new PacketHeaderDTO();

            if (isHost)
            {
                _mockedPlayerDatabaseService.Setup(mock => mock.GetAllAsync()).Returns(Task.FromResult(enumerable));
                _mockedPlayerDatabaseService.Setup(mock => mock.UpdateAsync(playerPOCO));
                packetHeaderDTO.Target = "host";
            }
            else
            {
                packetHeaderDTO.Target = "client";
            }

            var payload = JsonConvert.SerializeObject(relativeStatDTO);
            var packetDTO = new PacketDTO() {Payload = payload, Header = packetHeaderDTO};

            var expectedHandlerResponseDTO = new HandlerResponseDTO(SendAction.SendToClients, null);

            //act
            var handlerResponseDTO = _sut.HandlePacket(packetDTO);

            //assert
            Assert.AreEqual(expectedHandlerResponseDTO, handlerResponseDTO);
            _mockedClientController.Verify(mock => mock.IsHost(), Times.Once);
            _mockedWorldService.Verify(mock => mock.GetPlayer(player.Object.Id), Times.Once);
            player.Verify(mock => mock.AddHealth(-1));
            _mockedClientController.Verify(mock => mock.GetOriginId(), Times.Once);
            _mockedWorldService.Verify(mock => mock.DisplayStats(), Times.Once);
            if (isHost)
            {
                _mockedClientController.Verify(mock => mock.SessionId, Times.Once);
                _mockedPlayerDatabaseService.Verify(mock => mock.GetAllAsync(), Times.Once);
                _mockedPlayerDatabaseService.Verify(mock => mock.UpdateAsync(playerPOCO), Times.Once);
            }
            else
            {
                _mockedClientController.Verify(mock => mock.IsBackupHost, Times.Once);
            }
        }
    }
}