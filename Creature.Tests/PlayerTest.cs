using Moq;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Creature.Creature.StateMachine;
using Creature.Creature.StateMachine.Data;

namespace Creature.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    class PlayerTest
    {
        private Player _sut;
        private Mock<ICreatureStateMachine> creatureStateMachineMock;

        [SetUp]
        public void Setup()
        {
            creatureStateMachineMock = new Mock<ICreatureStateMachine>();
            _sut = new Player(creatureStateMachineMock.Object);
        }

        [Test]
        public void Test_ApplyDamage_Deals_Damage()
        {
            // Arrange ---------
            PlayerData playerData = new PlayerData(new Vector2(), 50, 10, 10, null);
            creatureStateMachineMock.Setup(c => c.CreatureData).Returns(playerData);
            
            // Act -------------
            _sut.ApplyDamage(30);

            // Assert ----------
            Assert.AreEqual(_sut.CreatureStateMachine.CreatureData.Health, 20);
        }
        
        [Test]
        public void Test_HealAmount_Heals_Player()
        {
            // Arrange ---------
            PlayerData playerData = new PlayerData(new Vector2(), 30, 10, 10, null);
            creatureStateMachineMock.Setup(c => c.CreatureData).Returns(playerData);
            
            // Act -------------
            _sut.HealAmount(10);

            // Assert ----------
            Assert.AreEqual(_sut.CreatureStateMachine.CreatureData.Health, 40);
        }
        
        [Test]
        public void Test_Disconnect_Starts_State_Machine()
        {
            // Arrange ---------

            // Act -------------
            _sut.Disconnect();

            // Assert ----------
            creatureStateMachineMock.Verify(creatureStateMachine => creatureStateMachine.StartStateMachine());
        }
    }
}