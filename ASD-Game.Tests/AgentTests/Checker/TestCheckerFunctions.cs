using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ASD_Game.Agent.Antlr.Ast;
using ASD_Game.Agent.Antlr.Ast.Comparables;
using ASD_Game.Agent.Antlr.Ast.Comparables.Subjects;
using ASD_Game.Agent.Antlr.Checker;
using Moq;
using NUnit.Framework;
using Action = ASD_Game.Agent.Antlr.Ast.Action;

namespace ASD_Game.Tests.AgentTests.Checker
{
    [ExcludeFromCodeCoverage]
    public class TestCheckerFunctions
    {
        private Checking _sut;
        private const string ERROR = "ERROR: ";
        private const string TESTHASNOERROR = null;

        [SetUp]
        public void Setup()
        {
            _sut = new Checking();
        }
        

        
        [TestCaseSource("CheckTestCases")]
        [Test]
        public void Test_Check(Node node, string error)
        {
            //Arrange
            //Act
            _sut.Check(node);
            
            //Assert
            if (error is TESTHASNOERROR)
            {
                Assert.IsFalse(node.HasError());
            }
            else
            {
                Assert.IsTrue(node.HasError());
                Assert.AreEqual(ERROR + error, node.GetError().ToString());
            }
        }

        public static IEnumerable<TestCaseData> CheckTestCases
        {
            get
            {
                yield return new TestCaseData(new ActionReference("use").AddChild(new Item("katana")), TESTHASNOERROR).SetName("correct_actionreference_use");
                yield return new TestCaseData(new ActionReference("use"), "Use must be followed by an item!").SetName("acttionreference_use_withoutitem");
                yield return new TestCaseData(new ActionReference("test"), "'test' is not a valid action!").SetName("wrong_actionreference");
                yield return new TestCaseData(new ActionReference("attack"), TESTHASNOERROR).SetName("corect_actionreference");
                yield return new TestCaseData(new Rule("combat", "offensive"), TESTHASNOERROR).SetName("correct_rule");
                yield return new TestCaseData(new Rule("combat", "test"), "'test' is not a valid value!").SetName("rule_wrong_value");
                yield return new TestCaseData(new Rule("test", "offensive"), "'test' is not a setting!").SetName("rule_wrong_setting");
                yield return new TestCaseData(new Rule("test", "test"), "'test' is not a valid value!").SetName("rule_wrong_setting_and_value");
                yield return new TestCaseData(new Condition()
                    .AddChild(new Otherwise().AddChild(new ActionReference("attack")))
                    .AddChild(new When().AddChild(new ActionReference("walk"))), TESTHASNOERROR).SetName("correct_condition_otherwise_action");
                yield return new TestCaseData(new Condition()
                    .AddChild(new When().AddChild(new ActionReference("walk"))), TESTHASNOERROR).SetName("correct_condition_no_otherwise");
                yield return new TestCaseData(new Condition()
                    .AddChild(new Otherwise().AddChild(new ActionReference("attack")))
                    .AddChild(new When().AddChild(new ActionReference("attack"))), "Otherwise action cant be the same as then action!").SetName("condition_otherwise_same_as_then");
                yield return new TestCaseData(new When()
                    .AddChild(new Comparison("contains"))
                    .AddChild(new Inventory("inventory"))
                    .AddChild(new Item("katana")), TESTHASNOERROR).SetName("correct_contains");
                yield return new TestCaseData(new When()
                    .AddChild(new Comparison("does not contain"))
                    .AddChild(new Inventory("inventory"))
                    .AddChild(new Item("katana")), TESTHASNOERROR).SetName("correct_does_not_contain");
                yield return new TestCaseData(new When()
                    .AddChild(new Comparison("contains"))
                    .AddChild(new Comparable())
                    .AddChild(new Item("katana")), "Left side of the comparison 'contains' must be inventory!").SetName("contains_no_inventory");
                yield return new TestCaseData(new When()
                    .AddChild(new Comparison("contains"))
                    .AddChild(new Inventory("inventory"))
                    .AddChild(new AgentSubject("agent")), "Right side of the comparison 'contains' must be an item!").SetName("contains_no_item");
                yield return new TestCaseData(new When()
                    .AddChild(new Comparison("greater than"))
                    .AddChild(new Stat("stat"))
                    .AddChild(new Int(5)), TESTHASNOERROR).SetName("correct_greater_than");
                yield return new TestCaseData(new When()
                    .AddChild(new Comparison("less than"))
                    .AddChild(new Stat("stat"))
                    .AddChild(new Int(5)), TESTHASNOERROR).SetName("correct_less_than");
                yield return new TestCaseData(new When()
                    .AddChild(new Comparison("equals"))
                    .AddChild(new Stat("stat"))
                    .AddChild(new Int(5)), TESTHASNOERROR).SetName("correct_equals");
                yield return new TestCaseData(new When()
                    .AddChild(new Comparison("equals"))
                    .AddChild(new Comparable())
                    .AddChild(new Int(5)), "Left side of the comparison 'equals' must be a stat!").SetName("equals_no_stat");
                yield return new TestCaseData(new When()
                    .AddChild(new Comparison("equals"))
                    .AddChild(new Stat("stat"))
                    .AddChild(new Comparable()), "Right side of the comparison 'equals' must be an int value!").SetName("equals_no_int");
                yield return new TestCaseData(new When()
                    .AddChild(new Comparison("nearby"))
                    .AddChild(new AgentSubject("agent"))
                    .AddChild(new NPC("npc")), TESTHASNOERROR).SetName("correct_nearby");
                yield return new TestCaseData(new When()
                    .AddChild(new Comparison("finds"))
                    .AddChild(new AgentSubject("agent"))
                    .AddChild(new NPC("npc")), TESTHASNOERROR).SetName("correct_finds");
                yield return new TestCaseData(new When()
                    .AddChild(new Comparison("nearby"))
                    .AddChild(new Comparable())
                    .AddChild(new NPC("npc")), "Left side of the comparison 'nearby' must be agent!").SetName("nearby_no_agent");
                yield return new TestCaseData(new When()
                    .AddChild(new Comparison("nearby"))
                    .AddChild(new AgentSubject("agent"))
                    .AddChild(new Inventory("inventory")), "Right side of the comparison 'nearby' cant be agent or inventory").SetName("nearby_inventory");
                yield return new TestCaseData(new When()
                    .AddChild(new Comparison("nearby"))
                    .AddChild(new AgentSubject("agent"))
                    .AddChild(new AgentSubject("npc")), "Right side of the comparison 'nearby' cant be agent or inventory").SetName("nearby_wrong_agent");
                yield return new TestCaseData(new When()
                        .AddChild(new Comparison("nearby"))
                        .AddChild(new AgentSubject("agent"))
                        .AddChild(new Comparable()), "Right side of the comparison 'nearby' must be a subject!").SetName("nearby_no_subject");
                yield return new TestCaseData(new When()
                    .AddChild(new Comparison("test")), "'test' does not exist").SetName("comparison_does_not_exist");
                yield return new TestCaseData(new Action("engage")
                    .AddChild(new Condition()
                        .AddChild(new When()
                            .AddChild(new ActionReference("grab")))), TESTHASNOERROR).SetName("correct_action");
                yield return new TestCaseData(new Action("engage")
                    .AddChild(new Condition()
                        .AddChild(new When()
                            .AddChild(new ActionReference("engage")))), "No recursion allowed!").SetName("action_when_recusrsion");
                yield return new TestCaseData(new Action("engage")
                    .AddChild(new Condition()
                        .AddChild(new Otherwise()
                            .AddChild(new ActionReference("engage")))
                        .AddChild(new When()
                            .AddChild(new ActionReference("grab")))), "No recursion allowed!").SetName("action_otherwise_recusrsion");
                yield return new TestCaseData(new Action("test"), "'test' is not a valid/programmable action!").SetName("invalid_action_name");
                yield return new TestCaseData(new Item("katana"), TESTHASNOERROR).SetName("correct_item");
                yield return new TestCaseData(new Item("test"), "'test' is not a valid item!").SetName("invalid_item");
                yield return new TestCaseData(new Tile("street"), TESTHASNOERROR).SetName("correct_tile");
                yield return new TestCaseData(new Tile("test"), "'test' is not a valid tile!").SetName("invalid_tile");

            }
        }
    }
}