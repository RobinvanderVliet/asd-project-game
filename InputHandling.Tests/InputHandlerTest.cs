using System.Diagnostics.CodeAnalysis;
using InputHandling.Antlr;
using System;
using System.IO;
using System.Text;
using InputHandling.Antlr.Transformer;
using Moq;
using NUnit.Framework;
using Session;
using UserInterface;

namespace InputHandling.Tests
{
    [ExcludeFromCodeCoverage]
    public class InputHandlerTest
    {
        private IInputHandler _sut;
        private Mock<Pipeline> _mockedPipeline;
        private Mock<ScreenHandler> _mockedScreenHandler;
        private Mock<ISessionHandler> _mockedSessionHandler;
        private Mock<StartScreen> _mockedStartScreen;

        [SetUp]
        public void SetUp()
        {
            _mockedPipeline = new Mock<Pipeline>();
            _mockedPipeline.Object.Evaluator = new Mock<IEvaluator>().Object;
            _mockedScreenHandler = new Mock<ScreenHandler>();
            _mockedSessionHandler = new Mock<ISessionHandler>();
            _mockedStartScreen = new Mock<StartScreen>();
            _mockedScreenHandler.Object.ConsoleHelper = new Mock<ConsoleHelper>().Object;

            _sut = new InputHandler(_mockedPipeline.Object, _mockedSessionHandler.Object, _mockedScreenHandler.Object);
        }

        [Test]
        [TestCase("1")]
        [TestCase("2")]
        [TestCase("3")]
        [TestCase("4")]
        [TestCase("5")]
        [TestCase("Error")]
        public void Test_HandleStartScreenCommands_Calls(string input)
        {
            //Arrange
            _mockedScreenHandler.Setup(mock => mock.GetScreenInput()).Returns(input);
            _mockedScreenHandler.Setup(mock => mock.Screen).Returns(_mockedStartScreen.Object);
            _mockedStartScreen.Setup(mock => mock.UpdateInputMessage(It.IsAny<string>()));

            //Act
            _sut.HandleStartScreenCommands();

            //Assert
            if (input.Equals("1"))
            {
                _mockedScreenHandler.Verify(mock => mock.TransitionTo(It.IsAny<ConfigurationScreen>()));
            }
            else if (input.Equals("2"))
            {
                _mockedScreenHandler.Verify(mock => mock.TransitionTo(It.IsAny<SessionScreen>()));
            }
            else if (input.Equals("3"))
            {
                _mockedScreenHandler.Verify(mock => mock.TransitionTo(It.IsAny<LoadScreen>()));
            }
            else if (input.Equals("4"))
            {
                _mockedScreenHandler.Verify(mock => mock.TransitionTo(It.IsAny<EditorScreen>()));
            }
            else if (input.Equals("5"))
            {
                _mockedPipeline.Verify(mock => mock.ParseCommand("exit"));
            }
            else
            {
                _mockedStartScreen.Verify(mock => mock.UpdateInputMessage("Not a valid option, try again!"),
                    Times.Once);
            }
        }


        [Test]
        public void Test_HandleStopCommandEditorScreen()
        {
            //Arrange
            var mockEditor = new Mock<EditorScreen>();
            _mockedScreenHandler.Object.Screen = mockEditor.Object;
            _mockedScreenHandler.SetupSequence(x => x.GetScreenInput()).Returns("").Returns("Stop");

            string result;

            //Act
            result = _sut.CustomRuleHandleEditorScreenCommands("combat");


            //Assert
            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void Test_HandleEditorCommands()
        {
            //Arrange
            var mockEditor = new Mock<EditorScreen>();
            _mockedScreenHandler.Object.Screen = mockEditor.Object;
            _mockedScreenHandler.Object.Screen.SetScreen(_mockedScreenHandler.Object);

            _mockedScreenHandler.SetupSequence(x => x.GetScreenInput()).Returns("random").Returns("offensive")
                .Returns("no")
                .Returns("no");

            //Act
            _sut.HandleEditorScreenCommands();


            //Assert TODO: METHOD SHOULD RETURN SOMETHING AND THAT NEEDS TO BE TESTED
            Assert.AreEqual(true, true);
        }
        [Test]
        public void Test_PrintWarningEditorCommands()
        {
            //Arrange
            var mockEditor = new Mock<EditorScreen>();
            _mockedScreenHandler.Object.Screen = mockEditor.Object;
            _mockedScreenHandler.Object.Screen.SetScreen(_mockedScreenHandler.Object);

            _mockedScreenHandler.SetupSequence(x => x.GetScreenInput()).Returns("blah").Returns("offensive")
                .Returns("no")
                .Returns("no");

            //Act
            _sut.HandleEditorScreenCommands();


            //Assert TODO: METHOD SHOULD RETURN SOMETHING AND THAT NEEDS TO BE TESTED
            Assert.AreEqual(true, true);
        }
        
    }
}