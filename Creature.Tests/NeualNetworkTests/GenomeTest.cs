using Creature.Creature.NeuralNetworking;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Creature.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    internal class GenomeTest
    {
        private Genome _sut;

        private int _inputs;
        private int _outputs;
        private int _layers;
        private int _nextNode;
        private int _nextConnectionNo;

        [SetUp]
        public void Setup()
        {
            _inputs = 2;
            _outputs = 2;
            _layers = 2;
            _nextNode = 0;
            _nextConnectionNo = 1000;
        }

        [Test]
        public void Test_GetNode_NodeFound()
        {
            _sut = new Genome(_inputs, _outputs);

            NeuralNode expected = new NeuralNode(0);
            NeuralNode actual = _sut.GetNode(0);

            Assert.AreEqual(expected.number, actual.number);
        }

        [Test]
        public void Test_GetNode_NodeNotFound()
        {
            _sut = new Genome(_inputs, _outputs);

            NeuralNode actual = _sut.GetNode(_inputs + _outputs + 1);

            Assert.Null(actual);
        }

        [Test]
        public void Test_FeedForward()
        {
            _sut = new Genome(_inputs, _outputs);

            float[] inputvalues = new float[] { 1, 2 };

            int expected = _outputs;
            float[] output = _sut.FeedForward(inputvalues);

            Assert.AreEqual(expected, output.Length);
        }

        [Test]
        public void Test_GenerateNetwork()
        {
            _sut = new Genome(_inputs, _outputs);

            _sut.GenerateNetwork();

            int expected = _inputs + _outputs + 1;
            int actual = _sut.network.Count;

            Assert.AreEqual(expected, actual);
        }
    }
}