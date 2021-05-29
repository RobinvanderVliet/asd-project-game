using Creature.Creature;
using Creature.Creature.NeuralNetworking;
using Creature.Creature.NeuralNetworking.TrainingScenario;
using Creature.Creature.StateMachine.Data;
using Creature.Pathfinder;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Creature.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    internal class SmartCreatureActionsTest
    {
        private SmartCreatureActions _sut;
        private SmartMonster _smartTestMonster;
        private TrainingMapGenerator trainingMap;
        private TrainerAI _AI;
        private Node _node;

        private Vector2 loc;

        [SetUp]
        public void Setup()
        {
            trainingMap = new TrainingMapGenerator();
            MonsterData _MonsterData =
                new MonsterData
                (
                    new Vector2(14, 14),
                    20,
                    5,
                    200,
                    null,
                    false
                );
            _smartTestMonster = new SmartMonster(_MonsterData);
            _sut = new SmartCreatureActions(trainingMap.trainingmap);
            loc = new Vector2(15, 15);
        }

        [Test]
        public void Test_Wander()
        {
            _sut.Wander(_smartTestMonster);

            Assert.NotNull(_sut.path);
        }

        [Test]
        public void Test_Walk1()
        {
            float currLocation = _smartTestMonster.creatureData.Position.Y;
            _sut.WalkUp(_smartTestMonster);

            float expected = currLocation + 1;
            float actual = _smartTestMonster.creatureData.Position.Y;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test_Walk2()
        {
            float currLocation = _smartTestMonster.creatureData.Position.Y;
            _sut.WalkDown(_smartTestMonster);

            float expected = currLocation - 1;
            float actual = _smartTestMonster.creatureData.Position.Y;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test_Walk3()
        {
            float currLocation = _smartTestMonster.creatureData.Position.X;
            _sut.WalkLeft(_smartTestMonster);

            float expected = currLocation - 1;
            float actual = _smartTestMonster.creatureData.Position.X;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test_Walk4()
        {
            float currLocation = _smartTestMonster.creatureData.Position.X;
            _sut.WalkRight(_smartTestMonster);

            float expected = currLocation + 1;
            float actual = _smartTestMonster.creatureData.Position.X;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test_Attack_Hit()
        {
            Vector2 AIloc = new Vector2(15, 14);
            _AI = new TrainerAI(AIloc, "player");

            _sut.Attack(_AI, _smartTestMonster);

            int expected = 5;
            int actual = _smartTestMonster.DamageDealt;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test_Attack_Miss()
        {
            Vector2 AIloc = new Vector2(20, 20);
            _AI = new TrainerAI(AIloc, "player");

            _sut.Attack(_AI, _smartTestMonster);

            int expected = 0;
            int actual = _smartTestMonster.DamageDealt;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test_Flee()
        {
            Vector2 AIloc = new Vector2(15, 15);
            _AI = new TrainerAI(AIloc, "player");

            _sut.Flee(_AI, _smartTestMonster);

            Assert.NotNull(_sut.path);
        }

        [Test]
        public void Test_RunToMonster()
        {
            Vector2 AIloc = new Vector2(20, 20);
            _AI = new TrainerAI(AIloc, "monster");

            _sut.RunToMonster(_AI, _smartTestMonster);

            Vector2 expected = new Vector2(14, 16);
            Vector2 actual = _sut.path.Peek().Position;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test_TakeDamage()
        {
            int damage = 10;
            _sut.TakeDamage(damage, _smartTestMonster);

            int expected = damage;
            int actual = _smartTestMonster.DamageTaken;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test_TakeDamage_And_Die()
        {
            _smartTestMonster.creatureData.Health = 5;
            int damage = 10;

            _sut.TakeDamage(damage, _smartTestMonster);

            Assert.True(_smartTestMonster.dead);
        }
    }
}