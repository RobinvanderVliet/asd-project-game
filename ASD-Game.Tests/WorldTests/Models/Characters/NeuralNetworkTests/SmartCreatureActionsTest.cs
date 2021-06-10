using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using ASD_Game.World.Models.Characters.Algorithms.NeuralNetworking.TrainingScenario;
using ASD_Game.World.Models.Characters.Algorithms.Pathfinder;
using ASD_Game.World.Models.Characters.StateMachine.Data;
using NUnit.Framework;

namespace ASD_Game.Tests.WorldTests.Models.Characters.NeuralNetworkTests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    internal class SmartCreatureActionsTest
    {
        private SmartCreatureTrainingActions _sut;
        private SmartMonsterForTraining _smartTestMonster;
        private TrainingMapGenerator _trainingMap;
        private TrainerAI _AI;
        private Node _node;

        private Vector2 loc;

        [SetUp]
        public void Setup()
        {
            _trainingMap = new TrainingMapGenerator();
            MonsterData _MonsterData =
                new MonsterData
                (
                14,
                14,
                0
                );
            _smartTestMonster = new SmartMonsterForTraining("Zombie", 14, 14, "T", "monst");
            _sut = new SmartCreatureTrainingActions(_trainingMap.trainingmap);
            loc = new Vector2(15, 15);
        }

        [Test]
        public void Test_Wander_WanderABit()
        {
            //act
            _sut.Wander(_smartTestMonster);

            //assert
            Assert.NotNull(_sut.Path);
        }

        [Test]
        public void Test_Walk1_Walkup()
        {
            //arrange
            float currLocation = _smartTestMonster.CreatureData.Position.Y;

            //act
            _sut.WalkUp(_smartTestMonster);

            //assert
            float expected = currLocation + 1;
            float actual = _smartTestMonster.CreatureData.Position.Y;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test_Walk2_WalkDown()
        {
            //arrange
            float currLocation = _smartTestMonster.CreatureData.Position.Y;

            //act
            _sut.WalkDown(_smartTestMonster);

            //assert
            float expected = currLocation - 1;
            float actual = _smartTestMonster.CreatureData.Position.Y;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test_Walk3_WalkLeft()
        {
            //arrange
            float currLocation = _smartTestMonster.CreatureData.Position.X;

            //act
            _sut.WalkLeft(_smartTestMonster);

            //assert
            float expected = currLocation - 1;
            float actual = _smartTestMonster.CreatureData.Position.X;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test_Walk4_WalkRight()
        {
            //arrange
            float currLocation = _smartTestMonster.CreatureData.Position.X;

            //act
            _sut.WalkRight(_smartTestMonster);

            //assert
            float expected = currLocation + 1;
            float actual = _smartTestMonster.CreatureData.Position.X;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test_Attack_Hit()
        {
            //arrange
            Vector2 AIloc = new Vector2(15, 14);
            _AI = new TrainerAI(AIloc, "player");

            //act
            _sut.Attack(_AI, _smartTestMonster);

            //assert
            int expected = 5;
            int actual = _smartTestMonster.DamageDealt;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test_Attack_Miss()
        {
            //arrange
            Vector2 AIloc = new Vector2(20, 20);
            _AI = new TrainerAI(AIloc, "player");

            //act
            _sut.Attack(_AI, _smartTestMonster);

            //assert
            int expected = 0;
            int actual = _smartTestMonster.DamageDealt;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test_Flee_Run()
        {
            //arrange
            Vector2 AIloc = new Vector2(15, 15);
            _AI = new TrainerAI(AIloc, "player");

            //act
            _sut.Flee(_AI, _smartTestMonster);

            //assert
            Assert.NotNull(_sut.Path);
        }

        [Test]
        public void Test_RunToMonster_WalkTowardsIt()
        {
            //arrange
            Vector2 AIloc = new Vector2(20, 20);
            _AI = new TrainerAI(AIloc, "monster");

            //act
            _sut.RunToMonster(_AI, _smartTestMonster);

            //assert
            Vector2 expected = new Vector2(14, 16);
            Vector2 actual = _sut.Path.Peek().Position;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test_TakeDamage_AndDontDie()
        {
            //arrange
            int damage = 10;

            //act
            _sut.TakeDamage(damage, _smartTestMonster);

            //assert
            int expected = damage;
            int actual = _smartTestMonster.DamageTaken;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test_TakeDamage_And_Die()
        {
            //arrange
            _smartTestMonster.CreatureData.Health = 5;
            int damage = 10;

            //act
            _sut.TakeDamage(damage, _smartTestMonster);

            //assert
            Assert.True(_smartTestMonster.Dead);
        }
    }
}