using Creature.Pathfinder;
using Creature.World;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Creature.Creature;

namespace Creature.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    class DefaultWorldTest
    {
        private Mock<ICreature> _firstCreatureMock;
        private Mock<ICreature> _secondCreatureMock;

        private IWorld _sut;

        [SetUp]
        public void Setup()
        {
            _firstCreatureMock = new Mock<ICreature>();
            _secondCreatureMock = new Mock<ICreature>();

            int size = 2;
            _sut = new DefaultWorld(size);
        }

        [Test]
        public void Test_GenerateWorldNodes_GeneratesWorldNodes()
        {
            // Arrange  ---------
            List<List<Node>> expectedNodes = new List<List<Node>>();
            List<Node> horizontalNodes = new List<Node>();
            List<Node> verticalNodes = new List<Node>();

            // Act  -------------
            verticalNodes.Add(new Node(new Vector2(0, 0), true));
            verticalNodes.Add(new Node(new Vector2(0, 1), true));
            horizontalNodes.Add(new Node(new Vector2(1, 0), true));
            horizontalNodes.Add(new Node(new Vector2(1, 1), true));
            expectedNodes.Add(verticalNodes);
            expectedNodes.Add(horizontalNodes);

            _sut.GenerateWorldNodes();
            List<List<Node>> actualNodes = _sut.Nodes;
            //System.Diagnostics.Debug.WriteLine(expectedNodes);

            // Assert ----------
            Assert.That(expectedNodes.Count, Is.EqualTo(actualNodes.Count));
            Assert.That(expectedNodes[0].Count, Is.EqualTo(actualNodes[0].Count));
            //Assert.AreEqual(expectedNodes, actualNodes);
        }

        [Test]
        [Ignore("Ignore this test")]
        public void Test_SpawnCreature_CreatureGetsSpawned()
        {
            // Arrange  ---------

            // Act  -------------
            _sut.SpawnCreature(_firstCreatureMock.Object);
            _sut.SpawnCreature(_secondCreatureMock.Object);

            // Assert ----------
            Assert.AreEqual(2, _sut.Creatures.Count);
        }

        [Test]
        [Ignore("Ignore this test")]
        public void Test_SpawnPlayer_PlayerGetsSpawned()
        {
            // Arrange  ---------

            // Act  -------------
            _sut.SpawnPlayer(_firstCreatureMock.Object);
            _sut.SpawnPlayer(_secondCreatureMock.Object);

            // Assert ----------
            Assert.AreEqual(2, _sut.Creatures.Count);
        }
    }
}
