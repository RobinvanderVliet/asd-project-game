using System.Diagnostics.CodeAnalysis;
using ASD_Game.Agent.Antlr.Ast.Comparables;
using ASD_Game.Agent.Antlr.Ast.Comparables.Subjects;
using NUnit.Framework;

namespace ASD_Game.Tests.AgentTests.Ast
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class SubjectTest
    {

        private const string TESTNAME = "test";

        [Test]
        public void Test_GetNodeTypeSubject_CorrectOutput()
        {
            //Arrange
            var node = new Subject(TESTNAME);
            //Act
            var result = node.GetNodeType();
            //Assert
            Assert.AreEqual("Subject", result);
        }

        [Test]
        public void Test_GetNodeTypeCurrent_CorrectOutput()
        {
            //Arrange
            var node = new Current(TESTNAME);
            //Act
            var result = node.GetNodeType();
            //Assert
            Assert.AreEqual("Current", result);
        }

        [Test]
        public void Test_GetNodeTypeInventory_CorrectOutput()
        {
            //Arrange
            var node = new Inventory(TESTNAME);
            //Act
            var result = node.GetNodeType();
            //Assert
            Assert.AreEqual("Inventory", result);
        }

        [Test]
        public void Test_GetNodeNPC_CorrectOutput()
        {
            //Arrange
            var node = new NPC(TESTNAME);
            //Act
            var result = node.GetNodeType();
            //Assert
            Assert.AreEqual("NPC", result);
        }

        [Test]
        public void Test_GetNodeOpponent_CorrectOutput()
        {
            //Arrange
            var node = new AgentSubject(TESTNAME);
            //Act
            var result = node.GetNodeType();
            //Assert
            Assert.AreEqual("Agent", result);
        }

        [Test]
        public void Test_GetNodePlayer_CorrectOutput()
        {
            //Arrange
            var node = new Player(TESTNAME);
            //Act
            var result = node.GetNodeType();
            //Assert
            Assert.AreEqual("Player", result);
        }

        [Test]
        public void Test_GetNodeTile_CorrectOutput()
        {
            //Arrange
            var node = new Tile(TESTNAME);
            //Act
            var result = node.GetNodeType();
            //Assert
            Assert.AreEqual("Tile", result);
        }

    }
}