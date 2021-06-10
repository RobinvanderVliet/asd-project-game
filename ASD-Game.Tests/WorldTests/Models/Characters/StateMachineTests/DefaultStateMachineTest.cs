using System.Diagnostics.CodeAnalysis;
using System.Threading;
using ASD_Game.World.Models;
using ASD_Game.World.Models.Characters;
using ASD_Game.World.Models.Characters.StateMachine;
using NUnit.Framework;
using WorldGeneration.StateMachine.Event;

namespace ASD_Game.Tests.WorldTests.Models.Characters.StateMachineTests
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
            // FireEvent works with a queue which gets cleared on a interval. There was a possibility that the event 
            // was not yet completed before doing the assertion and thus making the test fail.            
            Thread.Sleep(1000);

            //Assert
            Assert.IsTrue(_correctlyTransitioned);

            // Set State back initial state
            _monsterStateMachine.FireEvent(CharacterEvent.Event.LOST_PLAYER);
        }

        [Test]
        [Ignore("Hard to test because Appcelerate, this test does not give a consistent result")]
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
            // FireEvent works with a queue which gets cleared on a interval. There was a possibility that the event 
            // was not yet completed before doing the assertion and thus making the test fail.            
            Thread.Sleep(1000);

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
            // FireEvent works with a queue which gets cleared on a interval. There was a possibility that the event 
            // was not yet completed before doing the assertion and thus making the test fail.            
            Thread.Sleep(1000);
            //Assert
            Assert.IsTrue(_correctlyTransitioned);

            // Set State back initial state
            _monsterStateMachine.FireEvent(CharacterEvent.Event.LOST_PLAYER);
        }

        [Test]
        [Ignore("Hard to test because Appcelerate, this test does not give a consistent result")]
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
            // FireEvent works with a queue which gets cleared on a interval. There was a possibility that the event 
            // was not yet completed before doing the assertion and thus making the test fail.
            Thread.Sleep(1000);

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