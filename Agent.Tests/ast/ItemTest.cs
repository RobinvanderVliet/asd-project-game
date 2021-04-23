using Agent.antlr.ast;
using Agent.antlr.ast.comparables;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace Agent.Tests.ast
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class ItemTest
    {
        private Item _sut;
        private const string Type = "Item";
        
        [SetUp]
        public void Setup()
        {
            this._sut = new Item(Type);
        }
        
        /*
         * GetNodeType()
         *
         * Test of de juiste type terug gegeven wordt
         */
        [Test]
        public void Test_GetNodeType_CorrectOutput()
        {
            //Arrange
            
            //Act
            var result = this._sut.GetNodeType();
            //Assert
            Assert.AreEqual(Type, result);
        }
        /*
         * AddSubjectToChild()
         *
         * Test if the stat is added to item
        */
        [Test]
        public void Test_AddChild_AddItem()
        {
            //Arrange
            var stat = new Stat("");
            this._sut.AddChild(stat);

            //Act
            var result = ( this._sut.GetChildren()[0]).GetNodeType();

            //Assert
            Assert.AreEqual("Stat", result);
        }
        
        /*
         * AddItemToChild()
         *
         * Test if the node is added to item
        */
        [Test]
        public void Test_AddChild_AddNoe()
        {
            //Arrange
            var node = new Node();
            this._sut.AddChild(node);

            //Act
            var result = ( this._sut.GetChildren()[0])?.GetNodeType();

            //Assert
            Assert.AreEqual("Node", result);
        }
       
    }
}