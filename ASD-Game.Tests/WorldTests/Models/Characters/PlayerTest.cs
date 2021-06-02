using NUnit.Framework;
using WorldGeneration;

namespace World.Tests.Models.Characters
{
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
    }
}