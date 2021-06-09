using System.Diagnostics.CodeAnalysis;
using ASD_Game.Items;
using ASD_Game.Items.Consumables;
using ASD_Game.Items.Consumables.ConsumableStats;
using ASD_Game.World.Models.Characters;
using NUnit.Framework;

namespace ASD_Game.Tests.WorldTests.Models.Characters
{
    [ExcludeFromCodeCoverage]
    public class PlayerTest
    {
        private Player _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new Player("Robin", 0, 42, "#", "12345");
        }

        [Test]
        public void Test_AddHealth_WithoutExceedingHealthMin()
        {
            _sut.AddHealth(-20);
            Assert.AreEqual(80, _sut.Health);
        }

        [Test]
        public void Test_AddHealth_WithoutExceedingHealthMax()
        {
            _sut.AddHealth(-30);
            _sut.AddHealth(5);
            Assert.AreEqual(75, _sut.Health);
        }

        [Test]
        public void Test_AddHealth_ExceedingHealthMin()
        {
            _sut.AddHealth(-500);
            Assert.AreEqual(0, _sut.Health);
        }

        [Test]
        public void Test_AddHealth_ExceedingHealthMax()
        {
            _sut.AddHealth(500);
            Assert.AreEqual(100, _sut.Health);
        }

        [Test]
        public void Test_AddStamina_WithoutExceedingStaminaMin()
        {
            _sut.AddStamina(-20);
            Assert.AreEqual(80, _sut.Stamina);
        }

        [Test]
        public void Test_AddStamina_WithoutExceedingStaminaMax()
        {
            _sut.AddStamina(-30);
            _sut.AddStamina(5);
            Assert.AreEqual(75, _sut.Stamina);
        }

        [Test]
        public void Test_AddStamina_ExceedingStaminaMin()
        {
            _sut.AddStamina(-500);
            Assert.AreEqual(0, _sut.Stamina);
        }

        [Test]
        public void Test_AddStamina_ExceedingStaminaMax()
        {
            _sut.AddStamina(500);
            Assert.AreEqual(100, _sut.Stamina);
        }

        [Test]
        public void Test_AddRadiationLevel_WithoutExceedingRadiationLevelMin()
        {
            _sut.AddRadiationLevel(100);
            _sut.AddRadiationLevel(-20);
            Assert.AreEqual(80, _sut.RadiationLevel);
        }

        [Test]
        public void Test_AddRadiationLevel_WithoutExceedingRadiationLevelMax()
        {
            _sut.AddRadiationLevel(100);
            _sut.AddRadiationLevel(-30);
            _sut.AddRadiationLevel(5);
            Assert.AreEqual(75, _sut.RadiationLevel);
        }

        [Test]
        public void Test_AddRadiationLevel_ExceedingRadiationLevelMin()
        {
            _sut.AddRadiationLevel(-500);
            Assert.AreEqual(0, _sut.RadiationLevel);
        }

        [Test]
        public void Test_AddRadiationLevel_ExceedingRadiationLevelMax()
        {
            _sut.AddRadiationLevel(500);
            Assert.AreEqual(100, _sut.RadiationLevel);
        }
        
        [Test]
        public void Test_UseConsumeable_EatHealthConsumeable()
        {
            _sut.Health = 50;
            var healthConsumable = new HealthConsumable
            {
                Health = Health.Low
            };
            _sut.UseConsumable(healthConsumable);
            
            Assert.AreEqual(75, _sut.Health);
        }
        
        [Test]
        public void Test_UseConsumeable_EatStaminaConsumeable()
        {
            _sut.Stamina = 50;
            var staminaConsumable = new StaminaConsumable
            {
                Stamina = Stamina.Low
            };
            _sut.UseConsumable(staminaConsumable);
            
            Assert.AreEqual(75, _sut.Stamina);
        }
        
        [Test]
        public void Test_GetArmorPoints_HasHelmetWith20ArmorPoints()
        {
            var item = ItemFactory.GetMilitaryHelmet();
            var inventory = new Inventory {Helmet = null};
            inventory.AddItem(item);
            _sut.Inventory = inventory;
            Assert.AreEqual(20, _sut.GetArmorPoints());
        }
        
        [Test]
        public void Test_GetArmorPoints_HasHelmetWith20ArmorPointsAndBodyArmorWith40ArmorPoints()
        {
            var helmetItem = ItemFactory.GetMilitaryHelmet();
            var bodyItem = ItemFactory.GetTacticalVest();
            var inventory = new Inventory {Helmet = null, Armor = null};
            inventory.AddItem(helmetItem);
            inventory.AddItem(bodyItem);
            _sut.Inventory = inventory;
            Assert.AreEqual(60, _sut.GetArmorPoints());
        }
    }
}