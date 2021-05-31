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
using WorldGeneration;

namespace ActionHandling.Tests
{
    class AttackHandlerTest
    {

        [ExcludeFromCodeCoverage]
        [TestFixture]
        public class MoveHandlerTests
        {
            //Declaration and initialisation of constant variables
            //Declaration of variables
            private AttackHandler _sut;
            private AttackDTO attackDTO;
            //Declaration of mocks
            private Mock<IClientController> _mockedClientController;
            private Mock<IWorldService> _mockedWorldService;
            private Mock<Player> _mockedPlayer;
            [SetUp]
            public void Setup()
            {
                _mockedClientController = new Mock<IClientController>();
                _mockedWorldService = new Mock<IWorldService>();
                _sut = new AttackHandler(_mockedClientController.Object, _mockedWorldService.Object);
                

            }
            [Test]
            public void Test_SendMove_SendsTheMessageAndPacketTypeToClientController()
            {
                string playerGuid = new Guid().ToString();
                string AttackGuid = new Guid().ToString();
                Player player = new Player("John", 10, 10, "Nee", playerGuid);
                _mockedWorldService.Setup(WorldService => WorldService.getCurrentPlayer())
                .Returns(player);
                attackDTO = new AttackDTO();
                attackDTO.XPosition = 10;
                attackDTO.YPosition = 10;
                attackDTO.Damage = 20;
                attackDTO.PlayerGuid = playerGuid;
                attackDTO.AttackedPlayerGuid = AttackGuid;
                //Arrange ---------
                var payload = JsonConvert.SerializeObject(attackDTO);
                _mockedClientController.Setup(mock => mock.SendPayload(payload, PacketType.Attack));
                //Act ---------
                _sut.SendAttack("left");
                //Assert ---------
                _mockedClientController.Verify(mock => mock.SendPayload(payload, PacketType.Attack), Times.Once());
            }
            // [Test]
            // public void Test_HandlePacket_HandleMoveProperly()
            // {
            //     //Arrange ---------
            //     string playerGuid = new Guid().ToString();
            //     string GameGuid = new Guid().ToString();
            //     //Arrange ---------
            //     _mapCharacterDTO = new MapCharacterDTO(10,10, playerGuid, GameGuid);
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
