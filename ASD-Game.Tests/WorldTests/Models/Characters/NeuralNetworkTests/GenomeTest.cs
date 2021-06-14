using System.Diagnostics.CodeAnalysis;
using ASD_Game.World.Models.Characters.Algorithms.NeuralNetworking;
using NUnit.Framework;

namespace ASD_Game.Tests.WorldTests.Models.Characters.NeuralNetworkTests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    internal class GenomeTest
    {
        private Genome _sut;

        private int _inputs;
        private int _outputs;

        [SetUp]
        public void Setup()
        {
            _inputs = 2;
            _outputs = 2;
        }

        [Test]
        public void Test_GetNode_NodeFound()
        {
            //arrange
            _sut = new Genome(_inputs, _outputs);
            NeuralNode expected = new NeuralNode(0);

            //act
            NeuralNode actual = _sut.GetNode(0);

            //assert
            Assert.AreEqual(expected.Number, actual.Number);
        }

        [Test]
        public void Test_GetNode_NodeNotFound()
        {
            //arrange
            _sut = new Genome(_inputs, _outputs);

            //act
            NeuralNode actual = _sut.GetNode(_inputs + _outputs + 1);

            //assert
            Assert.Null(actual);
        }

        [Test]
        public void Test_FeedForward_FeedForwardInput()
        {
            //arrange
            _sut = new Genome(_inputs, _outputs);
            float[] inputvalues = new float[] { 1, 2 };
            int expected = _outputs;

            //act
            float[] output = _sut.FeedForward(inputvalues);

            //assert
            Assert.AreEqual(expected, output.Length);
        }

        [Test]
        public void Test_GenerateNetwork_GenerateANetwork()
        {
            //arrange
            _sut = new Genome(_inputs, _outputs);

            //act
            _sut.GenerateNetwork();

            //assert
            int expected = _inputs + _outputs + 1;
            int actual = _sut.Network.Count;

            Assert.AreEqual(expected, actual);
        }
    }
}