using System;
using System.IO;
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
            EditorScreen editorScreen = new EditorScreen();
            StringReader textReaderContinue = new StringReader("");
            StringReader textReaderActualTest = new StringReader("stop");
            StringReader textReader3 = new StringReader("");
            
            
            //editorScreen.UpdateLastQuestion()
            var mockEditor = new Mock<EditorScreen>().Object;
            _mockedScreenHandler.Object.Screen = mockEditor;
            Console.SetIn(textReaderContinue);
            Console.SetIn(textReaderActualTest);
            Console.SetIn(textReader3);

            //Act
            _sut.customRuleHandleEditorScreenCommands("combat");
            
            
            //Assert
            Assert.AreEqual("stop", Console.ReadLine());
            
        }
    }
}