using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using ASD_Game.World.Models.Characters.Algorithms.NeuralNetworking.TrainingScenario;
using ASD_Game.World.Models.Characters.StateMachine.Data;
using NUnit.Framework;

namespace ASD_Game.Tests.WorldTests.Models.Characters.NeuralNetworkTests
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
            _sut.ScanMap(_smartMonster, 200);

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
        public void Test_ScanMapPlayerAI_NotAdjacent()
        {
            //arrange/act
            _player.Location = new Vector2(20, 20);

            //assert
            Assert.Null(_sut.ScanMapPlayerAI(_player.Location, _smartMonster));
        }

        [Test]
        public void Test_CheckNewPosition_IsNewPosition()
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