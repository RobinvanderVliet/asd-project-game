using System.Diagnostics.CodeAnalysis;
using Moq;
using NUnit.Framework;
using Player.Model;

namespace Player.Tests
{
    [ExcludeFromCodeCoverage]
    public class PlayerModelTest
    {
        private PlayerModel sut;
        private Mock<IInventory> mockedInventory;
        private Mock<IBitcoin> mockedBitcoins;
        private Mock<IRadiationLevel> mockedRadiationLevel;
        
        [SetUp]
        public void Setup()
        {
            mockedInventory = new Mock<IInventory>();
            mockedBitcoins = new Mock<IBitcoin>();
            mockedRadiationLevel = new Mock<IRadiationLevel>();
            sut = new PlayerModel("Jan", mockedInventory.Object, mockedBitcoins.Object, mockedRadiationLevel.Object);
        }
        
        [Test]
        public void Test_GetName_GetsNameSuccessfully()
        {
            Assert.AreEqual("Jan", sut.Name);
        }
        
        [Test]
        public void Test_SetName_SetsNameSuccessfully()
        {
            var name = "Nieuwe naam";
            sut.Name = name;
            
            Assert.AreEqual(name, sut.Name);
        }
        
        [Test]
        public void Test_GetHealth_GetsHealthSuccessfully()
        {
            Assert.AreEqual(100, sut.Health);
        }
        
        [Test]
        public void Test_SetHealth_SetsHealthSuccessfully()
        {
            var HP = 50;
            sut.Health = HP;
            
            Assert.AreEqual(HP, sut.Health);
        }
        
        [Test]
        public void Test_GetStamina_GetsStaminaSuccessfully()
        {
            Assert.AreEqual(10, sut.Stamina);
        }
        
        [Test]
        public void Test_SetStamina_SetsStaminaSuccessfully()
        {
            var stamina = 5;
            sut.Stamina = stamina;
            
            Assert.AreEqual(stamina, sut.Stamina);
        }
        
        [Test]
        public void Test_GetInventory_GetsInventorySuccessfully()
        {
            Assert.AreEqual(mockedInventory.Object, sut.Inventory);
        }
        
        [Test]
        public void Test_SetInventory_SetsInventorySuccessfully()
        {
            var inventory = new Inventory();
            sut.Inventory = inventory;
            
            Assert.AreEqual(inventory, sut.Inventory);
        }
        
        [Test]
        public void Test_GetBitcoins_GetsBitcoinsSuccessfully()
        {
            Assert.AreEqual(mockedBitcoins.Object, sut.Bitcoins);
        }
        
        [Test]
        public void Test_SetBitcoins_SetsBitcoinsSuccessfully()
        {
            var bitcoins = new Bitcoin(5);
            sut.Bitcoins = bitcoins;
            
            Assert.AreEqual(bitcoins, sut.Bitcoins);
        }
        
        [Test]
        public void Test_GetRadiationLevel_GetsRadiationLevelSuccessfully()
        {
            Assert.AreEqual(mockedRadiationLevel.Object, sut.RadiationLevel);
        }
        
        [Test]
        public void Test_SetRadiationLevel_SetsRadiationLevelSuccessfully()
        {
            var radiationLevel = new RadiationLevel(5);
            sut.RadiationLevel = radiationLevel;
            
            Assert.AreEqual(radiationLevel, sut.RadiationLevel);
        }

        [Test]
        public void Test_RemoveHealth_WithoutDying()
        {
            sut.RemoveHealth(50);
            
            Assert.AreEqual(50, sut.Health);
        }
        
        [Test]
        public void Test_RemoveHealth_StopsAtDyingState()
        {
            sut.RemoveHealth(200);
            
            Assert.AreEqual(0, sut.Health);
        }
        
        [Test]
        public void Test_AddHealth_WithoutExceedingHealthCap()
        {
            sut.RemoveHealth(50);
            
            sut.AddHealth(40);
            
            Assert.AreEqual(90, sut.Health);
        }
        
        [Test]
        public void Test_AddHealth_ReachesHealthCap()
        {
            sut.RemoveHealth(30);
            
            sut.AddHealth(40);
            
            Assert.AreEqual(100, sut.Health);
        }
        
        [Test]
        public void Test_RemoveStamina_WithoutRunningOutOfMana()
        {
            sut.RemoveStamina(5);
            
            Assert.AreEqual(5, sut.Stamina);
        }
        
        [Test]
        public void Test_RemoveStamina_StopsAtNoMana()
        {
            sut.RemoveStamina(20);
            
            Assert.AreEqual(0, sut.Stamina);
        }
        
        [Test]
        public void Test_AddStamina_WithoutExceedingStaminaCap()
        {
            sut.RemoveStamina(5);
            
            sut.AddStamina(4);
            
            Assert.AreEqual(9, sut.Stamina);
        }
        
        [Test]
        public void Test_AddStamina_ReachesStaminaCap()
        {
            sut.RemoveStamina(3);
            
            sut.AddStamina(4);
            
            Assert.AreEqual(10, sut.Stamina);
        }
        
        [Test]
        public void Test_GetItem_VerifyInventoryMoqWorks()
        {
            Item item = new Item("Naam", "Beschrijving");
            mockedInventory.Setup(mockedInventory => mockedInventory.GetItem("Naam")).Returns(item);
            
            Assert.AreEqual(item, sut.GetItem("Naam"));
            mockedInventory.Verify(mockedInventory => mockedInventory.GetItem("Naam"), Times.Once);
        }
        
        [Test]
        public void Test_AddInventoryItem_AddsItemSuccessfully()
        {
            Item item = new Item("Naam", "Beschrijving");
            mockedInventory.Setup(mockedInventory => mockedInventory.AddItem(item));

            sut.AddInventoryItem(item);
            
            mockedInventory.Verify(mockedInventory => mockedInventory.AddItem(item), Times.Once);
        }
        
        [Test]
        public void Test_RemoveInventoryItem_RemovesItemSuccessfully()
        {
            Item item = new Item("Naam", "Beschrijving");
            mockedInventory.Setup(mockedInventory => mockedInventory.RemoveItem(item));

            sut.RemoveInventoryItem(item);
            
            mockedInventory.Verify(mockedInventory => mockedInventory.RemoveItem(item), Times.Once);
        }
        
        [Test]
        public void Test_EmptyInventory_EmptiesInventorySuccessfully()
        {
            mockedInventory.Setup(mockedInventory => mockedInventory.EmptyInventory());

            sut.EmptyInventory();
            
            mockedInventory.Verify(mockedInventory => mockedInventory.EmptyInventory(), Times.Once);
        }
        
        [Test]
        public void Test_GetAmount_VerifyBitcoinMoqWorks()
        {
            mockedBitcoins.Setup(mockedBitcoins => mockedBitcoins.Amount).Returns(20);

            Assert.AreEqual(20, sut.Bitcoins.Amount);
            mockedBitcoins.Verify(mockedBitcoins => mockedBitcoins.Amount, Times.Once);
        }
        
        [Test]
        public void Test_AddBitcoins_AddsBitcoinsSuccessfully()
        {
            mockedBitcoins.Setup(mockedBitcoins => mockedBitcoins.AddAmount(20));

            sut.AddBitcoins(20);
            
            mockedBitcoins.Verify(mockedBitcoins => mockedBitcoins.AddAmount(20), Times.Once);
        }
        
        [Test]
        public void Test_RemoveBitcoins_RemovesBitcoinsSuccessfully()
        {
            mockedBitcoins.Setup(mockedBitcoins => mockedBitcoins.RemoveAmount(10));

            sut.RemoveBitcoins(10);
            
            mockedBitcoins.Verify(mockedBitcoins => mockedBitcoins.RemoveAmount(10), Times.Once);
        }
        
        [Test]
        public void Test_DropItem_DropsItemSuccessfully()
        {
            Item item = new Item("Naam", "Beschrijving");
            mockedInventory.Setup(mockedInventory => mockedInventory.GetItem("Naam")).Returns(item);
            mockedInventory.Setup(mockedInventory => mockedInventory.RemoveItem(item));

            sut.DropItem("Naam");
            
            mockedInventory.Verify(mockedInventory => mockedInventory.RemoveItem(item), Times.Once);
        }
        
        [Test]
        public void Test_GetAttackDamage_GetDefaultAttackDamage()
        {
            Assert.AreEqual(5, sut.GetAttackDamage());
        }
        
        [Test]
        public void Test_GetLevel_VerifyRadiationLevelMoqWorks()
        {
            mockedRadiationLevel.Setup(mockedRadiationLevel => mockedRadiationLevel.Level).Returns(1);

            Assert.AreEqual(1, sut.RadiationLevel.Level);
            mockedRadiationLevel.Verify(mockedRadiationLevel => mockedRadiationLevel.Level, Times.Once);
        }
    }
}