using System;
using System.Diagnostics.CodeAnalysis;
using Agent.Antlr.Ast;
using Agent.Antlr.Ast.Comparables;
using Agent.Antlr.Ast.Comparables.Subjects;
using NUnit.Framework;
using Comparable = Agent.Antlr.Ast.Comparable;

namespace Agent.Tests.Ast
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
            this._sut = new When();
        }
        
        [Test]
        public void Test_GetNodeType_CorrectOutput()
        {
            //Arrange
            
            //Act
            var result = this._sut.GetNodeType();
            //Assert
            Assert.AreEqual(result, TYPE);
        }
        
        [Test]
        [TestCase(typeof(Current))]
        [TestCase(typeof(Inventory))]
        [TestCase(typeof(NPC))]
        [TestCase(typeof(Opponent))]
        [TestCase(typeof(Player))]
        [TestCase(typeof(Tile))]
        [TestCase(typeof(Int))]
        [TestCase(typeof(Item))]
        [TestCase(typeof(Stat))]
        [TestCase(typeof(Subject))]
        [TestCase(typeof(Comparable))]
        public void Test_AddChild_AllComparablesComparableL( Type testCase)
        {
            //Arrange
            var comparable = ComparableTestCase(testCase);
            
            //Act
            this._sut.AddChild( comparable);
            //Assert
            
            Assert.AreEqual( comparable, this._sut.GetComparableL());
            Assert.AreEqual(null,this._sut.GetComparableR());
            Assert.AreEqual(null,this._sut.GetComparison());
            Assert.AreEqual(null,this._sut.GetThen());
            Assert.AreEqual(1, this._sut.GetChildren().Count);
        }
        
     
        [Test]
        [TestCase(typeof(Current))]
        [TestCase(typeof(Inventory))]
        [TestCase(typeof(NPC))]
        [TestCase(typeof(Opponent))]
        [TestCase(typeof(Player))]
        [TestCase(typeof(Tile))]
        [TestCase(typeof(Int))]
        [TestCase(typeof(Item))]
        [TestCase(typeof(Stat))]
        [TestCase(typeof(Subject))]
        [TestCase(typeof(Comparable))]
        public void Test_AddChild_AllComparablesComparableR( Type testCase)
        {
            //Arrange
            var comparableL = new Comparable();
            var comparableR = ComparableTestCase(testCase);
            this._sut.AddChild(comparableL);
            //Act
            this._sut.AddChild(comparableR);
            //Assert
            
            Assert.AreEqual(comparableL, this._sut.GetComparableL());
            Assert.AreEqual(comparableR,this._sut.GetComparableR());
            Assert.AreEqual(null,this._sut.GetComparison());
            Assert.AreEqual(null,this._sut.GetThen());
            Assert.AreEqual(2, this._sut.GetChildren().Count);
        }
      
        [Test]
        [TestCase(typeof(Current))]
        [TestCase(typeof(Inventory))]
        [TestCase(typeof(NPC))]
        [TestCase(typeof(Opponent))]
        [TestCase(typeof(Player))]
        [TestCase(typeof(Tile))]
        [TestCase(typeof(Int))]
        [TestCase(typeof(Item))]
        [TestCase(typeof(Stat))]
        [TestCase(typeof(Subject))]
        [TestCase(typeof(Comparable))]
        public void Test_AddChild_AllComparablesComparableExtra( Type testCase)
        {
            //Arrange
            var comparableL = new Comparable();
            var comparableR = new Comparable();
            var comparable = ComparableTestCase(testCase);
            this._sut.AddChild(comparableL);
            this._sut.AddChild(comparableR);
            //Act
            this._sut.AddChild(comparable);
            //Assert
            
            Assert.AreEqual(comparableL, this._sut.GetComparableL());
            Assert.AreEqual(comparableR,this._sut.GetComparableR());
            Assert.AreEqual(null,this._sut.GetComparison());
            Assert.AreEqual(null,this._sut.GetThen());
            Assert.AreEqual(3, this._sut.GetChildren().Count);
            Assert.AreEqual(comparable, this._sut.GetChildren()[2]);
        }
        
        [Test]
        public void Test_AddChild_ActionReference()
        {
            //Arrange
            var action = new ActionReference("test");
            //Act
            this._sut.AddChild(action);
            //Assert
            Assert.AreEqual(null, this._sut.GetComparableL());
            Assert.AreEqual(null,this._sut.GetComparableR());
            Assert.AreEqual(null,this._sut.GetComparison());
            Assert.AreEqual(action,this._sut.GetThen());
            Assert.AreEqual(1, this._sut.GetChildren().Count);
        }
        
     
        [Test]
        public void Test_Child_Comparison()
        {
            //Arrange
            var comparison = new Comparison("test");
            //Act
            this._sut.AddChild(comparison);
            //Assert
            Assert.AreEqual(null, this._sut.GetComparableL());
            Assert.AreEqual(null,this._sut.GetComparableR());
            Assert.AreEqual(comparison,this._sut.GetComparison());
            Assert.AreEqual(null,this._sut.GetThen());
            
            Assert.AreEqual(1, this._sut.GetChildren().Count);
        }
        
                
    
        [Test]
        public void Test_AddChild_ExtraNode()
        {
            //Arrange
            var extra = new Node();
            //Act
            this._sut.AddChild(extra);
            //Assert
            Assert.AreEqual(null, this._sut.GetComparableL());
            Assert.AreEqual(null,this._sut.GetComparableR());
            Assert.AreEqual(null,this._sut.GetComparison());
            Assert.AreEqual(null,this._sut.GetThen());
            Assert.AreEqual(1, this._sut.GetChildren().Count);
            Assert.AreEqual(extra, this._sut.GetChildren()[0]);
        }
        
        private static Comparable ComparableTestCase(Type testCase)
        {
            Comparable comparable;
            if (testCase == typeof(Int)){
                comparable = (Comparable) Activator.CreateInstance(testCase,1);
            }
            else if (testCase == typeof(Comparable)){
                comparable = (Comparable) Activator.CreateInstance(testCase);
            }
            else {
                comparable = (Comparable) Activator.CreateInstance(testCase, "test");
            }

            return comparable;
        }
        

        
        
        
    }
}