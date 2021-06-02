using Creature.Creature.NeuralNetworking;
using Creature.Creature.NeuralNetworking.TrainingScenario;
using Creature.Creature.StateMachine.Data;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Creature.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    internal class TrainingScenarioTest
    {
        private TrainingScenario _sut = new TrainingScenario();
        private MonsterData _MonsterData;

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
        }

        [Test]
        public void Test_SetupTraining()
        {
            _sut.SetupTraining();
            Assert.AreEqual(50, _sut.pop.pop.Count);
        }
    }
}