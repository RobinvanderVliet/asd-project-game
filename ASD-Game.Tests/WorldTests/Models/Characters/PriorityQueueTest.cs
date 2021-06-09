using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using ASD_Game.World.Models.Characters.Algorithms.Pathfinder;
using NUnit.Framework;

namespace ASD_Game.Tests.WorldTests.Models.Characters
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    class PriorityQueueTest
    {
        private PriorityQueue<Node> _sut;
        private Node _node1;
        private Node _node2;
        private Node _node3;

        [SetUp]
        public void Setup()
        {
            _node1 = new Node(new Vector2(0, 6), false);
            _node2 = new Node(new Vector2(7, 2), false);
            _node3 = new Node(new Vector2(16, 9), false);
            _node1.DistanceToTarget = 1;
            _node1.Cost = 3;
            _node2.DistanceToTarget = 1;
            _node2.Cost = 1;
            _node3.DistanceToTarget = 1;
            _node3.Cost = 2;

            _sut = new PriorityQueue<Node>();
        }

        [Test]
        public void Test_Peek_ReturnsNodeWithLowestFScore()
        {
            // Arrange ---------
            _sut.Enqueue(_node1);
            _sut.Enqueue(_node2);
            _sut.Enqueue(_node3);

            // Act -------------
            Vector2 actualNodePosition = _sut.Peek().Position;
            Vector2 expectedNodePosition = new Vector2(7, 2);

            // Assert ----------
            Assert.That(actualNodePosition, Is.EqualTo(expectedNodePosition));
        }

        [Test]
        public void Test_Contains_ReturnsTrueIfItemIsInPriorityQueue()
        {
            // Arrange ---------
            _sut.Enqueue(_node1);

            // Act -------------
            bool actual = _sut.Contains(_node1);
            bool expected = true;

            // Assert ----------
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Test_Dequeue_RemovesNodeWithLowestFScore()
        {
            // Arrange ---------
            _sut.Enqueue(_node1);
            _sut.Enqueue(_node2);
            _sut.Enqueue(_node3);

            // Act -------------
            _sut.Dequeue();
            bool actual = _sut.Contains(_node2);
            bool expected = false;

            // Assert ----------
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
