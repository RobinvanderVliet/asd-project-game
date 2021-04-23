using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Creature.World;
using Creature.Pathfinder;
using System.Numerics;

namespace Creature.Tests
{
    class DefaultWorldTest
    {
        Mock<ICreature> firstCreatureMock;
        Mock<ICreature> secondCreatureMock;

        private IWorld _sut;

        [SetUp]
        public void Setup()
        {
            firstCreatureMock = new Mock<ICreature>();
            secondCreatureMock = new Mock<ICreature>();

            int size = 2;
            _sut = new DefaultWorld(size);
        }

        [Test]
        public void Test_GenerateWorldNodes_GeneratesWorldNodes()
        {
            // Arrange
            List<List<Node>> expectedNodes = new List<List<Node>>();
            List<Node> horizontalNodes = new List<Node>();
            List<Node> verticalNodes = new List<Node>();

            // Act
            verticalNodes.Add(new Node(new Vector2(0, 0), true));
            verticalNodes.Add(new Node(new Vector2(0, 1), true));
            horizontalNodes.Add(new Node(new Vector2(1, 0), true));
            horizontalNodes.Add(new Node(new Vector2(1, 1), true));
            expectedNodes.Add(verticalNodes);
            expectedNodes.Add(horizontalNodes);

            _sut.GenerateWorldNodes();
            List<List<Node>> actualNodes = _sut.Nodes;
            //System.Diagnostics.Debug.WriteLine(expectedNodes);

            // Assert
            Assert.That(expectedNodes.Count, Is.EqualTo(actualNodes.Count));
            Assert.That(expectedNodes[0].Count, Is.EqualTo(actualNodes[0].Count));
            //Assert.AreEqual(expectedNodes, actualNodes);
        }

        [Test]
        [Ignore("Ignore this test")]
        public void Test_SpawnCreature_CreatureGetsSpawned()
        {
            // Act
            _sut.SpawnCreature(firstCreatureMock.Object);
            _sut.SpawnCreature(secondCreatureMock.Object);

            // Assert
            Assert.AreEqual(2, _sut.creatures.Count);
        }

        [Test]
        [Ignore("Ignore this test")]
        public void Test_SpawnPlayer_PlayerGetsSpawned()
        {
            // Act
            _sut.SpawnPlayer(firstCreatureMock.Object);
            _sut.SpawnPlayer(secondCreatureMock.Object);

            // Assert
            Assert.AreEqual(2, _sut.creatures.Count);
        }
    }
}
