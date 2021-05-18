using System.Diagnostics.CodeAnalysis;
using Moq;
using NUnit.Framework;
using Player.Exceptions;
using Player.Model;

namespace Player.Tests
{
    [ExcludeFromCodeCoverage]
    public class PlayerModelTest
    {
        private PlayerModel _sut;
        private Mock<IInventory> _mockedInventory;
        private Mock<IBitcoin> _mockedBitcoins;
        private Mock<IRadiationLevel> _mockedRadiationLevel;

        [SetUp]
        public void Setup()
        {
            _mockedInventory = new Mock<IInventory>();
            _mockedBitcoins = new Mock<IBitcoin>();
            _mockedRadiationLevel = new Mock<IRadiationLevel>();
            _sut = new PlayerModel("Jan", _mockedInventory.Object, _mockedBitcoins.Object, _mockedRadiationLevel.Object);
        }
        
        [Test]
        public void Test_GetName_GetsNameSuccessfully()
        {
            Assert.AreEqual("Jan", _sut.Name);
        }
        
        [Test]
        public void Test_SetName_SetsNameSuccessfully()
        {
            var name = "New Name";
            _sut.Name = name;
            
            Assert.AreEqual(name, _sut.Name);
        }
        
        [Test]
        public void Test_GetHealth_GetsHealthSuccessfully()
        {
            Assert.AreEqual(100, _sut.Health);
        }
        
        [Test]
        public void Test_SetHealth_SetsHealthSuccessfully()
        {
            var HP = 50;
            _sut.Health = HP;
            
            Assert.AreEqual(HP, _sut.Health);
        }
        
        [Test]
        public void Test_GetStamina_GetsStaminaSuccessfully()
        {
            Assert.AreEqual(10, _sut.Stamina);
        }
        
        [Test]
        public void Test_SetStamina_SetsStaminaSuccessfully()
        {
            var stamina = 5;
            _sut.Stamina = stamina;
            
            Assert.AreEqual(stamina, _sut.Stamina);
        }
        
        [Test]
        public void Test_GetInventory_GetsInventorySuccessfully()
        {
            Assert.AreEqual(_mockedInventory.Object, _sut.Inventory);
        }
        
        [Test]
        public void Test_SetInventory_SetsInventorySuccessfully()
        {
            var inventory = new Inventory();
            _sut.Inventory = inventory;
            
            Assert.AreEqual(inventory, _sut.Inventory);
        }
        
        [Test]
        public void Test_GetBitcoins_GetsBitcoinsSuccessfully()
        {
            Assert.AreEqual(_mockedBitcoins.Object, _sut.Bitcoins);
        }
        
        [Test]
        public void Test_SetBitcoins_SetsBitcoinsSuccessfully()
        {
            var bitcoins = new Bitcoin(5);
            _sut.Bitcoins = bitcoins;
            
            Assert.AreEqual(bitcoins, _sut.Bitcoins);
        }
        
        [Test]
        public void Test_GetRadiationLevel_GetsRadiationLevelSuccessfully()
        {
            Assert.AreEqual(_mockedRadiationLevel.Object, _sut.RadiationLevel);
        }
        
        [Test]
        public void Test_SetRadiationLevel_SetsRadiationLevelSuccessfully()
        {
            var radiationLevel = new RadiationLevel(5);
            _sut.RadiationLevel = radiationLevel;
            
            Assert.AreEqual(radiationLevel, _sut.RadiationLevel);
        }
        
        [Test]
        public void Test_GetCurrentPosition_GetsCurrentPositionSuccessfully()
        {
            int expectedX = 26;
            int expectedY = 11;
            
            Assert.AreEqual(expectedX, _sut.XPosition);
            Assert.AreEqual(expectedY, _sut.YPosition);
        }
        
        [Test]
        public void Test_SetCurrentPosition_SetsCurrentPositionSuccessfully()
        {
            int expectedX = 27;
            int expectedY = 12;
            
            _sut.XPosition = expectedX;
            _sut.YPosition = expectedY;
            
            Assert.AreEqual(expectedX, _sut.XPosition);
            Assert.AreEqual(expectedY, _sut.YPosition);
        }

        [Test]
        public void Test_RemoveHealth_WithoutDying()
        {
            _sut.RemoveHealth(50);
            
            Assert.AreEqual(50, _sut.Health);
        }
        
        [Test]
        public void Test_RemoveHealth_StopsAtDyingState()
        {
            _sut.RemoveHealth(200);
            
            Assert.AreEqual(0, _sut.Health);
        }
        
        [Test]
        public void Test_AddHealth_WithoutExceedingHealthCap()
        {
            _sut.RemoveHealth(50);
            
            _sut.AddHealth(40);
            
            Assert.AreEqual(90, _sut.Health);
        }
        
        [Test]
        public void Test_AddHealth_ReachesHealthCap()
        {
            _sut.RemoveHealth(30);
            
            _sut.AddHealth(40);
            
            Assert.AreEqual(100, _sut.Health);
        }
        
        [Test]
        public void Test_RemoveStamina_WithoutRunningOutOfMana()
        {
            _sut.RemoveStamina(5);
            
            Assert.AreEqual(5, _sut.Stamina);
        }
        
        [Test]
        public void Test_RemoveStamina_StopsAtNoMana()
        {
            _sut.RemoveStamina(20);
            
            Assert.AreEqual(0, _sut.Stamina);
        }
        
        [Test]
        public void Test_AddStamina_WithoutExceedingStaminaCap()
        {
            _sut.RemoveStamina(5);
            
            _sut.AddStamina(4);
            
            Assert.AreEqual(9, _sut.Stamina);
        }
        
        [Test]
        public void Test_AddStamina_ReachesStaminaCap()
        {
            _sut.RemoveStamina(3);
            
            _sut.AddStamina(4);
            
            Assert.AreEqual(10, _sut.Stamina);
        }
        
        [Test]
        public void Test_GetItem_VerifyInventoryMoqWorks()
        {
            Item item = new Item("ItemName", "Description");
            _mockedInventory.Setup(mockedInventory => mockedInventory.GetItem("ItemName")).Returns(item);
            
            Assert.AreEqual(item, _sut.GetItem("ItemName"));
            _mockedInventory.Verify(mockedInventory => mockedInventory.GetItem("ItemName"), Times.Once);
        }
        
        [Test]
        public void Test_AddInventoryItem_AddsItemSuccessfully()
        {
            Item item = new Item("ItemName", "Description");
            _mockedInventory.Setup(mockedInventory => mockedInventory.AddItem(item));

            _sut.AddInventoryItem(item);
            
            _mockedInventory.Verify(mockedInventory => mockedInventory.AddItem(item), Times.Once);
        }
        
        [Test]
        public void Test_RemoveInventoryItem_RemovesItemSuccessfully()
        {
            Item item = new Item("ItemName", "Description");
            _mockedInventory.Setup(mockedInventory => mockedInventory.RemoveItem(item));

            _sut.RemoveInventoryItem(item);
            
            _mockedInventory.Verify(mockedInventory => mockedInventory.RemoveItem(item), Times.Once);
        }
        
        [Test]
        public void Test_EmptyInventory_EmptiesInventorySuccessfully()
        {
            _mockedInventory.Setup(mockedInventory => mockedInventory.EmptyInventory());

            _sut.EmptyInventory();
            
            _mockedInventory.Verify(mockedInventory => mockedInventory.EmptyInventory(), Times.Once);
        }
        
        [Test]
        public void Test_GetAmount_VerifyBitcoinMoqWorks()
        {
            _mockedBitcoins.Setup(mockedBitcoins => mockedBitcoins.Amount).Returns(20);

            Assert.AreEqual(20, _sut.Bitcoins.Amount);
            _mockedBitcoins.Verify(mockedBitcoins => mockedBitcoins.Amount, Times.Once);
        }
        
        [Test]
        public void Test_AddBitcoins_AddsBitcoinsSuccessfully()
        {
            _mockedBitcoins.Setup(mockedBitcoins => mockedBitcoins.AddAmount(20));

            _sut.AddBitcoins(20);
            
            _mockedBitcoins.Verify(mockedBitcoins => mockedBitcoins.AddAmount(20), Times.Once);
        }
        
        [Test]
        public void Test_RemoveBitcoins_RemovesBitcoinsSuccessfully()
        {
            _mockedBitcoins.Setup(mockedBitcoins => mockedBitcoins.RemoveAmount(10));

            _sut.RemoveBitcoins(10);
            
            _mockedBitcoins.Verify(mockedBitcoins => mockedBitcoins.RemoveAmount(10), Times.Once);
        }
        
        [Test]
        public void Test_DropItem_DropsItemSuccessfully()
        {
            Item item = new Item("ItemName", "Description");
            _mockedInventory.Setup(mockedInventory => mockedInventory.GetItem("ItemName")).Returns(item);
            _mockedInventory.Setup(mockedInventory => mockedInventory.RemoveItem(item));

            _sut.DropItem("ItemName");
            
            _mockedInventory.Verify(mockedInventory => mockedInventory.RemoveItem(item), Times.Once);
        }
        
        [Test]
        public void Test_DropItem_ThrowsExceptionBecauseNoItemExists()
        {
            _mockedInventory.Setup(mockedInventory => mockedInventory.GetItem("ItemName"));
        
            Assert.Throws<ItemException>(() => _sut.DropItem("ItemName"));
        }
        
        [Test]
        public void Test_GetAttackDamage_GetDefaultAttackDamage()
        {
            Assert.AreEqual(5, _sut.GetAttackDamage());
        }
        
        [Test]
        public void Test_GetLevel_VerifyRadiationLevelMoqWorks()
        {
            _mockedRadiationLevel.Setup(mockedRadiationLevel => mockedRadiationLevel.Level).Returns(1);

            Assert.AreEqual(1, _sut.RadiationLevel.Level);
            _mockedRadiationLevel.Verify(mockedRadiationLevel => mockedRadiationLevel.Level, Times.Once);
        }
        
        [Test]
        public void Test_SetNewPlayerPosition_SetsNewPlayerPosition()
        {
            int[] test = {0, 5};
            int expectedX = 26;
            int expectedY = 16;
            
            _sut.SetNewPlayerPosition(0, 5);
            
            Assert.AreEqual(expectedX, _sut.XPosition);
            Assert.AreEqual(expectedY, _sut.YPosition);
        }
    }
}