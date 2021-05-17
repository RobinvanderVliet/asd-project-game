using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Moq;
using NUnit.Framework;
using Player.ActionHandlers;
using Player.DTO;
using Player.Model;
using Player.Services;

namespace Player.Tests
{
    [ExcludeFromCodeCoverage]
    public class PlayerServiceTest
    {
        private PlayerService _sut;
        private Mock<IPlayerModel> _mockedPlayerModel;
        private List<PlayerDTO> _mockedPlayerList;
        private Mock<IMoveHandler> _mockedMoveHandler;
        
        [SetUp]
        public void Setup()
        {
            _mockedPlayerModel = new Mock<IPlayerModel>();
            _mockedPlayerList = new List<PlayerDTO>();
            _mockedMoveHandler = new Mock<IMoveHandler>();
            _sut = new PlayerService(_mockedPlayerModel.Object, _mockedPlayerList, _mockedMoveHandler.Object);
        }
        
        [Test]
        public void Test_AddHealth_CallsFunctionFromPlayerModel()
        {
            int health = 10;
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.AddHealth(health));
            
            _sut.AddHealth(health);
            
            _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.AddHealth(health));
        }
        
        [Test]
        public void Test_RemoveHealth_CallsFunctionFromPlayerModel()
        {
            int health = 10;
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.RemoveHealth(health));
            
            _sut.RemoveHealth(health);
            
            _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.RemoveHealth(health));
        }
        
        [Test]
        public void Test_AddStamina_CallsFunctionFromPlayerModel()
        {
            int stamina = 10;
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.AddStamina(stamina));
            
            _sut.AddStamina(stamina);
            
            _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.AddStamina(stamina));
        }
        
        [Test]
        public void Test_RemoveStamina_CallsFunctionFromPlayerModel()
        {
            int stamina = 10;
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.RemoveStamina(stamina));
            
            _sut.RemoveStamina(stamina);
            
            _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.RemoveStamina(stamina));
        }
        
        [Test]
        public void Test_GetItem_CallsFunctionFromPlayerModel()
        {
            Item item = new Item("ItemName", "Description");
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.GetItem("ItemName")).Returns(item);

            Assert.AreEqual(item, _sut.GetItem("ItemName"));
            _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.GetItem("ItemName"));
        }
        
        [Test]
        public void Test_AddInventoryItem_CallsFunctionFromPlayerModel()
        {
            Item item = new Item("ItemName", "Description");
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.AddInventoryItem(item));

            _sut.AddInventoryItem(item);
            
            _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.AddInventoryItem(item));
        }
        
        [Test]
        public void Test_RemoveInventoryItem_CallsFunctionFromPlayerModel()
        {
            Item item = new Item("ItemName", "Description");
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.RemoveInventoryItem(item));

            _sut.RemoveInventoryItem(item);
            
            _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.RemoveInventoryItem(item));
        }
        
        [Test]
        public void Test_EmptyInventory_CallsFunctionFromPlayerModel()
        {
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.EmptyInventory());

            _sut.EmptyInventory();
            
            _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.EmptyInventory());
        }
        
        [Test]
        public void Test_AddBitcoins_CallsFunctionFromPlayerModel()
        {
            int amount = 10;
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.AddBitcoins(amount));

            _sut.AddBitcoins(amount);
            
            _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.AddBitcoins(amount));
        }
        
        [Test]
        public void Test_RemoveBitcoins_CallsFunctionFromPlayerModel()
        {
            int amount = 10;
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.RemoveBitcoins(amount));

            _sut.RemoveBitcoins(amount);
            
            _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.RemoveBitcoins(amount));
        }
        
        [Test]
        public void Test_GetAttackDamage_CallsFunctionFromPlayerModel()
        {
            int dmg = 5;
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.GetAttackDamage()).Returns(dmg);

            Assert.AreEqual(dmg, _sut.GetAttackDamage());
            _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.GetAttackDamage());
        }
        
        [Test]
        public void Test_PickupItem_CallsFunctionFromPlayerModel()
        {
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.PickupItem());

            _sut.PickupItem();
            
            _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.PickupItem());
        }
        
        [Test]
        public void Test_DropItem_CallsFunctionFromPlayerModel()
        {
            string itemName = "Test";
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.DropItem(itemName));

            _sut.DropItem(itemName);
            
            _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.DropItem(itemName));
        }
        
        [Test]
        public void Test_GetCurrentPosition_VerifyPlayerModelMoqWorks1()
        {
            var direction_right = "right";
            int steps = 5;
            int x = 26;
            int y = 11;
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.SetNewPlayerPosition(steps, 0));
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.XPosition).Returns(x);
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.YPosition).Returns(y);;

            _sut.HandleDirection(direction_right, steps);
            
            _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.SetNewPlayerPosition(steps, 0), Times.Once);
        }
        
        [Test]
        public void Test_GetCurrentPosition_VerifyPlayerModelMoqWorks2()
        {
            var direction_left = "left";
            int steps = 5;
            int x = 26;
            int y = 11;
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.SetNewPlayerPosition(-steps, 0));
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.XPosition).Returns(x);
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.YPosition).Returns(y);

            _sut.HandleDirection(direction_left, steps);
            
            _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.SetNewPlayerPosition(-steps, 0), Times.Once);
        }
        
        [Test]
        public void Test_GetCurrentPosition_VerifyPlayerModelMoqWorks3()
        {
            var direction_left = "forward";
            int steps = 5;
            int x = 26;
            int y = 11;
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.SetNewPlayerPosition(0, -steps));
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.XPosition).Returns(x);
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.YPosition).Returns(y);

            _sut.HandleDirection(direction_left, steps);
            
            _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.SetNewPlayerPosition(0, -steps), Times.Once);
        }
        
        [Test]
        public void Test_GetCurrentPosition_VerifyPlayerModelMoqWorks4()
        {
            var direction_left = "backward";
            int steps = 5;
            int x = 26;
            int y = 11;
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.SetNewPlayerPosition(0, steps));
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.XPosition).Returns(x);
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.YPosition).Returns(y);

            _sut.HandleDirection(direction_left, steps);
            
            _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.SetNewPlayerPosition(0, steps), Times.Once);
        }
    }
}