using Creature.Creature.StateMachine.CustomRuleSet;
using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Creature.Tests.Creature.StateMachine.CustomRuleSet
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    class RulesetFactoryTest
    {
        List<KeyValuePair<string, string>> _rulesetSettingsList;
        private RuleSetFactory _sut;

        [SetUp]
        public void Setup()
        {
            _rulesetSettingsList = new()
            {
                new("aggressiveness", "high"),
                new("explore", "random"),
                new("combat_default_agent_comparable", "agent"),
                new("combat_default_agent_treshold", "monster"),
                new("combat_default_agent_comparison", "sees"),
                new("combat_default_agent_comparison_true", "follow"),
                new("combat_default_agent_comparable", "agent"),
                new("combat_default_agent_treshold", "agent"),
                new("combat_default_agent_comparison", "nearby"),
                new("combat_default_agent_comparison_true", "engage"),
                new("combat_engage_agent_comparable", "health"),
                new("combat_engage_agent_treshold", "50"),
                new("combat_engage_agent_comparison", "less than"),
                new("combat_engage_agent_comparison_true", "attack"),
                new("combat_engage_agent_comparison_false", "flee"),
            };
            _sut = new RuleSetFactory();
        }

        [Test]
        public void Test_GetRuleSetListFromSettingsList_ReturnsRulesetListWithThreeItems()
        {
            // Arrange

            // Act
            List<RuleSet> rulesetList = _sut.GetRuleSetListFromSettingsList(_rulesetSettingsList);

            // Assert
            Assert.AreEqual(rulesetList.Count, 3);
        }

        [Test]
        public void Test_GetRuleSetListFromSettingsList_ReturnsRulesetListWithCorrectSettingParameterPerRuleset()
        {
            // Arrange

            // Act
            List<RuleSet> rulesetList = _sut.GetRuleSetListFromSettingsList(_rulesetSettingsList);
            string setting1 = rulesetList[0].Setting;
            string setting2 = rulesetList[1].Setting;
            string setting3 = rulesetList[2].Setting;

            // Assert
            Assert.AreEqual(setting1, "combat");
            Assert.AreEqual(setting2, "combat");
            Assert.AreEqual(setting3, "combat");
        }

        [Test]
        public void Test_GetRuleSetListFromSettingsList_ReturnsRulesetListWithCorrectActionParameterPerRuleset()
        {
            // Arrange

            // Act
            List<RuleSet> rulesetList = _sut.GetRuleSetListFromSettingsList(_rulesetSettingsList);
            string action1 = rulesetList[0].Action;
            string action2 = rulesetList[1].Action;
            string action3 = rulesetList[2].Action;

            // Assert
            Assert.AreEqual(action1, "default");
            Assert.AreEqual(action2, "default");
            Assert.AreEqual(action3, "engage");
        }

        [Test]
        public void Test_GetSimpleRuleSetListFromSettingsList_ReturnsRulesetListWithTwoItems()
        {
            // Arrange

            // Act
            List<KeyValuePair<string, string>> rulesetList = _sut.GetSimpleRuleSetListFromSettingsList(_rulesetSettingsList);

            // Assert
            Assert.AreEqual(rulesetList.Count, 2);
        }
    }
}