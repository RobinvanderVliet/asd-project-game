using System.Collections.Generic;
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
using Session;
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
        private Mock<MapCharacterDTO> _mockedMapCharacterDTO;
        private Mock<SessionHandler> _mockedSessionHandler;

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
            _mockedPlayerModel.SetupGet(_mockedPlayerModel => _mockedPlayerModel.Health).Returns(50);
            
            //act
            _sut.AddHealth(40);
            
            //assert
            _mockedPlayerModel.VerifySet(_mockedPlayerModel => _mockedPlayerModel.Health = 90);
        }

        [Test]
        public void Test_AddHealth_ReachesHealthCap()
        {
            //arrange
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.Health).Returns(70);
            
            //act
            _sut.AddHealth(40);
            
            //assert
            _mockedPlayerModel.VerifySet(_mockedPlayerModel => _mockedPlayerModel.Health = 100);
        }
        
        [Test]
        public void Test_RemoveHealth_WithoutDying()
        {
            //arrange
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.Health).Returns(100);
            
            //act
            _sut.RemoveHealth(50);
            
            //assert
            _mockedPlayerModel.VerifySet(_mockedPlayerModel => _mockedPlayerModel.Health = 50);
        }

        [Test]
        public void Test_RemoveHealth_StopsAtDyingState()
        {
            //arrange
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.Health).Returns(100);
            
            //act
            _sut.RemoveHealth(200);
            
            //assert
            _mockedPlayerModel.VerifySet(_mockedPlayerModel => _mockedPlayerModel.Health = 0);
        }
        
        [Test]
        public void Test_AddStamina_WithoutExceedingStaminaCap()
        {
            //arrange
            _mockedPlayerModel.SetupGet(_mockedPlayerModel => _mockedPlayerModel.Stamina).Returns(5);
            
            //act
            _sut.AddStamina(4);
            
            //assert
            _mockedPlayerModel.VerifySet(_mockedPlayerModel => _mockedPlayerModel.Stamina = 9);
        }

        [Test]
        public void Test_AddStamina_ReachesStaminaCap()
        {
            //arrange
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.Stamina).Returns(7);
            
            //act
            _sut.AddStamina(4);
            
            //assert
            _mockedPlayerModel.VerifySet(_mockedPlayerModel => _mockedPlayerModel.Stamina = 10);
        }
        
        [Test]
        public void Test_RemoveStamina_WithoutDying()
        {
            //arrange
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.Stamina).Returns(10);
            
            //act
            _sut.RemoveStamina(5);
            
            //assert
            _mockedPlayerModel.VerifySet(_mockedPlayerModel => _mockedPlayerModel.Stamina = 5);
        }

        [Test]
        public void Test_RemoveStamina_StopsAtDyingState()
        {
            //arrange
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.Stamina).Returns(10);
            
            //act
            _sut.RemoveStamina(20);
            
            //assert
            _mockedPlayerModel.VerifySet(_mockedPlayerModel => _mockedPlayerModel.Stamina = 0);
        }
        
        [Test]
        public void Test_GetItem_VerifyFunctionCallsInventoryGetItem()
        {
            //arrange
            Item item = new Item("ItemName", "Description");
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.Inventory.GetItem("ItemName")).Returns(item);
            
            //act
            _sut.GetItem("ItemName");
            //assert
            _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.Inventory.GetItem("ItemName"), Times.Once);
        }
        
        [Test]
        public void Test_GetItem_GetsCorrectItem()
        {
            //arrange
            Item item = new Item("ItemName", "Description");
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.Inventory.GetItem("ItemName")).Returns(item);
            
            //act & assert
            Assert.AreEqual(item, _sut.GetItem("ItemName"));
        }

        [Test]
        public void Test_AddInventoryItem_AddsItemSuccessfully()
        {
            //arrange
            Item item = new Item("ItemName", "Description");
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.Inventory.AddItem(item));
            //act
            _sut.AddInventoryItem(item);
            //assert
            _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.Inventory.AddItem(item), Times.Once);
        }

        [Test]
        public void Test_RemoveInventoryItem_RemovesItemSuccessfully()
        {
            //arrange
            Item item = new Item("ItemName", "Description");
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.Inventory.RemoveItem(item));
            //act
            _sut.RemoveInventoryItem(item);
            //assert
            _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.Inventory.RemoveItem(item), Times.Once);
        }

        [Test]
        public void Test_EmptyInventory_EmptiesInventorySuccessfully()
        {
            //arrange
            Item item = new Item("ItemName", "Description");
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.Inventory.EmptyInventory());
            //act
            _sut.EmptyInventory();
            //assert
            _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.Inventory.EmptyInventory(), Times.Once);
        }

        [Test]
        public void Test_AddBitcoins_AddsBitcoinsSuccessfully()
        {
            //arrange
            Item item = new Item("ItemName", "Description");
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.Bitcoins.AddAmount(20));
            //act
            _sut.AddBitcoins(20);
            //assert
            _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.Bitcoins.AddAmount(20), Times.Once);
        }

        [Test]
        public void Test_RemoveBitcoins_RemovesBitcoinsSuccessfully()
        {
            //arrange
            Item item = new Item("ItemName", "Description");
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.Bitcoins.RemoveAmount(10));
            //act
            _sut.RemoveBitcoins(10);
            //assert
            _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.Bitcoins.RemoveAmount(10), Times.Once);
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
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.Inventory.GetItem("ItemName")).Returns(item);
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.Inventory.RemoveItem(item));
            //act
            _sut.DropItem("ItemName");
            //assert
            _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.Inventory.RemoveItem(item), Times.Once);
        }

        [Test]
        public void Test_DropItem_ThrowsExceptionBecauseNoItemExists()
        {
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.Inventory.GetItem("ItemName"));

            Assert.Throws<ItemException>(() => _sut.DropItem("ItemName"));
        }

        // [Test]
        // public void Test_GetCurrentPosition_VerifyPlayerModelMoqWorks1()
        // {
        //     //arrange
        //     var direction_right = "right";
        //     int steps = 5;
        //     int x = 26;
        //     int y = 11;
        //     
        //     //ff checken of dit in de setup kan
        //     _mockedMapCharacterDTO = new Mock<MapCharacterDTO>(x,y, "test", "test2");
        //     
        //     _mockedMapCharacterDTO.SetupSet(mock => mock.XPosition = x);
        //     _mockedMapCharacterDTO.SetupSet(mock => mock.YPosition = y);
        //     // _mockedMapCharacterDTO.Setup(_mockedMapCharacterDTO => _mockedMapCharacterDTO.YPosition >> y);
        //
        //     _mockedWorldService.Setup(mock => mock.getCurrentCharacterPositions()).Returns(_mockedMapCharacterDTO.Object);
        //     _mockedMoveHandler.Setup(_mockedMoveHandler => _mockedMoveHandler.SendMove());
        //     
        //     //act
        //     _sut.HandleDirection(direction_right, steps);
        //     
        //     //assert
        //     // _mockedMapCharacterDTO.Verify(mock => mock.XPosition == x);
        //     // _mockedMapCharacterDTO.Verify(mock => mock.YPosition == y);
        //     _mockedMapCharacterDTO.Verify(_mockedMapCharacterDTO => _mockedMapCharacterDTO.XPosition, Times.Once);
        //     _mockedMapCharacterDTO.Verify(_mockedMapCharacterDTO => _mockedMapCharacterDTO.YPosition, Times.Once);
        //
        // }

        // [Test]
        // public void Test_GetCurrentPosition_VerifyPlayerModelMoqWorks2()
        // {
        //     var direction_left = "left";
        //     int steps = 5;
        //     int x = 26;
        //     int y = 11;
        //     
        //     _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.XPosition).Returns(x);
        //     _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.YPosition).Returns(y);
        //     
        //     _mockedMoveHandler.Setup(_mockedMoveHandler => _mockedMoveHandler.SendMove())
        //
        //     _sut.HandleDirection(direction_left, steps);
        //     
        //     _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.XPosition, Times.Once);
        //     _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.YPosition, Times.Once);
        // }
        //
        // [Test]
        // public void Test_GetCurrentPosition_VerifyPlayerModelMoqWorks3()
        // {
        //     var direction_left = "forward";
        //     int steps = 5;
        //     int x = 26;
        //     int y = 11;
        //     _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.SetNewPlayerPosition(0, -steps));
        //     _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.XPosition).Returns(x);
        //     _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.YPosition).Returns(y);
        //
        //     _sut.HandleDirection(direction_left, steps);
        //     
        //     _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.SetNewPlayerPosition(0, -steps), Times.Once);
        // }
        //
        // [Test]
        // public void Test_GetCurrentPosition_VerifyPlayerModelMoqWorks4()
        // {
        //     var direction_left = "backward";
        //     int steps = 5;
        //     int x = 26;
        //     int y = 11;
        //     _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.SetNewPlayerPosition(0, steps));
        //     _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.XPosition).Returns(x);
        //     _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.YPosition).Returns(y);
        //
        //     _sut.HandleDirection(direction_left, steps);
        //     
        //     _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.SetNewPlayerPosition(0, steps), Times.Once);
        // }
        //

        // [Test]
        // public void Test_CreateSession_CallsFunctionFromSessionHandler()
        // {
        //     // Arrange
        //     const string sessionName = "cool world";
        //     _mockedSessionHandler.Setup(mock => mock.CreateSession(sessionName));
        //
        //     // Act
        //     _sut.CreateSession(sessionName);
        //
        //     // Assert
        //     _mockedSessionHandler.Verify(mock => mock.CreateSession(sessionName), Times.Once);
        // }
        //
        // [Test]
        // public void Test_RequestSessions_CallsFunctionFromSessionHandler()
        // {
        //     // Arrange
        //     _mockedSessionHandler.Setup(mock => mock.RequestSessions());
        //
        //     // Act
        //     _sut.RequestSessions();
        //     
        //     // Assert
        //     _mockedSessionHandler.Verify(mock => mock.RequestSessions(), Times.Once);
        // }
        //
        // [Test]
        // public void Test_JoinSession_CallsFunctionFromSessionHandler()
        // {
        //     // Arrange
        //     const string sessionId = "1234-1234";
        //     _mockedSessionHandler.Setup(mock => mock.JoinSession(sessionId));
        //     
        //     // Act
        //     _sut.JoinSession(sessionId);
        //     
        //     // Assert
        //     _mockedSessionHandler.Verify(mock => mock.JoinSession(sessionId));
        // }
    }
}
