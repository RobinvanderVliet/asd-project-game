using Creature.Creature;
using Creature.Creature.NeuralNetworking;
using Creature.Creature.StateMachine.Data;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

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
                new System.Numerics.Vector2(0, 0),
                10,
                10,
                10,
                null,
                false
            );
        }

        [Test]
        public void Test_PopulationDone_True()
        {
            _sut = new Population(1, _MonsterData);

            _sut.pop[0].dead = true;

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

            int Expectedgen = _sut.gen + 1;

            _sut.NaturalSelection();

            Assert.AreEqual(Expectedgen, _sut.gen);
        }
    }
}