using System.Collections.Generic;
using NUnit.Framework;
using Moq;
using Creature.Pathfinder;
using System.Numerics;
using System.Diagnostics.CodeAnalysis;

namespace Creature.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    class PriorityQueueTest
    {
        private Node _node1;
        private Node _node2;
        private Node _node3;

        private Mock<PriorityQueue<Node>> _priorityQueue;

        [SetUp]
        public void Setup()
        {
            _node1 = new Node(new Vector2(0, 0), false);
            _node2 = new Node(new Vector2(7, 0), false);
            _node3 = new Node(new Vector2(16, 0), false);
            _node1.distanceToTarget = 1;
            _node1.cost = 3;
            _node2.distanceToTarget = 1;
            _node2.cost = 1;
            _node3.distanceToTarget = 1;
            _node3.cost = 2;

            _priorityQueue = new Mock<PriorityQueue<Node>> { CallBase = true };
        }

        [Test]
        public void Test_Dequeue_ReturnsNodeWithLowestFScore()
        {
            // Arrange ---------
            Node actualNode;
            _priorityQueue.Object.Enqueue(_node1);
            _priorityQueue.Object.Enqueue(_node2);
            _priorityQueue.Object.Enqueue(_node3);
            actualNode = _priorityQueue.Object.Dequeue();

            // Act -------------
            float actual = actualNode.position.X;
            int expected = 7;

            // Assert ----------
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Test_Contains_ReturnsFalseIfItemNotInPriorityQueue()
        {
            // Arrange ---------
            _priorityQueue.Object.Enqueue(_node1);
            _priorityQueue.Object.Enqueue(_node2);

            // Act -------------
            bool actual = _priorityQueue.Object.Contains(_node3);
            bool expected = false;

            // Assert ----------
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
