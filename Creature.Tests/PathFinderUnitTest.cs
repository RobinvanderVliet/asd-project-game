using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using Creature.Pathfinder;
using System.Numerics;

namespace Creature.Tests
{
    [TestFixture]
    class PathFinderUnitTest
    {
        // Declaratie en initialisatie constante variabelen
        // Declaratie variabelen
        List<List<Node>> nodes;
        int board_size;
        Vector2 startPos;
        Vector2 endPos;
        
        // Declaratie mocks
        Mock<PathFinder> pathfinder;

        [SetUp]
        public void Setup()
        {
            // Initialiseren variabelen
            board_size = 20;
            startPos = new Vector2(0, 0);
            endPos = new Vector2(19, 19);

            nodes = new List<List<Node>>();
            for (int row = 0; row < board_size; row++)
            {
                List<Node> nodePoints = new List<Node>();
                for (int col = 0; col < board_size; col++)
                {
                    Vector2 nodeLocation = new Vector2(row, col);
                    Node node = new Node(nodeLocation, true);
                    nodePoints.Add(node);
                }
                nodes.Add(nodePoints);
            }
            // Initialiseren mocks
            pathfinder = new Mock<PathFinder>(nodes) { CallBase = true};
        }

        /*
        Geef de functie die getest wordt

        Beschrijf duidelijk wat er getest wordt
        */
        [Test]
        public void Test_FindPath_BetweenStartAndEndPoint()
        {
            // Arrange ---------
            Stack<Node> actualPath = new Stack<Node>();
            Stack<Node> path = new Stack<Node>();
            actualPath = pathfinder.Object.FindPath(startPos, endPos);
            
            // Act ---------
            int actual = actualPath.Count;
            int expected = 38;

            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
