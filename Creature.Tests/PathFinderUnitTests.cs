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
    class PathFinderUnitTests
    {
        //Declaratie en initialisatie constante variabelen
        //Declaratie variabelen
        List<List<Node>> nodes = new List<List<Node>>();
        int board_size = 20;
        Vector2 startPos = new Vector2(0, 0);
        Vector2 endPos = new Vector2(19, 19);
        //Declaratie mocks
        Mock<PathFinder> pathfinder;

            [SetUp]
            public void Setup()
            {
            //Initialiseren variabelen
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
            //Initialiseren mocks
            pathfinder = new Mock<PathFinder>(nodes) { CallBase = true};
        }

            /*
            Geef de functie die getest wordt

            Beschrijf duidelijk wat er getest wordt
            */
            [Test]
            public void Test_FindPath_Between_Start_And_End_Point()
            {
            //Arrange ---------
            Stack<Node> actualPath = new Stack<Node>();
            Stack<Node> path = new Stack<Node>();
            actualPath = pathfinder.Object.FindPath(startPos, endPos);
            //Assert ---------
            int actual = actualPath.Count;
            int expected = 38;

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
