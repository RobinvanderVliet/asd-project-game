using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ASD_Game.Agent.Antlr.Ast;
using NUnit.Framework;

namespace ASD_Game.Tests.AgentTests.Ast
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class NodeTest
    {
        private Node _sut;
        private const string TYPE = "Node";

        [SetUp]
        public void Setup()
        {
            _sut = new Node();
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
        public void Test_GetChildren()
        {
            //Arrange

            //Act
            var result = _sut.GetChildren();
            //Assert
            Assert.IsInstanceOf(typeof(List<Node>), result);
        }

        [Test]
        public void Test_AddChild()
        {
            //Arrange
            var node = new Node();
            _sut.AddChild(node);
            //Act
            var result = (_sut.GetChildren()[0])?.GetNodeType();

            //Assert
            Assert.AreEqual("Node", result);
            Assert.AreEqual(1, _sut.GetChildren().Count);
        }

        [Test]
        public void Test_RemoveChild()
        {
            //Arrange
            var node = new Node();
            _sut.AddChild(node);
            //Act
            _sut.RemoveChild(node);

            //Assert
            Assert.AreEqual(0, _sut.GetChildren().Count);
        }


        [Test]
        public void Test_GetErrorAfterSetError()
        {
            //Arrange
            var message = "testtt";
            _sut.SetError(message);
            //Act
            var result = _sut.GetError();

            //Assert
            Assert.IsInstanceOf(typeof(ASTError), result);
            Assert.AreEqual((new ASTError(message)).ToString(), result.ToString());
        }


        [Test]
        public void Test_ToString()
        {
            //Arrange
            var expected = "[Node][Node][Node]";
            _sut.AddChild(new Node());
            _sut.AddChild(new Node());
            //Act
            var result = _sut.ToString();

            //Assert
            Assert.AreEqual(expected, result);
        }
    }
}