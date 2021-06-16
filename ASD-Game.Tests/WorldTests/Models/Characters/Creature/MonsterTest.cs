using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using ASD_Game.World.Models.Characters;

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
            _sut = new Monster("monster", 10, 10, "$", "my-id");
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