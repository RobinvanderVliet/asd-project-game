using System.Diagnostics.CodeAnalysis;
using ASD_Game.World.Models.Characters.Algorithms.NeuralNetworking;
using NUnit.Framework;

namespace ASD_Game.Tests.WorldTests.Models.Characters.NeuralNetworkTests
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
        public void Test_Engage_EngageNode()
        {
            //arrange
            _sut = new NeuralNode(0);
            _sut.Layer = 0;

            NeuralNode connectedNode = new NeuralNode(1);
            connectedNode.Layer = 1;

            _ConnectionGene = new ConnectionGene(_sut, connectedNode, 1, 1001);
            _sut.OutputConnections.Add(_ConnectionGene);

            //act
            _sut.Engage();

            //assert
            float expected = 0.0f;
            float actual = _sut.OutputConnections[0].ToNode.InputSum;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test_Engage_Sigmoid()
        {
            //arrange
            _sut = new NeuralNode(0);

            _sut.Layer = 1;

            _sut.InputSum = 12;

            //act
            _sut.Engage();

            //assert
            float expected = 0.999993861f;
            float actual = _sut.OutputValue;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test_IsConnected_False()
        {
            //act
            _sut = new NeuralNode(0);

            //assert
            Assert.False(_sut.IsConnectedTo(_sut));
        }

        [Test]
        public void Test_IsConnected_True()
        {
            //arrange
            _sut = new NeuralNode(0);
            NeuralNode connectedNode = new NeuralNode(1);
            connectedNode.Layer = 1;

            _ConnectionGene = new ConnectionGene(_sut, connectedNode, 1, 1001);

            //act
            _sut.OutputConnections.Add(_ConnectionGene);

            //assert
            Assert.True(_sut.IsConnectedTo(connectedNode));
        }
    }
}