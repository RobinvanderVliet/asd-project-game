using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using WorldGeneration;
using WorldGeneration.Models;
using WorldGeneration.StateMachine;
using WorldGeneration.StateMachine.Event;

namespace Creature.Tests.StateMachineTests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class DefaultStateMachineTest
    {
        private DefaultStateMachine _monsterStateMachine;
        private Monster _monster;
        private bool _correctlyTransitioned;

        [SetUp]
        public void Setup()
        {
            _monster = new Monster("Zombie", 15, 15, CharacterSymbol.TERMINATOR, "monster1");
        }

        [Test]
        public void Test_FireEvent_SuccessfulStateChange()
        {
            _correctlyTransitioned = false;
            _monsterStateMachine = new MonsterStateMachine(_monster.MonsterData, null);
            _monsterStateMachine.StartStateMachine();

            //Arrange
            _monsterStateMachine._passiveStateMachine.TransitionCompleted += (sender, args) =>
            {
                _correctlyTransitioned = true;
            };

            _monsterStateMachine._passiveStateMachine.TransitionDeclined += (sender, args) =>
            {
                _correctlyTransitioned = false;
            };

            //Act
            _monsterStateMachine.FireEvent(CharacterEvent.Event.SPOTTED_PLAYER);

            //Assert
            Assert.IsTrue(_correctlyTransitioned);

            // Set State back initial state
            _monsterStateMachine.FireEvent(CharacterEvent.Event.LOST_PLAYER);
        }

        [Test]
        public void Test_FireEvent_NotSuccessfulStateChange()
        {
            _correctlyTransitioned = true;
            _monsterStateMachine = new MonsterStateMachine(_monster.MonsterData, null);
            _monsterStateMachine.StartStateMachine();

            //Arrange
            _monsterStateMachine._passiveStateMachine.TransitionCompleted += (sender, args) =>
            {
                _correctlyTransitioned = true;
            };

            _monsterStateMachine._passiveStateMachine.TransitionDeclined += (sender, args) =>
            {
                _correctlyTransitioned = false;
            };

            _monsterStateMachine._passiveStateMachine.TransitionExceptionThrown += (sender, args) =>
            {
                _correctlyTransitioned = false;
            };

            //Act
            _monsterStateMachine.FireEvent(CharacterEvent.Event.PLAYER_IN_RANGE);

            //Assert
            Assert.IsFalse(_correctlyTransitioned);
        }

        [Test]
        public void Test_FireEvent_SuccessfulStateChangeWithArgument()
        {
            _correctlyTransitioned = false;
            _monsterStateMachine = new MonsterStateMachine(_monster.MonsterData, null);
            _monsterStateMachine.StartStateMachine();

            //Arrange
            _monsterStateMachine._passiveStateMachine.TransitionCompleted += (sender, args) =>
            {
                _correctlyTransitioned = true;
            };

            _monsterStateMachine._passiveStateMachine.TransitionDeclined += (sender, args) =>
            {
                _correctlyTransitioned = false;
            };

            //Act
            _monsterStateMachine.FireEvent(CharacterEvent.Event.SPOTTED_PLAYER, _monster.MonsterData);

            //Assert
            Assert.IsTrue(_correctlyTransitioned);

            // Set State back initial state
            _monsterStateMachine.FireEvent(CharacterEvent.Event.LOST_PLAYER);
        }

        [Test]
        public void Test_FireEvent_NotSuccessfulStateChangeWithArgument()
        {
            _correctlyTransitioned = true;
            _monsterStateMachine = new MonsterStateMachine(_monster.MonsterData, null);
            _monsterStateMachine.StartStateMachine();
            //Arrange
            _monsterStateMachine._passiveStateMachine.TransitionCompleted += (sender, args) =>
            {
                _correctlyTransitioned = true;
            };

            _monsterStateMachine._passiveStateMachine.TransitionDeclined += (sender, args) =>
            {
                _correctlyTransitioned = false;
            };
            _monsterStateMachine._passiveStateMachine.TransitionExceptionThrown += (sender, args) =>
            {
                _correctlyTransitioned = false;
            };

            //Act
            _monsterStateMachine.FireEvent(CharacterEvent.Event.PLAYER_IN_RANGE, _monster.MonsterData);

            //Assert
            Assert.IsFalse(_correctlyTransitioned);
        }

        [Test]
        public void Test_StartStateMachine_StateMachineIsActive()
        {
            //Arrange
            MonsterStateMachine newStateMachine = new MonsterStateMachine(_monster.MonsterData, null);

            //Act
            newStateMachine.StartStateMachine();

            //Assert
            Assert.IsTrue(newStateMachine._passiveStateMachine.IsRunning);
        }
    }
}