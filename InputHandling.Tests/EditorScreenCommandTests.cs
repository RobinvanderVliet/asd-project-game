using System;
using System.IO;
using System.Text;
using ActionHandling;
using Chat;
using InputHandling.Antlr;
using InputHandling.Antlr.Ast;
using InputHandling.Antlr.Transformer;
using Moq;
using NUnit.Framework;
using Session;
using UserInterface;

namespace InputHandling.Tests
{
    public class EditorScreenCommandTests
    {
        private InputHandler _sut;
        private Mock<IPipeline> _mockedPipeline;
        private Mock<ISessionHandler> _mockedSessionHandler;
        private Mock<ScreenHandler> _mockedScreenHandler;

        [SetUp]
        public void Setup()
        {
            _mockedPipeline = new Mock<IPipeline>();
            _mockedSessionHandler = new Mock<ISessionHandler>();
            _mockedScreenHandler = new Mock<ScreenHandler>();
            
            _sut = new InputHandler(_mockedPipeline.Object, _mockedSessionHandler.Object, _mockedScreenHandler.Object);
        }

        [Test]
        public void Test_HandleStopCommandEditorScreen()
        {
            //Arrange

            var mockEditor = new Mock<EditorScreen>();
            _mockedScreenHandler.Object.Screen = mockEditor.Object;
            
            var input = new StringBuilder();
            //vul hier je input in, per readline een nieuwe appendline
            input.AppendLine("").AppendLine("hallo");

            //Act
            using (TextReader txt = new StringReader(input.ToString()))
            {
                Console.SetIn(txt);
                _sut.CustomRuleHandleEditorScreenCommands("combat");
            }
            
            
            //Assert
            Assert.AreEqual("stop", Console.ReadLine());
            
        }
    }
}