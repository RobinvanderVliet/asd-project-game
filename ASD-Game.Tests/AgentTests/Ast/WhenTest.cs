using System;
using System.Diagnostics.CodeAnalysis;
using ASD_Game.Agent.Antlr.Ast;
using ASD_Game.Agent.Antlr.Ast.Comparables;
using ASD_Game.Agent.Antlr.Ast.Comparables.Subjects;
using NUnit.Framework;
using Comparable = ASD_Game.Agent.Antlr.Ast.Comparable;

namespace ASD_Game.Tests.AgentTests.Ast
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class WhenTest
    {

        private When _sut;
        private const string TYPE = "When";

        [SetUp]
        public void Setup()
        {
            _sut = new When();
        }

        [Test]
        public void Test_GetNodeType_CorrectOutput()
        {
            //Arrange

            //Act
            var result = _sut.GetNodeType();
            //Assert
            Assert.AreEqual(result, TYPE);
        }

        [Test]
        [TestCase(typeof(Current))]
        [TestCase(typeof(Inventory))]
        [TestCase(typeof(NPC))]
        [TestCase(typeof(AgentSubject))]
        [TestCase(typeof(Player))]
        [TestCase(typeof(Tile))]
        [TestCase(typeof(Int))]
        [TestCase(typeof(Item))]
        [TestCase(typeof(Stat))]
        [TestCase(typeof(Subject))]
        [TestCase(typeof(Comparable))]
        public void Test_AddChild_AllComparablesComparableL(Type testCase)
        {
            //Arrange
            var comparable = ComparableTestCase(testCase);

            //Act
            _sut.AddChild(comparable);
            //Assert
            
            Assert.AreEqual( comparable, _sut.ComparableL);
            Assert.AreEqual(null,_sut.ComparableR);
            Assert.AreEqual(null,_sut.Comparison);
            Assert.AreEqual(null,_sut.Then);
            Assert.AreEqual(1, _sut.GetChildren().Count);
        }


        [Test]
        [TestCase(typeof(Current))]
        [TestCase(typeof(Inventory))]
        [TestCase(typeof(NPC))]
        [TestCase(typeof(AgentSubject))]
        [TestCase(typeof(Player))]
        [TestCase(typeof(Tile))]
        [TestCase(typeof(Int))]
        [TestCase(typeof(Item))]
        [TestCase(typeof(Stat))]
        [TestCase(typeof(Subject))]
        [TestCase(typeof(Comparable))]
        public void Test_AddChild_AllComparablesComparableR(Type testCase)
        {
            //Arrange
            var comparableL = new Comparable();
            var comparableR = ComparableTestCase(testCase);
            _sut.AddChild(comparableL);
            //Act
            _sut.AddChild(comparableR);
            //Assert
            
            Assert.AreEqual(comparableL, _sut.ComparableL);
            Assert.AreEqual(comparableR,_sut.ComparableR);
            Assert.AreEqual(null,_sut.Comparison);
            Assert.AreEqual(null,_sut.Then);
            Assert.AreEqual(2, _sut.GetChildren().Count);
        }

        [Test]
        [TestCase(typeof(Current))]
        [TestCase(typeof(Inventory))]
        [TestCase(typeof(NPC))]
        [TestCase(typeof(AgentSubject))]
        [TestCase(typeof(Player))]
        [TestCase(typeof(Tile))]
        [TestCase(typeof(Int))]
        [TestCase(typeof(Item))]
        [TestCase(typeof(Stat))]
        [TestCase(typeof(Subject))]
        [TestCase(typeof(Comparable))]
        public void Test_AddChild_AllComparablesComparableExtra(Type testCase)
        {
            //Arrange
            var comparableL = new Comparable();
            var comparableR = new Comparable();
            var comparable = ComparableTestCase(testCase);
            _sut.AddChild(comparableL);
            _sut.AddChild(comparableR);
            //Act
            _sut.AddChild(comparable);
            //Assert
            
            Assert.AreEqual(comparableL, _sut.ComparableL);
            Assert.AreEqual(comparableR,_sut.ComparableR);
            Assert.AreEqual(null,_sut.Comparison);
            Assert.AreEqual(null,_sut.Then);
            Assert.AreEqual(3, _sut.GetChildren().Count);
            Assert.AreEqual(comparable, _sut.GetChildren()[2]);
        }

        [Test]
        public void Test_AddChild_ActionReference()
        {
            //Arrange
            var action = new ActionReference("test");
            //Act
            _sut.AddChild(action);
            //Assert
            Assert.AreEqual(null, _sut.ComparableL);
            Assert.AreEqual(null,_sut.ComparableR);
            Assert.AreEqual(null,_sut.Comparison);
            Assert.AreEqual(action,_sut.Then);
            Assert.AreEqual(1, _sut.GetChildren().Count);
        }


        [Test]
        public void Test_Child_Comparison()
        {
            //Arrange
            var comparison = new Comparison("test");
            //Act
            _sut.AddChild(comparison);
            //Assert
            Assert.AreEqual(null, _sut.ComparableL);
            Assert.AreEqual(null,_sut.ComparableR);
            Assert.AreEqual(comparison,_sut.Comparison);
            Assert.AreEqual(null,_sut.Then);
            
            Assert.AreEqual(1, _sut.GetChildren().Count);
        }



        [Test]
        public void Test_AddChild_ExtraNode()
        {
            //Arrange
            var extra = new Node();
            //Act
            _sut.AddChild(extra);
            //Assert
            Assert.AreEqual(null, _sut.ComparableL);
            Assert.AreEqual(null,_sut.ComparableR);
            Assert.AreEqual(null,_sut.Comparison);
            Assert.AreEqual(null,_sut.Then);
            Assert.AreEqual(1, _sut.GetChildren().Count);
            Assert.AreEqual(extra, _sut.GetChildren()[0]);
        }

        private static Comparable ComparableTestCase(Type testCase)
        {
            Comparable comparable;
            if (testCase == typeof(Int))
            {
                comparable = (Comparable)Activator.CreateInstance(testCase, 1);
            }
            else if (testCase == typeof(Comparable))
            {
                comparable = (Comparable)Activator.CreateInstance(testCase);
            }
            else
            {
                comparable = (Comparable)Activator.CreateInstance(testCase, "test");
            }

            return comparable;
        }





    }
}