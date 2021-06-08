using NeuralNetworking.TrainingScenario;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using WorldGeneration.StateMachine.Data;

namespace Character.Tests.NeuralNetworkTest
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    internal class TrainerAITest
    {
        private TrainerAI _sut;
        private SmartMonsterForTraining _smartTestMonster;

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
            _smartTestMonster = new SmartMonsterForTraining("Zombie", 14, 14, "T", "monst");
        }

        [Test]
        public void Test_Update_Attack()
        {
            //arrange
            Vector2 location = new Vector2(15, 14);
            _sut = new TrainerAI(location, "player");

            //act
            _sut.Update(_smartTestMonster);

            //assert
            int expected = _sut.Damage;
            int actual = _smartTestMonster.DamageTaken;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test_Update_Walk()
        {
            //arrange
            Vector2 location = new Vector2(1, 1);
            _sut = new TrainerAI(location, "player");

            //act
            _sut.Update(_smartTestMonster);

            //assert
            Vector2 newLocation = _sut.Location;

            Assert.AreNotEqual(location, newLocation);
        }
    }
}