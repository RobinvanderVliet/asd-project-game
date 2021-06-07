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
            _sut.Layer = 0;

            NeuralNode connectedNode = new NeuralNode(1);
            connectedNode.Layer = 1;

            _ConnectionGene = new ConnectionGene(_sut, connectedNode, 1, 1001);

            _sut.OutputConnections.Add(_ConnectionGene);

            _sut.Engage();

            float expected = 0.0f;
            float actual = _sut.OutputConnections[0].ToNode.InputSum;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test_Engage_Sigmoid()
        {
            _sut = new NeuralNode(0);

            _sut.Layer = 1;

            _sut.InputSum = 12;

            _sut.Engage();

            float expected = 0.999993861f;
            float actual = _sut.OutputValue;

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
            connectedNode.Layer = 1;

            _ConnectionGene = new ConnectionGene(_sut, connectedNode, 1, 1001);

            _sut.OutputConnections.Add(_ConnectionGene);

            Assert.True(_sut.IsConnectedTo(connectedNode));
        }
    }
}