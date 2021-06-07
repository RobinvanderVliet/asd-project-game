using ASD_project.Creature.Creature.StateMachine.Builder;
using Creature.Creature.StateMachine;
using Creature.Creature.StateMachine.Builder;
using Creature.Creature.StateMachine.CustomRuleSet;
using Creature.Creature.StateMachine.Data;
using Creature.Creature.StateMachine.Event;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;

namespace Creature.Tests.Creature.StateMachine.Builder
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    class BuilderConfiguratorTest
    {
        private BuilderConfigurator _sut;

        private ICreatureData _creatureData1;
        private ICreatureData _creatureData2;
        private ICreatureData _creatureData3;
        private Mock<ICreatureStateMachine> _creatureStateMachineMock;

        [SetUp]
        public void Setup()
        {
            RuleSet ruleSet1 = new RuleSet();
            ruleSet1.Setting = "combat";
            ruleSet1.Action = "default";
            ruleSet1.Comparable = "agent";
            ruleSet1.Threshold = "monster";
            ruleSet1.Comparison = "sees";
            ruleSet1.ComparisonTrue = "follow";

            RuleSet ruleSet2 = new RuleSet();
            ruleSet2.Setting = "combat";
            ruleSet2.Action = "default";
            ruleSet2.Comparable = "agent";
            ruleSet2.Threshold = "agent";
            ruleSet2.Comparison = "nearby";
            ruleSet2.ComparisonTrue = "engage";

            RuleSet ruleSet3 = new RuleSet();
            ruleSet3.Setting = "combat";
            ruleSet3.Action = "engage";
            ruleSet3.Comparable = "health";
            ruleSet3.Threshold = "50";
            ruleSet3.Comparison = "less than";
            ruleSet3.ComparisonTrue = "flee";
            ruleSet3.ComparisonFalse = "attack";

            List<RuleSet> rulesetList = new()
            {
                ruleSet1,
                ruleSet2,
                ruleSet3
            };

            _creatureData1 = new AgentData(1, 0, 0);
            _creatureData1.Health = 10;
            _creatureData2 = new AgentData(1, 0, 0);
            _creatureData2.Health = 60;
            _creatureData3 = new MonsterData(1, 0, 0);
            _creatureStateMachineMock = new Mock<ICreatureStateMachine>();

            _sut = new BuilderConfigurator(rulesetList, _creatureData1, _creatureStateMachineMock.Object);
        }

        [Test]
        public void Test_GetBuilderInfoList_ReturnsBuilderInfoListWithFourBuilderInfoItems()
        {
            // Arrange

            // Act
            List<BuilderInfo> builderInfoList = _sut.GetBuilderInfoList();

            // Assert
            Assert.AreEqual(builderInfoList.Count, 4);
        }

        [Test]
        public void Test_GetBuilderInfoList_ReturnsBuilderInfoListWhereEventForFleeActionIsCreatureInRange()
        {
            // Arrange

            // Act
            List<BuilderInfo> builderInfoList = _sut.GetBuilderInfoList();
            BuilderInfo builderInfoFollow = builderInfoList.Where(x => x.Action == "flee").FirstOrDefault();
            CreatureEvent.Event currentEvent = builderInfoFollow.Event;

            // Assert
            Assert.AreEqual(currentEvent, CreatureEvent.Event.CREATURE_IN_RANGE);
        }

        [Test]
        public void Test_GetBuilderInfoList_ReturnsBuilderInfoListWhereEventForFollowActionIsSpottedCreature()
        {
            // Arrange

            // Act
            List<BuilderInfo> builderInfoList = _sut.GetBuilderInfoList();
            BuilderInfo builderInfoFollow = builderInfoList.Where(x => x.Action == "follow").FirstOrDefault();
            CreatureEvent.Event currentEvent = builderInfoFollow.Event;

            // Assert
            Assert.AreEqual(currentEvent, CreatureEvent.Event.SPOTTED_CREATURE);
        }

        [Test]
        public void Test_GetGuard_ReturnsTrueIfCreatureDataIsInstanceOfAgentAndHealthIsLowerThan50AndExecutedActionIsFlee()
        {
            // Arrange
            List<BuilderInfo> builderInfoList = _sut.GetBuilderInfoList();
            BuilderInfo builderInfoAttack = builderInfoList.Where(x => x.Action == "flee").FirstOrDefault();

            // Act
            bool condition = _sut.GetGuard(_creatureData1, _creatureData1, builderInfoAttack);

            // Assert
            Assert.True(condition);
        }

        [Test]
        public void Test_GetGuard_ReturnsFalseIfCreatureDataIsNotInstanceOfAgentAndHealthIsLowerThan50AndExecutedActionIsFlee()
        {
            // Arrange
            List<BuilderInfo> builderInfoList = _sut.GetBuilderInfoList();
            BuilderInfo builderInfoAttack = builderInfoList.Where(x => x.Action == "flee").FirstOrDefault();

            // Act
            bool condition = _sut.GetGuard(_creatureData3, _creatureData1, builderInfoAttack);

            // Assert
            Assert.False(condition);
        }

        [Test]
        public void Test_GetGuard_ReturnsFalseIfCreatureDataIsInstanceOfAgentAndHealthIsHigherThan50AndExecutedActionIsFlee()
        {
            // Arrange
            List<BuilderInfo> builderInfoList = _sut.GetBuilderInfoList();
            BuilderInfo builderInfoAttack = builderInfoList.Where(x => x.Action == "flee").FirstOrDefault();

            // Act
            bool condition = _sut.GetGuard(_creatureData2, _creatureData1, builderInfoAttack);

            // Assert
            Assert.False(condition);
        }

        [Test]
        public void Test_GetGuard_ReturnsTrueIfCreatureDataIsInstanceOfAgentAndHealthIsHigherThan50AndExecutedActionIsAttack()
        {
            // Arrange
            List<BuilderInfo> builderInfoList = _sut.GetBuilderInfoList();
            BuilderInfo builderInfoFlee = builderInfoList.Where(x => x.Action == "attack").FirstOrDefault();

            // Act
            bool condition = _sut.GetGuard(_creatureData2, _creatureData1, builderInfoFlee);

            // Assert
            Assert.True(condition);
        }
    }
}