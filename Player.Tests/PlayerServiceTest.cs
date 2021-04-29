using System.Diagnostics.CodeAnalysis;
using Moq;
using NUnit.Framework;
using Player.Model;
using Player.Services;

namespace Player.Tests
{
    [ExcludeFromCodeCoverage]
    public class PlayerServiceTest
    {
        private PlayerService sut;
        private Mock<IPlayerModel> _mockedPlayerModel;
        
        [SetUp]
        public void Setup()
        {
            _mockedPlayerModel = new Mock<IPlayerModel>();
            sut = new PlayerService(_mockedPlayerModel.Object);
        }
        
        [Test]
        public void Test_AddHealth_CallsFunctionFromPlayerModel()
        {
            int health = 10;
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.AddHealth(health));
            
            sut.AddHealth(health);
            
            _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.AddHealth(health));
        }
        
        [Test]
        public void Test_RemoveHealth_CallsFunctionFromPlayerModel()
        {
            int health = 10;
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.RemoveHealth(health));
            
            sut.RemoveHealth(health);
            
            _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.RemoveHealth(health));
        }
        
        [Test]
        public void Test_AddStamina_CallsFunctionFromPlayerModel()
        {
            int stamina = 10;
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.AddStamina(stamina));
            
            sut.AddStamina(stamina);
            
            _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.AddStamina(stamina));
        }
        
        [Test]
        public void Test_RemoveStamina_CallsFunctionFromPlayerModel()
        {
            int stamina = 10;
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.RemoveStamina(stamina));
            
            sut.RemoveStamina(stamina);
            
            _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.RemoveStamina(stamina));
        }
        
        [Test]
        public void Test_GetItem_CallsFunctionFromPlayerModel()
        {
            Item item = new Item("ItemName", "Description");
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.GetItem("ItemName")).Returns(item);

            Assert.AreEqual(item, sut.GetItem("ItemName"));
            _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.GetItem("ItemName"));
        }
        
        [Test]
        public void Test_AddInventoryItem_CallsFunctionFromPlayerModel()
        {
            Item item = new Item("ItemName", "Description");
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.AddInventoryItem(item));

            sut.AddInventoryItem(item);
            
            _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.AddInventoryItem(item));
        }
        
        [Test]
        public void Test_RemoveInventoryItem_CallsFunctionFromPlayerModel()
        {
            Item item = new Item("ItemName", "Description");
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.RemoveInventoryItem(item));

            sut.RemoveInventoryItem(item);
            
            _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.RemoveInventoryItem(item));
        }
        
        [Test]
        public void Test_EmptyInventory_CallsFunctionFromPlayerModel()
        {
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.EmptyInventory());

            sut.EmptyInventory();
            
            _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.EmptyInventory());
        }
        
        [Test]
        public void Test_AddBitcoins_CallsFunctionFromPlayerModel()
        {
            int amount = 10;
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.AddBitcoins(amount));

            sut.AddBitcoins(amount);
            
            _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.AddBitcoins(amount));
        }
        
        [Test]
        public void Test_RemoveBitcoins_CallsFunctionFromPlayerModel()
        {
            int amount = 10;
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.RemoveBitcoins(amount));

            sut.RemoveBitcoins(amount);
            
            _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.RemoveBitcoins(amount));
        }
        
        [Test]
        public void Test_GetAttackDamage_CallsFunctionFromPlayerModel()
        {
            int dmg = 5;
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.GetAttackDamage()).Returns(dmg);

            Assert.AreEqual(dmg, sut.GetAttackDamage());
            _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.GetAttackDamage());
        }
        
        [Test]
        public void Test_PickupItem_CallsFunctionFromPlayerModel()
        {
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.PickupItem());

            sut.PickupItem();
            
            _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.PickupItem());
        }
        
        [Test]
        public void Test_DropItem_CallsFunctionFromPlayerModel()
        {
            string itemName = "Test";
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.DropItem(itemName));

            sut.DropItem(itemName);
            
            _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.DropItem(itemName));
        }
        
        [Test]
        public void Test_GetCurrentPosition_VerifyPlayerModelMoqWorks1()
        {
            var direction_right = "right";
            int steps = 5;
            int[] newMovement = {steps, 0};
            int[] playerPosition = {31, 11};
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.SetNewPlayerPosition(newMovement));
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.CurrentPosition).Returns(playerPosition);

            sut.HandleDirection(direction_right, steps);
            
            _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.SetNewPlayerPosition(newMovement), Times.Once);
        }
        
        [Test]
        public void Test_GetCurrentPosition_VerifyPlayerModelMoqWorks2()
        {
            var direction_left = "left";
            int steps = 5;
            int[] newMovement = {-steps, 0};
            int[] playerPosition = {31, 11};
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.SetNewPlayerPosition(newMovement));
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.CurrentPosition).Returns(playerPosition);

            sut.HandleDirection(direction_left, steps);
            
            _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.SetNewPlayerPosition(newMovement), Times.Once);
        }
        
        [Test]
        public void Test_GetCurrentPosition_VerifyPlayerModelMoqWorks3()
        {
            var direction_left = "forward";
            int steps = 5;
            int[] newMovement = {0, -steps};
            int[] playerPosition = {31, 11};
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.SetNewPlayerPosition(newMovement));
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.CurrentPosition).Returns(playerPosition);

            sut.HandleDirection(direction_left, steps);
            
            _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.SetNewPlayerPosition(newMovement), Times.Once);
        }
        
        [Test]
        public void Test_GetCurrentPosition_VerifyPlayerModelMoqWorks4()
        {
            var direction_left = "backward";
            int steps = 5;
            int[] newMovement = {0, steps};
            int[] playerPosition = {31, 11};
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.SetNewPlayerPosition(newMovement));
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.CurrentPosition).Returns(playerPosition);

            sut.HandleDirection(direction_left, steps);
            
            _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.SetNewPlayerPosition(newMovement), Times.Once);
        }
    }
}