using Creature.Creature.NeuralNetworking;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace Creature.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    internal class NeuralNodeTest
    {
        private NeuralNode _sut;
        private ConnectionGene _ConnectionGene;

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test_Engage()
        {
            _sut = new NeuralNode(0);
            _sut.layer = 0;

            NeuralNode connectedNode = new NeuralNode(1);
            connectedNode.layer = 1;

            _ConnectionGene = new ConnectionGene(_sut, connectedNode, 1, 1001);

            _sut.outputConnections.Add(_ConnectionGene);

            _sut.Engage();

            float expected = 0.0f;
            float actual = _sut.outputConnections[0].toNode.inputSum;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test_Engage_Sigmoid()
        {
            _sut = new NeuralNode(0);

            _sut.layer = 1;

            _sut.inputSum = 12;

            _sut.Engage();

            float expected = 0.999993861f;
            float actual = _sut.outputValue;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test_IsConnected_False()
        {
            _sut = new NeuralNode(0);

            Assert.False(_sut.IsConnectedTo(_sut));
        }

        [Test]
        public void Test_IsConnected_True()
        {
            _sut = new NeuralNode(0);
            NeuralNode connectedNode = new NeuralNode(1);
            connectedNode.layer = 1;

            _ConnectionGene = new ConnectionGene(_sut, connectedNode, 1, 1001);

            _sut.outputConnections.Add(_ConnectionGene);

            Assert.True(_sut.IsConnectedTo(connectedNode));
        }
    }
}