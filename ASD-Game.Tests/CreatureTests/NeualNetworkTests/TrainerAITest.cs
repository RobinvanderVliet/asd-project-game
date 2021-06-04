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
            Vector2 location = new Vector2(15, 14);
            _sut = new TrainerAI(location, "player");

            _sut.update(_smartTestMonster);

            int expected = _sut.damage;
            int actual = _smartTestMonster.DamageTaken;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test_Update_Walk()
        {
            Vector2 location = new Vector2(1, 1);
            _sut = new TrainerAI(location, "player");

            _sut.update(_smartTestMonster);

            Vector2 newLocation = _sut.location;

            Assert.AreNotEqual(location, newLocation);
        }
    }
}