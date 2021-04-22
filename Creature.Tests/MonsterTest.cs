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

namespace Creature.Tests
{
    class MonsterTest
    {
        private ICreature _sut;
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
        public void Test_Apply_Damage_Kills_Monster()
        {
            // Act
            _sut.ApplyDamage(30);

            // Assert
            Assert.That(_sut.IsAlive == false);
        }

        [Test]
        public void Test_Fire_Event_Attacks_Player()
        {
            // Arrange
            Mock<ICreature> playerMock = new Mock<ICreature>();

            // Act
            _sut.FireEvent(Monster.Event.SPOTTED_PLAYER, playerMock.Object);
            _sut.FireEvent(Monster.Event.PLAYER_IN_RANGE, playerMock.Object);

            // Assert
            playerMock.Verify((playerMock) => playerMock.ApplyDamage(_damage));
        }

        /*
            
        */
        [Test]
        public void Test_Fire_Event_Use_Consumable()
        {
            // Arrange
            Mock<ICreature> playerMock = new Mock<ICreature>();
            Mock<IConsumable> consumableMock = new Mock<IConsumable>();

            // Act
            _sut.FireEvent(Monster.Event.SPOTTED_PLAYER, playerMock.Object);
            _sut.FireEvent(Monster.Event.ALMOST_DEAD, consumableMock.Object);

            // Assert
            consumableMock.Verify((consumableMock) => consumableMock.Use());
        }

        [Test]
        public void Heal_Amount_Does_Not_Revive_Monster()
        {
            // Act
            _sut.ApplyDamage(30);
            _sut.HealAmount(50);

            // Assert
            Assert.That(_sut.IsAlive == false);
        }

        [Test]
        public void Test_Get_Top_Position_From_Path_Stack()
        {
            // Arrange
            Mock<ICreature> playerMock = new Mock<ICreature>();
            Stack<Node> path = new Stack<Node>();
            path.Push(new Node(new Vector2(1, 1), true));
            path.Push(new Node(new Vector2(1, 2), true));
            path.Push(new Node(new Vector2(1, 3), true));

            // Act
            _sut.FireEvent(Monster.Event.SPOTTED_PLAYER, playerMock.Object);
            _sut.Do(path);

            // Assert
            Assert.That(new Vector2(1, 3), Is.EqualTo(_sut.Position));
        }
    }
}
