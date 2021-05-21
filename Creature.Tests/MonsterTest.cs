using NUnit.Framework;
using Moq;
using Creature.World;
using System.Numerics;
using Creature.Creature.StateMachine.Data;
using System.Diagnostics.CodeAnalysis;

namespace Creature.Tests
{
    [ExcludeFromCodeCoverage]
    class MonsterTest
    {
        private Monster _sut;
        private int _damage;

        [SetUp]
        public void Setup()
        {
            Mock<IWorld> worldMock = new Mock<IWorld>();
            Vector2 position = new Vector2(10, 10);
            _damage = 5;
            int health = 20;
            int visionRange = 10;

            MonsterData monsterData = new MonsterData(position, health, _damage, visionRange, worldMock.Object, null, false);
            _sut = new Monster(monsterData);
        }

        [Test]
        public void Test_ApplyDamage_KillsMonster()
        {
            // Arrange ---------

            // Act -------------
            _sut.ApplyDamage(30);

            // Assert ----------
            Assert.False(_sut.CreatureStateMachine.CreatureData.IsAlive);
        }
    }
}
