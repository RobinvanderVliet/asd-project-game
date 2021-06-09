using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using ASD_Game.World.Models.Characters.Algorithms.Pathfinder;
using NUnit.Framework;

namespace ASD_Game.Tests.WorldTests.Models.Characters
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    class PathFinderTest
    {
        private PathFinder _sut;

        private List<List<Node>> _nodes;
        private int _boardSize;
        private Vector2 _startPos;
        private Vector2 _endPos;

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
            _sut = new PathFinder(_nodes);
        }

        [Test]
        public void Test_FindPath_BetweenStartAndEndPoint()
        {
            // Arrange ---------
            Stack<Node> actualPath = new Stack<Node>();
            actualPath = _sut.FindPath(_startPos, _endPos);

            // Act -------------
            int actual = actualPath.Count;
            int expected = 38;

            // Assert ----------
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
