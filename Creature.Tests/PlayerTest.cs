using Moq;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Creature.Creature;
using Creature.Creature.StateMachine;
using Creature.Creature.StateMachine.Data;
using Network;

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
            _sut = new Creature.Player(_creatureStateMachineMock.Object, _clientControllerMock.Object);
        }

        [Test]
        public void Test_ApplyDamage_DealsDamage()
        {
            // Arrange ---------
            PlayerData playerData = new PlayerData(new Vector2(), 50, 10, 10, null);
            _creatureStateMachineMock.Setup(c => c.CreatureData).Returns(playerData);
            
            // Act -------------
            _sut.ApplyDamage(30);

            // Assert ----------
            Assert.AreEqual(_sut.CreatureStateMachine.CreatureData.Health, 20);
        }
        
        [Test]
        public void Test_HealAmount_HealsPlayer()
        {
            // Arrange ---------
            PlayerData playerData = new PlayerData(new Vector2(), 30, 10, 10, null);
            _creatureStateMachineMock.Setup(c => c.CreatureData).Returns(playerData);
            
            // Act -------------
            _sut.HealAmount(10);

            // Assert ----------
            Assert.AreEqual(_sut.CreatureStateMachine.CreatureData.Health, 40);
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