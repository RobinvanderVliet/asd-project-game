using System;
using System.Diagnostics.CodeAnalysis;
using Chat;
using DataTransfer.DTO.Character;
using Moq;
using Network;
using NUnit.Framework;
using Player.ActionHandlers;
using Player.Exceptions;
using Player.Model;
using Player.Services;
using WorldGeneration;

namespace Player.Tests
{
    [ExcludeFromCodeCoverage]
    public class PlayerServiceTest
    {
        private PlayerService _sut;
        private Mock<IPlayerModel> _mockedPlayerModel;
        private Mock<IMoveHandler> _mockedMoveHandler;
        private Mock<IChatHandler> _mockedChatHandler;
        private Mock<IClientController> _mockedClientController;
        private Mock<IWorldService> _mockedWorldService;

        [SetUp]
        public void Setup()
        {
            _mockedPlayerModel = new Mock<IPlayerModel>();
            _mockedMoveHandler = new Mock<IMoveHandler>();
            _mockedChatHandler = new Mock<IChatHandler>();
            _mockedClientController = new Mock<IClientController>();
            _mockedWorldService = new Mock<IWorldService>();

            _sut = new PlayerService(_mockedPlayerModel.Object, _mockedChatHandler.Object, _mockedMoveHandler.Object, _mockedClientController.Object, _mockedWorldService.Object);
        }
        
        [Test]
        public void Test_Say_CallsFunctionFromChatHandler()
        {
            // Arrange
            const string message = "hello world";
            _mockedChatHandler.Setup(mock => mock.SendSay(message));
        
            // Act
            _sut.Say(message);
        
            // Assert
            _mockedChatHandler.Verify(mock => mock.SendSay(message), Times.Once);
        }
        
        [Test]
        public void Test_Shout_CallsFunctionFromChatHandler()
        {
            // Arrange
            const string message = "hello world";
            _mockedChatHandler.Setup(mock => mock.SendShout(message));
        
            // Act
            _sut.Shout(message);
        
            // Assert
            _mockedChatHandler.Verify(mock => mock.SendShout(message), Times.Once);
        }

        [Test]
        public void Test_AddHealth_WithoutExceedingHealthCap()
        {
            //arrange
            _mockedPlayerModel.SetupGet(mock => mock.Health).Returns(50);
            
            //act
            _sut.AddHealth(40);
            
            //assert
            _mockedPlayerModel.VerifySet(mock => mock.Health = 90);
        }

        [Test]
        public void Test_AddHealth_ReachesHealthCap()
        {
            //arrange
            _mockedPlayerModel.Setup(mock => mock.Health).Returns(70);
            
            //act
            _sut.AddHealth(40);
            
            //assert
            _mockedPlayerModel.VerifySet(mock => mock.Health = 100);
        }
        
        [Test]
        public void Test_RemoveHealth_WithoutDying()
        {
            //arrange
            _mockedPlayerModel.Setup(mock => mock.Health).Returns(100);
            
            //act
            _sut.RemoveHealth(50);
            
            //assert
            _mockedPlayerModel.VerifySet(mock => mock.Health = 50);
        }

        [Test]
        public void Test_RemoveHealth_StopsAtDyingState()
        {
            //arrange
            _mockedPlayerModel.Setup(mock => mock.Health).Returns(100);
            
            //act
            _sut.RemoveHealth(200);
            
            //assert
            _mockedPlayerModel.VerifySet(mock => mock.Health = 0);
        }
        
        [Test]
        public void Test_AddStamina_WithoutExceedingStaminaCap()
        {
            //arrange
            _mockedPlayerModel.SetupGet(mock => mock.Stamina).Returns(5);
            
            //act
            _sut.AddStamina(4);
            
            //assert
            _mockedPlayerModel.VerifySet(mock => mock.Stamina = 9);
        }

        [Test]
        public void Test_AddStamina_ReachesStaminaCap()
        {
            //arrange
            _mockedPlayerModel.Setup(mock => mock.Stamina).Returns(7);
            
            //act
            _sut.AddStamina(4);
            
            //assert
            _mockedPlayerModel.VerifySet(mock => mock.Stamina = 10);
        }
        
        [Test]
        public void Test_RemoveStamina_WithoutDying()
        {
            //arrange
            _mockedPlayerModel.Setup(mock => mock.Stamina).Returns(10);
            
            //act
            _sut.RemoveStamina(5);
            
            //assert
            _mockedPlayerModel.VerifySet(mock => mock.Stamina = 5);
        }

        [Test]
        public void Test_RemoveStamina_StopsAtDyingState()
        {
            //arrange
            _mockedPlayerModel.Setup(mock => mock.Stamina).Returns(10);
            
            //act
            _sut.RemoveStamina(20);
            
            //assert
            _mockedPlayerModel.VerifySet(mock => mock.Stamina = 0);
        }
        
        [Test]
        public void Test_GetItem_VerifyFunctionCallsInventoryGetItem()
        {
            //arrange
            Item item = new Item("ItemName", "Description");
            _mockedPlayerModel.Setup(mock => mock.Inventory.GetItem("ItemName")).Returns(item);
            
            //act
            _sut.GetItem("ItemName");
            //assert
            _mockedPlayerModel.Verify(mock => mock.Inventory.GetItem("ItemName"), Times.Once);
        }
        
        [Test]
        public void Test_GetItem_GetsCorrectItem()
        {
            //arrange
            Item item = new Item("ItemName", "Description");
            _mockedPlayerModel.Setup(mock => mock.Inventory.GetItem("ItemName")).Returns(item);
            
            //act & assert
            Assert.AreEqual(item, _sut.GetItem("ItemName"));
        }

        [Test]
        public void Test_AddInventoryItem_AddsItemSuccessfully()
        {
            //arrange
            Item item = new Item("ItemName", "Description");
            _mockedPlayerModel.Setup(mock => mock.Inventory.AddItem(item));
            //act
            _sut.AddInventoryItem(item);
            //assert
            _mockedPlayerModel.Verify(mock => mock.Inventory.AddItem(item), Times.Once);
        }

        [Test]
        public void Test_RemoveInventoryItem_RemovesItemSuccessfully()
        {
            //arrange
            Item item = new Item("ItemName", "Description");
            _mockedPlayerModel.Setup(mock => mock.Inventory.RemoveItem(item));
            //act
            _sut.RemoveInventoryItem(item);
            //assert
            _mockedPlayerModel.Verify(mock => mock.Inventory.RemoveItem(item), Times.Once);
        }

        [Test]
        public void Test_EmptyInventory_EmptiesInventorySuccessfully()
        {
            //arrange
            Item item = new Item("ItemName", "Description");
            _mockedPlayerModel.Setup(mock => mock.Inventory.EmptyInventory());
            //act
            _sut.EmptyInventory();
            //assert
            _mockedPlayerModel.Verify(mock => mock.Inventory.EmptyInventory(), Times.Once);
        }

        [Test]
        public void Test_AddBitcoins_AddsBitcoinsSuccessfully()
        {
            //arrange
            Item item = new Item("ItemName", "Description");
            _mockedPlayerModel.Setup(mock => mock.Bitcoins.AddAmount(20));
            //act
            _sut.AddBitcoins(20);
            //assert
            _mockedPlayerModel.Verify(mock => mock.Bitcoins.AddAmount(20), Times.Once);
        }

        [Test]
        public void Test_RemoveBitcoins_RemovesBitcoinsSuccessfully()
        {
            //arrange
            Item item = new Item("ItemName", "Description");
            _mockedPlayerModel.Setup(mock => mock.Bitcoins.RemoveAmount(10));
            //act
            _sut.RemoveBitcoins(10);
            //assert
            _mockedPlayerModel.Verify(mock => mock.Bitcoins.RemoveAmount(10), Times.Once);
        }
        
        [Test]
        public void Test_GetAttackDamage_GetDefaultAttackDamage()
        {
            Assert.AreEqual(5, _sut.GetAttackDamage());
        }

        [Test]
        public void Test_DropItem_DropsItemSuccessfully()
        {
            //arrange
            Item item = new Item("ItemName", "Description");
            _mockedPlayerModel.Setup(mock => mock.Inventory.GetItem("ItemName")).Returns(item);
            _mockedPlayerModel.Setup(mock => mock.Inventory.RemoveItem(item));
            //act
            _sut.DropItem("ItemName");
            //assert
            _mockedPlayerModel.Verify(mock => mock.Inventory.RemoveItem(item), Times.Once);
        }

        [Test]
        public void Test_DropItem_ThrowsExceptionBecauseNoItemExists()
        {
            _mockedPlayerModel.Setup(mock => mock.Inventory.GetItem("ItemName"));

            Assert.Throws<ItemException>(() => _sut.DropItem("ItemName"));
        }

        [TestCase("up")]
        [TestCase("down")]
        [TestCase("left")]
        [TestCase("right")]
        [Test]
        public void Test_GetCurrentPosition_VerifyPlayerModelMoqWorks1(string move)
        {
            //arrange
            var direction_right = move;
            int steps = 5;
            int x = 26;
            int y = 11;
            
            MapCharacterDTO test = new MapCharacterDTO(x,y, "test", "test2", "#", ConsoleColor.White, ConsoleColor.Black, 0);
            MapCharacterDTO expected = new MapCharacterDTO(x+5,y+0, "test", "test2", "#", ConsoleColor.White, ConsoleColor.Black, 0);
            
            _mockedWorldService.Setup(mock => mock.getCurrentCharacterPositions()).Returns(test);
            _mockedMoveHandler.Setup(mock => mock.SendMove(expected));
            _mockedPlayerModel.Setup(mock => mock.PlayerGuid).Returns("test");
            _mockedPlayerModel.Setup(mock => mock.Symbol).Returns("#");
            
            //act
            _sut.HandleDirection(direction_right, steps);
            
            //assert
            _mockedWorldService.Verify(mock => mock.getCurrentCharacterPositions(), Times.Exactly(3));
            _mockedPlayerModel.Verify(mock => mock.PlayerGuid, Times.Once);
            _mockedPlayerModel.Verify(mock => mock.Symbol, Times.Once);
            _mockedMoveHandler.Verify(mock => mock.SendMove(It.IsAny<MapCharacterDTO>()), Times.Once);
        }
    }
}
