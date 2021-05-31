using ActionHandling.DTO;
using Items;
using Moq;
using Network;
using Network.DTO;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration;

namespace ActionHandling.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class InventoryHandlerTest
    {
        private InventoryHandler _sut;
        private Mock<IClientController> _mockedClientController;
        private Mock<IWorldService> _mockedWorldService;

        [SetUp]
        public void Setup()
        {
            _mockedClientController = new Mock<IClientController>();
            _mockedWorldService = new Mock<IWorldService>();
            _sut = new InventoryHandler(_mockedClientController.Object, _mockedWorldService.Object);
        }

        [Test]
        public void Test_Search_CallsWorldService()
        {
            //arrange
            string exampleResult = "result";

            _mockedWorldService.Setup(mock => mock.SearchCurrentTile()).Returns(exampleResult);

            //act
            _sut.Search();

            //assert
            _mockedWorldService.Verify(mock => mock.SearchCurrentTile(), Times.Once);
        }

        [Test]
        public void Test_HandlePacket_HandlesUsePacketSuccesOnHost()
        {
            //arrange
            bool isHost = true;
            string originId = "origin1";
            int index = 1;
            
            InventoryDTO inventoryDTO = new(originId, InventoryType.Use, index);

            var payload = JsonConvert.SerializeObject(inventoryDTO);
            PacketDTO packetDTO = new();
            packetDTO.Payload = payload;
            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO();
            packetHeaderDTO.Target = "host";

            Player player = new("arie", 0, 0, "#", originId);
            player.Health = 50;
            player.Inventory.AddConsumableItem(ItemFactory.GetBandage());

            _mockedWorldService.Setup(mock => mock.GetPlayer(originId)).Returns(player);
            _mockedClientController.Setup(mock => mock.IsHost()).Returns(isHost);

            var expectedResult = new HandlerResponseDTO(SendAction.SendToClients, null);

            //act
            var result = _sut.HandlePacket(packetDTO);

            //assert
            Assert.IsTrue(player.Inventory.ConsumableItemList.Count == 0);
            Assert.IsTrue(player.Health == 75);
            Assert.AreEqual(expectedResult, result);
        }





    }
}
