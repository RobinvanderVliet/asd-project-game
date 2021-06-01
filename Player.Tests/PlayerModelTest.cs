// should be deleted

/*using System.Diagnostics.CodeAnalysis;
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
            //arrange
            //act
            //assert
            Assert.AreEqual("Jan", _sut.Name);
        }
        
        [Test]
        public void Test_SetName_SetsNameSuccessfully()
        {
            //arrange
            var name = "New Name";
            //act
            _sut.Name = name;
            //assert
            Assert.AreEqual(name, _sut.Name);
        }
        
        [Test]
        public void Test_GetHealth_GetsHealthSuccessfully()
        {
            //arrange
            //act
            //assert
            Assert.AreEqual(100, _sut.Health);
        }
        
        [Test]
        public void Test_SetHealth_SetsHealthSuccessfully()
        {
            //arrange
            var HP = 50;
            //act
            _sut.Health = HP;
            //assert
            Assert.AreEqual(HP, _sut.Health);
        }
        
        [Test]
        public void Test_GetStamina_GetsStaminaSuccessfully()
        {
            //arrange
            //act
            //assert
            Assert.AreEqual(10, _sut.Stamina);
        }
        
        [Test]
        public void Test_SetStamina_SetsStaminaSuccessfully()
        {
            //arrange
            var stamina = 5;
            //act
            _sut.Stamina = stamina;
            //assert
            Assert.AreEqual(stamina, _sut.Stamina);
        }
        
        [Test]
        public void Test_GetInventory_GetsInventorySuccessfully()
        {
            //arrange
            //act
            //assert
            Assert.AreEqual(_mockedInventory.Object, _sut.Inventory);
        }
        
        [Test]
        public void Test_SetInventory_SetsInventorySuccessfully()
        {
            //arrange
            var inventory = new Inventory();
            //act
            _sut.Inventory = inventory;
            //assert
            Assert.AreEqual(inventory, _sut.Inventory);
        }
        
        [Test]
        public void Test_GetBitcoins_GetsBitcoinsSuccessfully()
        {
            //arrange
            //act
            //assert
            Assert.AreEqual(_mockedBitcoins.Object, _sut.Bitcoins);
        }
        
        [Test]
        public void Test_SetBitcoins_SetsBitcoinsSuccessfully()
        {
            //arrange
            var bitcoins = new Bitcoin(5);
            //act
            _sut.Bitcoins = bitcoins;
            //assert
            Assert.AreEqual(bitcoins, _sut.Bitcoins);
        }
        
        [Test]
        public void Test_GetRadiationLevel_GetsRadiationLevelSuccessfully()
        {
            //arrange
            //act
            //assert
            Assert.AreEqual(_mockedRadiationLevel.Object, _sut.RadiationLevel);
        }
        
        [Test]
        public void Test_SetRadiationLevel_SetsRadiationLevelSuccessfully()
        {
            //arrange
            var radiationLevel = new RadiationLevel(5);
            //act
            _sut.RadiationLevel = radiationLevel;
            //assert
            Assert.AreEqual(radiationLevel, _sut.RadiationLevel);
        }
        
        [Test]
        public void Test_SetCurrentPosition_SetsCurrentPositionSuccessfully()
        {
            //arrange
            int expectedX = 27;
            int expectedY = 12;
            //act
            _sut.XPosition = expectedX;
            _sut.YPosition = expectedY;
            //assert
            Assert.AreEqual(expectedX, _sut.XPosition);
            Assert.AreEqual(expectedY, _sut.YPosition);
        }

        [Test]
        public void Test_RemoveHealth_WithoutDying()
        {
            //arrange
            //act
            _sut.RemoveHealth(50);
            //assert
            Assert.AreEqual(50, _sut.Health);
        }
        
        [Test]
        public void Test_RemoveHealth_StopsAtDyingState()
        {
            //arrange
            //act
            _sut.RemoveHealth(200);
            //assert
            Assert.AreEqual(0, _sut.Health);
        }
        
        [Test]
        public void Test_AddHealth_WithoutExceedingHealthCap()
        {
            //arrange
            _sut.RemoveHealth(50);
            //act
            _sut.AddHealth(40);
            //assert
            Assert.AreEqual(90, _sut.Health);
        }
        
        [Test]
        public void Test_AddHealth_ReachesHealthCap()
        {
            //arrange
            _sut.RemoveHealth(30);
            //act
            _sut.AddHealth(40);
            //assert
            Assert.AreEqual(100, _sut.Health);
        }
        
        [Test]
        public void Test_RemoveStamina_WithoutRunningOutOfMana()
        {
            //arrange
            _sut.RemoveStamina(5);
            //act
            //assert
            Assert.AreEqual(5, _sut.Stamina);
        }
        
        [Test]
        public void Test_RemoveStamina_StopsAtNoMana()
        {
            //arrange
            //act
            _sut.RemoveStamina(20);
            //assert
            Assert.AreEqual(0, _sut.Stamina);
        }
        
        [Test]
        public void Test_AddStamina_WithoutExceedingStaminaCap()
        {
            //arrange
            _sut.RemoveStamina(5);
            //act
            _sut.AddStamina(4);
            //assert
            Assert.AreEqual(9, _sut.Stamina);
        }
        
        [Test]
        public void Test_AddStamina_ReachesStaminaCap()
        {
            //arrange
            _sut.RemoveStamina(3);
            //act
            _sut.AddStamina(4);
            //assert
            Assert.AreEqual(10, _sut.Stamina);
        }
        
        [Test]
        public void Test_GetItem_VerifyInventoryMoqWorks()
        {
            //arrange
            Item item = new Item("ItemName", "Description");
            _mockedInventory.Setup(mockedInventory => mockedInventory.GetItem("ItemName")).Returns(item);
            //act & assert
            Assert.AreEqual(item, _sut.GetItem("ItemName"));
            _mockedInventory.Verify(mockedInventory => mockedInventory.GetItem("ItemName"), Times.Once);
        }
        
        [Test]
        public void Test_AddInventoryItem_AddsItemSuccessfully()
        {
            //arrange
            Item item = new Item("ItemName", "Description");
            _mockedInventory.Setup(mockedInventory => mockedInventory.AddItem(item));
            //act
            _sut.AddInventoryItem(item);
            //assert
            _mockedInventory.Verify(mockedInventory => mockedInventory.AddItem(item), Times.Once);
        }
        
        [Test]
        public void Test_RemoveInventoryItem_RemovesItemSuccessfully()
        {
            //arrange
            Item item = new Item("ItemName", "Description");
            _mockedInventory.Setup(mockedInventory => mockedInventory.RemoveItem(item));
            //act
            _sut.RemoveInventoryItem(item);
            //assert
            _mockedInventory.Verify(mockedInventory => mockedInventory.RemoveItem(item), Times.Once);
        }
        
        [Test]
        public void Test_EmptyInventory_EmptiesInventorySuccessfully()
        {
            //arrange
            _mockedInventory.Setup(mockedInventory => mockedInventory.EmptyInventory());
            //act
            _sut.EmptyInventory();
            //assert
            _mockedInventory.Verify(mockedInventory => mockedInventory.EmptyInventory(), Times.Once);
        }
        
        [Test]
        public void Test_GetAmount_VerifyBitcoinMoqWorks()
        {
            //assert
            _mockedBitcoins.Setup(mockedBitcoins => mockedBitcoins.Amount).Returns(20);
            //act & assert
            Assert.AreEqual(20, _sut.Bitcoins.Amount);
            _mockedBitcoins.Verify(mockedBitcoins => mockedBitcoins.Amount, Times.Once);
        }
        
        [Test]
        public void Test_AddBitcoins_AddsBitcoinsSuccessfully()
        {
            //arrange
            _mockedBitcoins.Setup(mockedBitcoins => mockedBitcoins.AddAmount(20));
            //act
            _sut.AddBitcoins(20);
            //assert
            _mockedBitcoins.Verify(mockedBitcoins => mockedBitcoins.AddAmount(20), Times.Once);
        }
        
        [Test]
        public void Test_RemoveBitcoins_RemovesBitcoinsSuccessfully()
        {
            //arrange
            _mockedBitcoins.Setup(mockedBitcoins => mockedBitcoins.RemoveAmount(10));
            //act
            _sut.RemoveBitcoins(10);
            //assert
            _mockedBitcoins.Verify(mockedBitcoins => mockedBitcoins.RemoveAmount(10), Times.Once);
        }
        
        [Test]
        public void Test_DropItem_DropsItemSuccessfully()
        {
            //arrange
            Item item = new Item("ItemName", "Description");
            _mockedInventory.Setup(mockedInventory => mockedInventory.GetItem("ItemName")).Returns(item);
            _mockedInventory.Setup(mockedInventory => mockedInventory.RemoveItem(item));
            //act
            _sut.DropItem("ItemName");
            //assert
            _mockedInventory.Verify(mockedInventory => mockedInventory.RemoveItem(item), Times.Once);
        }
        
        [Test]
        public void Test_DropItem_ThrowsExceptionBecauseNoItemExists()
        {
            //arrange
            _mockedInventory.Setup(mockedInventory => mockedInventory.GetItem("ItemName"));
            //act & assert
            Assert.Throws<ItemException>(() => _sut.DropItem("ItemName"));
        }
        
        [Test]
        public void Test_GetAttackDamage_GetDefaultAttackDamage()
        {
            //arrange
            //act
            //assert
            Assert.AreEqual(5, _sut.GetAttackDamage());
        }
        
        [Test]
        public void Test_GetLevel_VerifyRadiationLevelMoqWorks()
        {
            //arrange
            _mockedRadiationLevel.Setup(mockedRadiationLevel => mockedRadiationLevel.Level).Returns(1);
            //act & assert
            Assert.AreEqual(1, _sut.RadiationLevel.Level);
            _mockedRadiationLevel.Verify(mockedRadiationLevel => mockedRadiationLevel.Level, Times.Once);
        }
    }
}*/