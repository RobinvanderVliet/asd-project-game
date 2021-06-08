using Creature.Creature.StateMachine;
using Moq;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using Creature.Creature;

namespace Creature.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    class MonsterTest
    {
        private Monster _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new Monster("monster", 10, 10, "$");
        }

        [Test]
        public void Test_CreateMonster_CreatesMonsterData()
        {
            // Assert ----------
            Assert.That(_sut.MonsterData.Position.X == 10);
            Assert.That(_sut.MonsterData.Position.Y == 10);
        }
    }
}