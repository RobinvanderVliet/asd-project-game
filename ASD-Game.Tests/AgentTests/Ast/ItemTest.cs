using System.Diagnostics.CodeAnalysis;
using ASD_Game.Agent.Antlr.Ast;
using ASD_Game.Agent.Antlr.Ast.Comparables;
using NUnit.Framework;

namespace ASD_Game.Tests.AgentTests.Ast
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class ItemTest
    {
        private Item _sut;
        private const string TYPE = "Item";

        [SetUp]
        public void Setup()
        {
            _sut = new Item(TYPE);
        }


        [Test]
        public void Test_GetNodeType_CorrectOutput()
        {
            //Arrange

            //Act
            var result = _sut.GetNodeType();
            //Assert
            Assert.AreEqual(TYPE, result);
        }

        [Test]
        public void Test_AddChild_AddItem()
        {
            //Arrange
            var stat = new Stat("");
            _sut.AddChild(stat);

            //Act
            var result = (_sut.GetChildren()[0]).GetNodeType();

            //Assert
            Assert.AreEqual("Stat", result);
        }


        [Test]
        public void Test_AddChild_AddNoe()
        {
            //Arrange
            var node = new Node();
            _sut.AddChild(node);

            //Act
            var result = (_sut.GetChildren()[0])?.GetNodeType();

            //Assert
            Assert.AreEqual("Node", result);
        }

    }
}