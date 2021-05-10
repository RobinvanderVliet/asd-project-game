using Creature.Creature.StateMachine.Data;
using Creature.World;
using Moq;
using NUnit.Framework;
using System.Numerics;

namespace Creature.Tests
{
    class PlayerTest
    {
        private ICreature _sut;

        [SetUp]
        public void Setup()
        {
            Vector2 position = new Vector2(10, 10);
            Mock<IWorld> worldMock = new Mock<IWorld>();
            int health = 20;
            int damage = 5;
            int visionRange = 10;

            PlayerData playerData = new PlayerData(position, health, damage, visionRange, worldMock.Object);
            _sut = new Player(playerData);
        }

        [Test]
        public void Test_ApplyDamage_KillsPlayer()
        {
            // Arrange ---------

            // Act -------------
            _sut.ApplyDamage(30);

            // Assert ----------
            Assert.False(_sut.CreatureStateMachine.CreatureData.IsAlive);
        }

        [Test]
        public void Test_HealAmount__Healts_Player()
        {
            // Arrange ---------

            // Act -------------
            _sut.ApplyDamage(10);
            _sut.HealAmount(10);

            // Assert ----------
            Assert.AreEqual(_sut.CreatureStateMachine.CreatureData.Health, 20);
        }
    }
}
