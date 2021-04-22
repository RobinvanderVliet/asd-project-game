using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Moq;
using NUnit.Framework;
using Player.Model;

namespace Player.Tests
{
    [ExcludeFromCodeCoverage]
    public class Tests
    {
        private Model.Player sut;
        private Mock<IInventory> mockedInventory;
        private Mock<IBitcoin> mockedBitcoins;
        private Mock<IRadiationLevel> mockedRadiationLevel;
        
        [SetUp]
        public void Setup()
        {
            mockedInventory = new Mock<IInventory>();
            mockedBitcoins = new Mock<IBitcoin>();
            mockedRadiationLevel = new Mock<IRadiationLevel>();
            sut = new Model.Player("Jan", mockedInventory.Object, mockedBitcoins.Object, mockedRadiationLevel.Object);
        }

        [Test]
        public void RemoveHealthWithAmountLessThan100()
        {
            sut.RemoveHealth(50);
            
            Assert.AreEqual(50, sut.Health);
        }
        
        [Test]
        public void RemoveHealthWithAmountMoreThan100StopsAt0()
        {
            sut.RemoveHealth(200);
            
            Assert.AreEqual(0, sut.Health);
        }
        
        [Test]
        public void AddHealthWithoutExceedingHealthCap()
        {
            sut.RemoveHealth(50);
            
            sut.AddHealth(40);
            
            Assert.AreEqual(90, sut.Health);
        }
        
        [Test]
        public void AddHealthReachesHealthCap()
        {
            sut.RemoveHealth(30);
            
            sut.AddHealth(40);
            
            Assert.AreEqual(100, sut.Health);
        }
        
        [Test]
        public void RemoveStaminaWithAmountLessThan10()
        {
            sut.RemoveStamina(5);
            
            Assert.AreEqual(5, sut.Stamina);
        }
        
        [Test]
        public void RemoveStaminaWithAmountMoreThan100StopsAt0()
        {
            sut.RemoveStamina(20);
            
            Assert.AreEqual(0, sut.Stamina);
        }
        
        [Test]
        public void AddStaminaWithoutExceedingStaminaCap()
        {
            sut.RemoveStamina(5);
            
            sut.AddStamina(4);
            
            Assert.AreEqual(9, sut.Stamina);
        }
        
        [Test]
        public void AddStaminaReachesStaminaCap()
        {
            sut.RemoveStamina(3);
            
            sut.AddStamina(4);
            
            Assert.AreEqual(10, sut.Stamina);
        }
        
        [Test]
        public void VerifyInventoryMoqWorks()
        {
            Item item = new Item("Naam", "Beschrijving");
            mockedInventory.Setup(mockedInventory => mockedInventory.GetItem("Naam")).Returns(item);

            // sut.GetItem("Naam");
            
            Assert.AreEqual(item, sut.GetItem("Naam"));
            mockedInventory.Verify(mockedInventory => mockedInventory.GetItem("Naam"), Times.Once);
        }
        
        [Test]
        public void VerifyBitcoinMoqWorks()
        {
            mockedBitcoins.Setup(mockedBitcoins => mockedBitcoins.Amount).Returns(20);

            Assert.AreEqual(20, sut.Bitcoins.Amount);
            mockedBitcoins.Verify(mockedBitcoins => mockedBitcoins.Amount, Times.Once);
        }
        
        [Test]
        public void VerifyRadiationLevelMoqWorks()
        {
            mockedRadiationLevel.Setup(mockedRadiationLevel => mockedRadiationLevel.Level).Returns(1);

            Assert.AreEqual(1, sut.RadiationLevel.Level);
            mockedRadiationLevel.Verify(mockedRadiationLevel => mockedRadiationLevel.Level, Times.Once);
        }
        
        [Test]
        public void GetDefaultAttackDamage()
        {
            Assert.AreEqual(5, sut.GetAttackDamage());
        }
    }
}