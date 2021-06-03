using ActionHandling;
using Moq;
using Network;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActionHandling.DTO;
using DatabaseHandler.POCO;
using DatabaseHandler.Services;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Network.DTO;
using WorldGeneration;

namespace ActionHandling.Tests
{
    class AttackHandlerTest
    {

        [ExcludeFromCodeCoverage]
        [TestFixture]
        public class AttackHandlerTests
        {
            //Declaration and initialisation of constant variables
            //Declaration of variables
            private AttackHandler _sut;
            private AttackDTO _attackDTO;
            //Declaration of mocks
            private Mock<IClientController> _mockedClientController;
            private Mock<IWorldService> _mockedWorldService;
            private Mock<Player> _mockedPlayer;
            private Mock<List<Player>> _mockedPlayers;
            private Mock<IDeadHandler> _mockedDeadHandler;
            private Mock<DatabaseService<PlayerPOCO>> _mockedPlayerPocoDatabaseService;
            private Mock<DatabaseService<PlayerItemPOCO>> _mockedPlayerItemPocoDatabaseService;
            private Mock<DatabaseService<CreaturePOCO>> _mockedCreaturePocoDatabaseService;
            [SetUp]
            public void Setup()
            {
                _mockedClientController = new Mock<IClientController>();
                _mockedWorldService = new Mock<IWorldService>();
                _mockedDeadHandler = new Mock<IDeadHandler>();
                _mockedPlayerPocoDatabaseService = new Mock<DatabaseService<PlayerPOCO>>();
                _mockedPlayerItemPocoDatabaseService = new Mock<DatabaseService<PlayerItemPOCO>>();
                _mockedCreaturePocoDatabaseService = new Mock<DatabaseService<CreaturePOCO>>();
                    
                _sut = new AttackHandler(_mockedClientController.Object, _mockedWorldService.Object, _mockedDeadHandler.Object, _mockedPlayerPocoDatabaseService.Object, _mockedPlayerItemPocoDatabaseService.Object, _mockedCreaturePocoDatabaseService.Object);
                

            }
            // [TestCase("up")]
            // [TestCase("down")]
            // [TestCase("left")]
            // [TestCase("right")]
            
            // [Test]
            // public void Test_SendAttack_SendsTheMessageAndPacketTypeToClientController(String direction)
            // {
            //     //Arrange
            //     int x = 26;
            //     int y = 11;
            //     string PlayerGuid = Guid.NewGuid().ToString();
            //     //string AttackGuid = Guid.NewGuid().ToString();
            //     Player player = new Player("test", x, y, "#", PlayerGuid);
            //
            //     _mockedWorldService.Setup(WorldService => WorldService.getCurrentPlayer())
            //         .Returns(player);
            //     _attackDTO = new AttackDTO();
            //     _attackDTO.XPosition = 26;
            //     _attackDTO.YPosition = 11;
            //     _attackDTO.Damage = 20;
            //     _attackDTO.PlayerGuid = PlayerGuid;
            //     //attackDTO.AttackedPlayerGuid = AttackGuid;
            //
            //     _mockedClientController.Setup(ClientController => ClientController.GetOriginId())
            //         .Returns(PlayerGuid);
            //     var payload = JsonConvert.SerializeObject(_attackDTO);
            //
            //     _mockedClientController.Setup(mock => mock.SendPayload(payload, PacketType.Attack));
            //     //Act ---------
            //     _sut.SendAttack(direction);
            //     //Assert ---------
            //     _mockedClientController.Verify(mock => mock.SendPayload(It.IsAny<String>(), PacketType.Attack), Times.Once());
            // }

            // [Test]
            // public void TestHandlePacket_HandleAttack()
            // {
            //     //Arrange
            //     string playerGuid = new Guid().ToString();
            //     string GameGuid = new Guid().ToString();
            //     List<Player> allPlayers = new List<Player>();
            //     _attackDTO = new AttackDTO();
            //     _packetDTO.Payload = payload;
            //     _packetDTO.Header.Target = playerGuid;
            //     //Act
            //     HandlerResponseDTO actualResult = _sut.HandlePacket(_packetDTO);
            //     
            //
            //     //Assert
            //     HandlerResponseDTO ExpectedResult = new HandlerResponseDTO(SendAction.SendToClients, null);
            //     Assert.AreEqual(ExpectedResult, actualResult);
            // }
            
            




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
}
