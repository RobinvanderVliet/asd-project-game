using System.Diagnostics.CodeAnalysis;
using ASD_Game.Agent.Antlr.Ast;
using ASD_Game.Agent.Antlr.Ast.Comparables;
using NUnit.Framework;

namespace ASD_Game.Tests.AgentTests.Ast
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class ActionReferenceTest
    {
        private ActionReference _actionReference;
        private const string TYPE = "ActionReference";

        [SetUp]
        public void Setup()
        {
            _actionReference = new ActionReference("");
        }


        [Test]
        public void Test_GetNodeType_CorrectOutput()
        {
            //Arrange
            //Act
            var result = _actionReference.GetNodeType();
            //Assert
            Assert.AreEqual(TYPE, result);
        }


        [Test]
        public void Test_AddChild_AddSubjectChild()
        {
            //Arrange
            Subject subject = new Subject("");
            _actionReference.AddChild(subject);

            //Act
            var result = ((Subject)_actionReference.GetChildren()[0])?.GetNodeType();

            //Assert
            Assert.AreEqual("Subject", result);
        }


        [Test]
        public void Test_AddChild_AddItemChild()
        {
            //Arrange
            var item = new Item("");
            _actionReference.AddChild(item);

            //Act
            var result = ((Item)_actionReference.GetChildren()[0])?.GetNodeType();

            //Assert
            Assert.AreEqual("Item", result);
        }


        [Test]
        public void Test_AddChild_AddNodeChild()
        {
            //Arrange
            var node = new Node();
            _actionReference.AddChild(node);

            //Act
            var result = (_actionReference.GetChildren()[0])?.GetNodeType();

            //Assert
            Assert.AreEqual("Node", result);
        }
    }
}