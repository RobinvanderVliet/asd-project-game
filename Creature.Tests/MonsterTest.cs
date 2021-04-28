using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Creature;
using Moq;
using Creature.World;
using System.Numerics;
using Creature.Consumable;
using Creature.Pathfinder;
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

            _sut = new Monster(worldMock.Object, position, _damage, health, visionRange);
        }

        [Test]
        public void Test_ApplyDamage_KillsMonster()
        {
            // Arrange ---------

            // Act -------------
            _sut.ApplyDamage(30);

            // Assert ----------
            Assert.That(_sut.IsAlive == false);
        }

        [Test]
        public void Test_FireEventSpottedPlayer_FollowsPlayer()
        {
            // Arrange ---------
            Mock<ICreature> playerMock = new Mock<ICreature>();

            // Act -------------
            _sut.FireEvent(Monster.Event.SPOTTED_PLAYER, playerMock.Object);

            // Assert ----------
            Assert.AreEqual(true, _sut.IsFollowing);
            //Assert.AreEqual(playerMock.Object, _sut.Player);
        }

        [Test]
        public void Test_FireEventLostPlayer_DoesNotFollowPlayer()
        {
            // Arrange ---------
            Mock<ICreature> playerMock = new Mock<ICreature>();

            // Act -------------
            _sut.FireEvent(Monster.Event.LOST_PLAYER, playerMock.Object);

            // Assert ----------
            Assert.AreEqual(false, _sut.IsFollowing);
            //Assert.AreEqual(playerMock.Object, _sut.Player);
        }

        [Test]
        public void Test_FireEventPlayerInRange_AttacksPlayer()
        {
            // Arrange ---------
            Mock<ICreature> playerMock = new Mock<ICreature>();

            // Act -------------
            _sut.FireEvent(Monster.Event.SPOTTED_PLAYER, playerMock.Object);
            _sut.FireEvent(Monster.Event.PLAYER_IN_RANGE, playerMock.Object);

            // Assert ----------
            playerMock.Verify((playerMock) => playerMock.ApplyDamage(_damage));
        }

        [Test]
        public void Test_FireEventSpottedPlayer_DoesNotAttackPlayer()
        {
            // Arrange ---------
            Mock<ICreature> playerMock = new Mock<ICreature>();

            // Act -------------
            _sut.FireEvent(Monster.Event.SPOTTED_PLAYER, playerMock.Object);

            // Assert ----------
            playerMock.Verify((playerMock) => playerMock.ApplyDamage(_damage), Times.Never);
        }

        [Test]
        public void Test_FireEventAlmostDead_UsesConsumable()
        {
            // Arrange ---------
            Mock<ICreature> playerMock = new Mock<ICreature>();
            Mock<IConsumable> consumableMock = new Mock<IConsumable>();

            // Act -------------
            _sut.FireEvent(Monster.Event.SPOTTED_PLAYER, playerMock.Object);
            _sut.FireEvent(Monster.Event.ALMOST_DEAD, consumableMock.Object);

            // Assert ----------
            consumableMock.Verify((consumableMock) => consumableMock.Use());
        }

        [Test]
        public void Test_HealAmount_DoesNotReviveMonsterWhenDamageTooHigh()
        {
            // Arrange ---------

            // Act -------------
            _sut.ApplyDamage(30);
            _sut.HealAmount(50);

            // Assert ----------
            Assert.That(_sut.IsAlive == false);
        }

        [Test]
        public void Test_FireEventSpottedPlayer_GetsTopPositionFromPathStack()
        {
            // Arrange ---------
            Mock<ICreature> playerMock = new Mock<ICreature>();
            Stack<Node> path = new Stack<Node>();
            path.Push(new Node(new Vector2(1, 1), true));
            path.Push(new Node(new Vector2(1, 2), true));
            path.Push(new Node(new Vector2(1, 3), true));

            // Act -------------
            _sut.FireEvent(Monster.Event.SPOTTED_PLAYER, playerMock.Object);
            _sut.Do(path);

            // Assert ----------
            Assert.That(new Vector2(1, 3), Is.EqualTo(_sut.Position));
        }
    }
}
