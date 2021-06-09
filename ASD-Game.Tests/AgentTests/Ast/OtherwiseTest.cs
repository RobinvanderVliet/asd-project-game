using System.Diagnostics.CodeAnalysis;
using ASD_Game.Agent.Antlr.Ast;
using NUnit.Framework;

namespace ASD_Game.Tests.AgentTests.Ast
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class OtherwiseTest
    {
        private Otherwise _otherwise;
        private const string TYPE = "Otherwise";

        [SetUp]
        public void Setup()
        {
            _otherwise = new Otherwise();
        }



        [Test]
        public void Test_GetNodeType_CorrectOutput()
        {
            //Arrange
            //Act
            var result = _otherwise.GetNodeType();
            //Assert
            Assert.AreEqual(result, TYPE);
        }


        [Test]
        public void Test_AddChild_ActionReference()
        {
            //Arrange
            ActionReference actionReference = new ActionReference("action Reference");
            _otherwise.AddChild(actionReference);

            //Act

            var result = (_otherwise.GetChildren()[0]).GetNodeType();

            //Assert
            Assert.AreEqual(result, "ActionReference");
        }


        [Test]
        public void Test_AddChild_Node()
        {
            //Arrange
            var node = new Node();
            _otherwise.AddChild(node);

            //Act

            var result = (_otherwise.GetChildren()[0]).GetNodeType();

            //Assert
            Assert.AreEqual("Node", result);
        }
    }
}