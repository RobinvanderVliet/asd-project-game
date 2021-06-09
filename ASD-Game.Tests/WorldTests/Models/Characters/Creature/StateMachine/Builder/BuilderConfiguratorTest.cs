using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ASD_Game.Items;
using ASD_Game.Items.Consumables;
using ASD_Game.World.Models.Characters;
using ASD_Game.World.Models.Characters.StateMachine.Builder;
using ASD_Game.World.Models.Characters.StateMachine.Data;
using WorldGeneration.StateMachine.CustomRuleSet;
using ASD_Game.World.Models.Characters.StateMachine;
using World.Models.Characters.StateMachine.Event;

namespace Creature.Tests.Creature.StateMachine.Builder
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    class BuilderConfiguratorTest
    {
        private BuilderConfigurator _sut;

        private Item _item1;
        private Item _item2;
        private ICharacterData _creatureData1;
        private ICharacterData _creatureData2;
        private ICharacterData _creatureData3;
        private Mock<ICharacterStateMachine> _creatureStateMachineMock;

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

            RuleSet ruleSet4 = new RuleSet();
            ruleSet4.Setting = "explore";
            ruleSet4.Action = "default";
            ruleSet4.Comparable = "monster";
            ruleSet4.Threshold = "item";
            ruleSet4.Comparison = "finds";
            ruleSet4.ComparisonTrue = "collect";

            RuleSet ruleSet5 = new RuleSet();
            ruleSet5.Setting = "explore";
            ruleSet5.Action = "default";
            ruleSet5.Comparable = "agent";
            ruleSet5.Threshold = "item";
            ruleSet5.Comparison = "finds";
            ruleSet5.ComparisonTrue = "collect";

            RuleSet ruleSet6 = new RuleSet();
            ruleSet6.Setting = "explore";
            ruleSet6.Action = "collect";
            ruleSet6.Comparable = "inventory";
            ruleSet6.Threshold = "item";
            ruleSet6.Comparison = "contains";
            ruleSet6.ComparisonTrue = "wander";

            List<RuleSet> rulesetList = new()
            {
                ruleSet1,
                ruleSet2,
                ruleSet3,
                ruleSet4,
                ruleSet5,
                ruleSet6
            };

            _item1 = new HealthConsumable();
            _item1.ItemName = "apple";
            _item2 = new HealthConsumable();
            _item2.ItemName = "pear";

            _creatureData1 = new AgentData(1, 0, 0);
            Inventory inventory = new();
            inventory.AddConsumableItem((Consumable)_item1);
            _creatureData1.Inventory = inventory;
            _creatureData1.Health = 10;
            _creatureData2 = new AgentData(1, 0, 0);
            _creatureData2.Health = 60;
            _creatureData3 = new MonsterData(1, 0, 0);
            _creatureData3.Inventory = inventory;

            _creatureStateMachineMock = new Mock<ICharacterStateMachine>();

            _sut = new BuilderConfigurator(rulesetList, _creatureData1, _creatureStateMachineMock.Object);
        }

        [Test]
        public void Test_GetBuilderInfoList_ReturnsBuilderInfoListWithEightBuilderInfoItems()
        {
            // Arrange

            // Act
            List<BuilderInfo> builderInfoList = _sut.GetBuilderInfoList();

            // Assert
            Assert.AreEqual(builderInfoList.Count, 8);
        }

        [Test]
        public void Test_GetBuilderInfoList_ReturnsBuilderInfoListWhereEventForFleeActionIsCreatureInRange()
        {
            // Arrange

            // Act
            List<BuilderInfo> builderInfoList = _sut.GetBuilderInfoList();
            BuilderInfo builderInfoFlee = builderInfoList.Where(x => x.Action == "flee").FirstOrDefault();
            CharacterEvent.Event currentEvent = builderInfoFlee.Event;

            // Assert
            Assert.AreEqual(currentEvent, CharacterEvent.Event.CREATURE_IN_RANGE);
        }

        [Test]
        public void Test_GetBuilderInfoList_ReturnsBuilderInfoListWhereEventForFollowActionIsSpottedCreature()
        {
            // Arrange

            // Act
            List<BuilderInfo> builderInfoList = _sut.GetBuilderInfoList();
            BuilderInfo builderInfoFollow = builderInfoList.Where(x => x.Action == "follow").FirstOrDefault();
            CharacterEvent.Event currentEvent = builderInfoFollow.Event;

            // Assert
            Assert.AreEqual(currentEvent, CharacterEvent.Event.SPOTTED_CREATURE);
        }

        [Test]
        public void Test_GetBuilderInfoList_ReturnsBuilderInfoListWhereEventForWanderActionIsFoundItem()
        {
            // Arrange

            // Act
            List<BuilderInfo> builderInfoList = _sut.GetBuilderInfoList();
            BuilderInfo builderInfoWander = builderInfoList.Where(x => x.Action == "wander").FirstOrDefault();
            CharacterEvent.Event currentEvent = builderInfoWander.Event;

            // Assert
            Assert.AreEqual(currentEvent, CharacterEvent.Event.FOUND_ITEM);
        }

        [Test]
        public void Test_GetGuard_ReturnsTrueIfCreatureDataIsInstanceOfAgentAndHealthIsLowerThan50AndExecutedActionIsFlee()
        {
            // Arrange
            List<BuilderInfo> builderInfoList = _sut.GetBuilderInfoList();
            BuilderInfo builderInfoFlee = builderInfoList.Where(x => x.Action == "flee").FirstOrDefault();

            // Act
            bool condition = _sut.GetGuard(_creatureData1, _creatureData1, builderInfoFlee);

            // Assert
            Assert.True(condition);
        }

        [Test]
        public void Test_GetGuard_ReturnsFalseIfCreatureDataIsNotInstanceOfAgentAndHealthIsLowerThan50AndExecutedActionIsFlee()
        {
            // Arrange
            List<BuilderInfo> builderInfoList = _sut.GetBuilderInfoList();
            BuilderInfo builderInfoFlee = builderInfoList.Where(x => x.Action == "flee").FirstOrDefault();

            // Act
            bool condition = _sut.GetGuard(_creatureData3, _creatureData1, builderInfoFlee);

            // Assert
            Assert.False(condition);
        }

        [Test]
        public void Test_GetGuard_ReturnsFalseIfCreatureDataIsInstanceOfAgentAndHealthIsHigherThan50AndExecutedActionIsFlee()
        {
            // Arrange
            List<BuilderInfo> builderInfoList = _sut.GetBuilderInfoList();
            BuilderInfo builderInfoFlee = builderInfoList.Where(x => x.Action == "flee").FirstOrDefault();
            
            // Act
            bool condition = _sut.GetGuard(_creatureData2, _creatureData1, builderInfoFlee);

            // Assert
            Assert.False(condition);
        }

        [Test]
        public void Test_GetGuard_ReturnsTrueIfCreatureDataIsInstanceOfAgentAndHealthIsHigherThan50AndExecutedActionIsAttack()
        {
            // Arrange
            List<BuilderInfo> builderInfoList = _sut.GetBuilderInfoList();
            BuilderInfo builderInfoAttack = builderInfoList.Where(x => x.Action == "attack").FirstOrDefault();

            // Act
            bool condition = _sut.GetGuard(_creatureData2, _creatureData1, builderInfoAttack);

            // Assert
            Assert.True(condition);
        }

        [Test]
        public void Test_GetGuard_ReturnsTrueIfCreatureDataIsInstanceOfAgentAndInventoryContainsItemAndExecutedActionIsWandering()
        {
            // Arrange
            List<BuilderInfo> builderInfoList = _sut.GetBuilderInfoList();
            BuilderInfo builderInfoWander = builderInfoList.Where(x => x.Action == "wander").FirstOrDefault();

            // Act
            bool condition = _sut.GetGuard(_creatureData1, _item1, builderInfoWander);

            // Assert
            Assert.True(condition);
        }

        [Test]
        public void Test_GetGuard_ReturnsFalseIfCreatureDataIsInstanceOfAgentAndInventoryDoesNotContainItemAndExecutedActionIsWandering()
        {
            // Arrange
            List<BuilderInfo> builderInfoList = _sut.GetBuilderInfoList();
            BuilderInfo builderInfoWander = builderInfoList.Where(x => x.Action == "wander").FirstOrDefault();

            // Act
            bool condition = _sut.GetGuard(_creatureData1, _item2, builderInfoWander);

            // Assert
            Assert.False(condition);
        }

        [Test]
        public void Test_GetGuard_ReturnsFalseIfCreatureDataIsNotInstanceOfAgentAndExecutedActionIsWandering()
        {
            // Arrange
            List<BuilderInfo> builderInfoList = _sut.GetBuilderInfoList();
            BuilderInfo builderInfoWander = builderInfoList.Where(x => x.Action == "wander").FirstOrDefault();

            // Act
            bool condition = _sut.GetGuard(_creatureData3, _item1, builderInfoWander);

            // Assert
            Assert.False(condition);
        }
    }
}