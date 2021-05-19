using System.Collections.Generic;
using NUnit.Framework;
using Moq;
using Creature.Pathfinder;
using System.Numerics;

namespace Creature.Tests
{
    [TestFixture]
    class PathFinderTest
    {
        private List<List<Node>> _nodes;
        private int _boardSize;
        private Vector2 _startPos;
        private Vector2 _endPos;
        
        private Mock<PathFinder> _pathfinder;

        [SetUp]
        public void Setup()
        {
            _boardSize = 20;
            _startPos = new Vector2(0, 0);
            _endPos = new Vector2(19, 19);

            _nodes = new List<List<Node>>();
            for (int row = 0; row < _boardSize; row++)
            {
                List<Node> nodePoints = new List<Node>();
                for (int col = 0; col < _boardSize; col++)
                {
                    Vector2 nodeLocation = new Vector2(row, col);
                    Node node = new Node(nodeLocation, true);
                    nodePoints.Add(node);
                }
                _nodes.Add(nodePoints);
            }
            _pathfinder = new Mock<PathFinder>(_nodes) { CallBase = true};
        }

        [Test]
        public void Test_FindPath_BetweenStartAndEndPoint()
        {
            // Arrange ---------
            Stack<Node> actualPath = new Stack<Node>();
            actualPath = _pathfinder.Object.FindPath(_startPos, _endPos);
            
            // Act -------------
            int actual = actualPath.Count;
            int expected = 38;

            // Assert ----------
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
