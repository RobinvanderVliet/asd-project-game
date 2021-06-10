using System.Diagnostics.CodeAnalysis;
using ASD_Game.World.Models.Characters.Algorithms.NeuralNetworking;
using ASD_Game.World.Models.Characters.StateMachine.Data;
using NUnit.Framework;

namespace ASD_Game.Tests.WorldTests.Models.Characters.NeuralNetworkTests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    internal class PopulationTest
    {
        private Population _sut;
        private MonsterData _MonsterData;

        [SetUp]
        public void Setup()
        {
            _MonsterData = new MonsterData
            (
                14,
                14,
                0
            );
        }

        [Test]
        public void Test_PopulationDone_True()
        {
            //arrange
            _sut = new Population(1, _MonsterData);

            //act
            _sut.Pop[0].Dead = true;

            //assert
            Assert.True(_sut.Done());
        }

        [Test]
        public void Test_PopulationDone_False()
        {
            //act
            _sut = new Population(1, _MonsterData);

            //assert
            Assert.False(_sut.Done());
        }

        [Test]
        public void Test_NaturalSelection_ApplyNaturalSelectionToAPopulation()
        {
            //arrange
            _sut = new Population(10, _MonsterData);

            int Expectedgen = _sut.Gen + 1;

            //act
            _sut.NaturalSelection();

            //assert
            Assert.AreEqual(Expectedgen, _sut.Gen);
        }
    }
}