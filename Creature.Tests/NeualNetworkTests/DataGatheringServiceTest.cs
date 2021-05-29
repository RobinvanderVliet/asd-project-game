using Creature.Creature;
using Creature.Creature.NeuralNetworking.TrainingScenario;
using Creature.Creature.StateMachine.Data;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Creature.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    internal class DataGatheringServiceTest
    {
        private DataGatheringService _sut;
        private SmartMonster _smartMonster;
        private TrainerAI _player;

        [SetUp]
        public void Setup()
        {
            MonsterData _MonsterData =
                new MonsterData
                (
                    new Vector2(14, 14),
                    20,
                    5,
                    200,
                    null,
                    false
                );
            _player = new TrainerAI(new Vector2(16, 16), "player");
            _smartMonster = new SmartMonster(_MonsterData);
            _sut = new DataGatheringService();
        }

        [Test]
        public void Test_ScanMap()
        {
            _sut.ScanMap(_smartMonster, _smartMonster.creatureData.VisionRange);

            Assert.NotNull(_sut.closestPlayer);
            Assert.NotNull(_sut.closestMonster);
        }

        [Test]
        public void Test_ScanMapPlayerAI_Adjacent()
        {
            _player.location = new Vector2(14, 15);

            Assert.NotNull(_sut.ScanMapPlayerAI(_player.location, _smartMonster));
        }

        [Test]
        public void Test_ScanMapPlayerAI_Not_Adjacent()
        {
            _player.location = new Vector2(20, 20);

            Assert.Null(_sut.ScanMapPlayerAI(_player.location, _smartMonster));
        }

        [Test]
        public void Test_CheckNewPosition()
        {
            _sut.CheckNewPosition(_smartMonster);

            int expected = -13;
            int actual = _smartMonster.score;

            Assert.AreEqual(expected, actual);
        }
    }
}