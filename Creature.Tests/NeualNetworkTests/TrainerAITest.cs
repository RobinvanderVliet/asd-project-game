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
                    new Vector2(14, 14),
                    20,
                    5,
                    200,
                    null,
                    false
                );
            _smartTestMonster = new SmartMonsterForTraining(_MonsterData);
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
            Vector2 location = new Vector2(0, 0);
            _sut = new TrainerAI(location, "player");

            _sut.update(_smartTestMonster);

            Vector2 newLocation = _sut.location;

            Assert.AreNotEqual(location, newLocation);
        }
    }
}