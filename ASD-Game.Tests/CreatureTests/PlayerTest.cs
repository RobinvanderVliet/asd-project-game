using Creature.Creature.StateMachine;
using Creature.Creature.StateMachine.Data;
using Moq;
using Network;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Creature.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    class PlayerTest
    {
        private Creature.Player _sut;
        private Mock<ICreatureStateMachine> _creatureStateMachineMock;
        private Mock<Network.IClientController> _clientControllerMock;

        [SetUp]
        public void Setup()
        {
            _creatureStateMachineMock = new Mock<ICreatureStateMachine>();
            _clientControllerMock = new Mock<IClientController>();
            _sut = new Creature.Player(_creatureStateMachineMock.Object);
        }

        [Test]
        public void Test_ApplyDamage_DealsDamage()
        {
            // Arrange ---------
            PlayerData playerData = new PlayerData(new Vector2(), 50, 10, 10);
            _creatureStateMachineMock.Setup(c => c.CharacterData).Returns(playerData);

            // Act -------------
            _sut.ApplyDamage(30);

            // Assert ----------
            Assert.AreEqual(_sut.CreatureStateMachine.CharacterData.Health, 20);
        }

        [Test]
        public void Test_HealAmount_HealsPlayer()
        {
            // Arrange ---------
            PlayerData playerData = new PlayerData(new Vector2(), 30, 10, 10);
            _creatureStateMachineMock.Setup(c => c.CharacterData).Returns(playerData);

            // Act -------------
            _sut.HealAmount(10);

            // Assert ----------
            Assert.AreEqual(_sut.CreatureStateMachine.CharacterData.Health, 40);
        }

        [Test]
        public void Test_Disconnect_StartsStateMachine()
        {
            // Arrange ---------

            // Act -------------
            _sut.Disconnect();

            // Assert ----------
            _creatureStateMachineMock.Verify(creatureStateMachine => creatureStateMachine.StartStateMachine());
        }
    }
}