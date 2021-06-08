using Creature.Creature;
using Creature.Creature.NeuralNetworking.TrainingScenario;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using WorldGeneration.StateMachine.Data;

namespace Creature.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    internal class DataGatheringServiceTest
    {
        private DataGatheringServiceForTraining _sut;
        private SmartMonsterForTraining _smartMonster;
        private TrainerAI _player;

        [SetUp]
        public void Setup()
        {
            MonsterData _MonsterData =
                new MonsterData
                (
                14,
                14,
                0
                );
            _player = new TrainerAI(new Vector2(16, 16), "player");
            _smartMonster = new SmartMonsterForTraining("Zombie", 14, 14, "T", "monst");
            _sut = new DataGatheringServiceForTraining();
        }

        [Test]
        public void Test_ScanMap_ScanMapForNearbyCharacters()
        {
            //act
            _sut.ScanMap(_smartMonster, _smartMonster.CreatureData.VisionRange);

            //assert
            Assert.NotNull(_sut.ClosestPlayer);
            Assert.NotNull(_sut.ClosestMonster);
        }

        [Test]
        public void Test_ScanMapPlayerAI_Adjacent()
        {
            //arrange/act
            _player.Location = new Vector2(14, 15);

            //assert
            Assert.NotNull(_sut.ScanMapPlayerAI(_player.Location, _smartMonster));
        }

        [Test]
        public void Test_ScanMapPlayerAI_Not_Adjacent()
        {
            //arrange/act
            _player.Location = new Vector2(20, 20);

            //assert
            Assert.Null(_sut.ScanMapPlayerAI(_player.Location, _smartMonster));
        }

        [Test]
        public void Test_CheckNewPosition()
        {
            //act
            _sut.CheckNewPosition(_smartMonster);

            //assert
            int expected = -13;
            int actual = _smartMonster.Score;

            Assert.AreEqual(expected, actual);
        }
    }
}