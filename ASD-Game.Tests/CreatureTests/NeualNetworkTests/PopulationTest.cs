using Creature.Creature.NeuralNetworking;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using WorldGeneration.StateMachine.Data;

namespace Creature.Tests
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
            _sut = new Population(1, _MonsterData);

            _sut.Pop[0].Dead = true;

            Assert.True(_sut.Done());
        }

        [Test]
        public void Test_PopulationDone_False()
        {
            _sut = new Population(1, _MonsterData);

            Assert.False(_sut.Done());
        }

        [Test]
        public void Test_NaturalSelection()
        {
            _sut = new Population(10, _MonsterData);

            int Expectedgen = _sut.Gen + 1;

            _sut.NaturalSelection();

            Assert.AreEqual(Expectedgen, _sut.Gen);
        }
    }
}