using NeuralNetworking.TrainingScenario;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using WorldGeneration.StateMachine.Data;

namespace Character.Tests.NeuralNetworkTest
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
               14,
                14,
                0
                );
        }

        [Test]
        public void Test_SetupTraining_SetupATraining()
        {
            //act
            _sut.SetupTraining();
            //assert
            Assert.AreEqual(50, _sut.Pop.Pop.Count);
        }
    }
}