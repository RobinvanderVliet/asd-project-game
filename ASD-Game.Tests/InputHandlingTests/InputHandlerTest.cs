using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ASD_Game.InputHandling;
using ASD_Game.InputHandling.Antlr;
using ASD_Game.InputHandling.Antlr.Transformer;
using ASD_Game.InputHandling.Models;
using ASD_Game.Messages;
using ASD_Game.Session;
using ASD_Game.Session.GameConfiguration;
using ASD_Game.UserInterface;
using Moq;
using NUnit.Framework;

namespace ASD_Game.Tests.InputHandlingTests
{
    [ExcludeFromCodeCoverage]
    public class InputHandlerTest
    {
        private IInputHandler _sut;
        private Mock<Pipeline> _mockedPipeline;
        private Mock<ScreenHandler> _mockedScreenHandler;
        private Mock<IScreenHandler> _mockedInterfaceScreenHandler;
        private Mock<ISessionHandler> _mockedSessionHandler;
        private Mock<StartScreen> _mockedStartScreen;
        private Mock<SessionScreen> _mockedSessionScreen;
        private Mock<IGameConfigurationHandler> _mockedGameConfigurationHandler;
        public Mock<MessageService> mockedMessagesService;
        private Mock<ConsoleHelper> _mockedConsole;

        [SetUp]
        public void SetUp()
        {
            _mockedPipeline = new Mock<Pipeline>();
            _mockedPipeline.Object.Evaluator = new Mock<IEvaluator>().Object;

            _mockedInterfaceScreenHandler = new Mock<IScreenHandler>();
            _mockedScreenHandler = new Mock<ScreenHandler>();
            _mockedSessionHandler = new Mock<ISessionHandler>();
            _mockedStartScreen = new Mock<StartScreen>();
            _mockedSessionScreen = new Mock<SessionScreen>();
            _mockedGameConfigurationHandler = new Mock<IGameConfigurationHandler>();
            _mockedConsole = new();
            _mockedScreenHandler.Object.ConsoleHelper = _mockedConsole.Object;
            mockedMessagesService = new(_mockedScreenHandler.Object);
            _sut = new InputHandler(_mockedPipeline.Object, _mockedSessionHandler.Object, _mockedScreenHandler.Object, mockedMessagesService.Object, _mockedGameConfigurationHandler.Object);
        }

        [Test]
        public void Test_HandleGameScreenCommands_SendsCommand()
        {
            //Arrange
            var command = "move forward";
            _mockedScreenHandler.Setup(mock => mock.GetScreenInput()).Returns(command);
            //Act
            _sut.HandleGameScreenCommands();
            //Assert
            _mockedPipeline.Verify(mock => mock.ParseCommand(command));
        }

        [Test]
        [TestCase("1")]
        [TestCase("2")]
        [TestCase("3")]
        [TestCase("4")]
        [TestCase("5")]
        [TestCase("Error")]
        public void Test_HandleStartScreenCommands_HandlesInput(string input)
        {
            //Arrange
            _sut = new InputHandler(_mockedPipeline.Object, _mockedSessionHandler.Object, _mockedInterfaceScreenHandler.Object, mockedMessagesService.Object, _mockedGameConfigurationHandler.Object);

            _mockedInterfaceScreenHandler.Setup(mock => mock.GetScreenInput()).Returns(input);
            _mockedInterfaceScreenHandler.Setup(mock => mock.Screen).Returns(_mockedStartScreen.Object);
            _mockedStartScreen.Setup(mock => mock.UpdateInputMessage(It.IsAny<string>()));

            //Act
            _sut.HandleStartScreenCommands();

            //Assert
            if (input.Equals("1"))
            {
                _mockedInterfaceScreenHandler.Verify(mock => mock.TransitionTo(It.IsAny<ConfigurationScreen>()));
            }
            else if (input.Equals("2"))
            {
                _mockedInterfaceScreenHandler.Verify(mock => mock.TransitionTo(It.IsAny<SessionScreen>()));
            }
            else if (input.Equals("3"))
            {
                _mockedInterfaceScreenHandler.Verify(mock => mock.TransitionTo(It.IsAny<LoadScreen>()));
            }
            else if (input.Equals("4"))
            {
                _mockedInterfaceScreenHandler.Verify(mock => mock.TransitionTo(It.IsAny<EditorScreen>()));
            }
            else if (input.Equals("5"))
            {
                _mockedPipeline.Verify(mock => mock.ParseCommand("exit"));
            }
            else
            {
                _mockedStartScreen.Verify(mock => mock.UpdateInputMessage("Not a valid option, try again!"), Times.Once);
            }
        }

        [TestCase("1 Gerrit")]
        [TestCase("2 Gerrit")]
        [TestCase("2")]
        [TestCase("return")]
        public void Test_HandleSessionScreenCommands_HandlesInput(string input)
        {
            //Arrange
            _sut = new InputHandler(_mockedPipeline.Object, _mockedSessionHandler.Object, _mockedInterfaceScreenHandler.Object, mockedMessagesService.Object, _mockedGameConfigurationHandler.Object);
            
            var testId = "testId";
            var username = "Gerrit";
            
            _mockedInterfaceScreenHandler.Setup(mock => mock.GetScreenInput()).Returns(input);
            _mockedInterfaceScreenHandler.Setup(mock => mock.Screen).Returns(_mockedSessionScreen.Object);
            _mockedSessionScreen.Setup(mock => mock.GetSessionIdByVisualNumber(0)).Returns(testId);
            _mockedSessionScreen.Setup(mock => mock.GetSessionIdByVisualNumber(1)).Returns("");
            _mockedSessionScreen.Setup(mock => mock.UpdateInputMessage(It.IsAny<string>()));

            //Act
            _sut.HandleSessionScreenCommands();

            //Assert
            if (input.Equals("return"))
            {
                _mockedInterfaceScreenHandler.Verify(mock => mock.TransitionTo(It.IsAny<StartScreen>()), Times.Once);
            }
            else
            {
                if (input.Equals("1 Gerrit"))
                {
                    _mockedPipeline.Verify(mock => mock.ParseCommand("join_session \"" + testId + "\" " + "\"" + username + "\""));
                }
                else if (input.Equals("2"))
                {
                    _mockedSessionScreen.Verify(mock => mock.UpdateInputMessage("Provide both a session number and username (example: 1 Gerrit)"));
                }
                else
                {
                    _mockedSessionScreen.Verify(mock => mock.UpdateInputMessage("Not a valid session, try again!"));
                }
            }
        }

        [Test]
        public void Test_BreakEditorCommands()
        {
            //Arrange
            var mockEditor = new Mock<EditorScreen>();
            _mockedScreenHandler.Object.Screen = mockEditor.Object;
            _mockedScreenHandler.Object.Screen.SetScreen(_mockedScreenHandler.Object);

            _mockedScreenHandler.SetupSequence(x => x.GetScreenInput()).Returns("hoi").
                Returns("break");


            //Act
            _sut.HandleEditorScreenCommands();


            //Assert 
            mockEditor.Verify(x => x.PrintWarning(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void Test_PrintwarningCommand()
        {
            //Arrange
            var mockEditor = new Mock<EditorScreen>();
            _mockedScreenHandler.Object.Screen = mockEditor.Object;
            _mockedScreenHandler.Object.Screen.SetScreen(_mockedScreenHandler.Object);

            _mockedScreenHandler.SetupSequence(x => x.GetScreenInput()).Returns("test input moet fout gaan").Returns("random").Returns("offensive")
                .Returns("no").Returns("no").Returns("break");

            //Act
            _sut.HandleEditorScreenCommands();

            mockEditor.Verify(x => x.PrintWarning(It.IsAny<string>()), Times.Once);
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
        public void Test_HandleHelpEditorCommands()
        {
            //Arrange
            var mockEditor = new Mock<EditorScreen>();
            _mockedScreenHandler.Object.Screen = mockEditor.Object;
            _mockedScreenHandler.Object.Screen.SetScreen(_mockedScreenHandler.Object);
            _mockedScreenHandler.SetupSequence(x => x.GetScreenInput()).Returns("help explore").Returns("random").Returns("offensive")
                .Returns("no")
                .Returns("no");

            //Act
            _sut.HandleEditorScreenCommands();


            //Assert
            _mockedConsole.Verify(x => x.ClearConsole(), Times.AtLeastOnce);
            _mockedScreenHandler.Verify(x => x.GetScreenInput(), Times.Exactly(6));
        }

        [Test]
        public void Test_Checkinput_correct_baseword()
        {
            //Arrange
            List<string> rules = new();
            BaseVariables variables = new();
            rules.Add("when");
            rules.Add("player");
            rules.Add("nearby");
            rules.Add("player");
            rules.Add("then");
            rules.Add("attack");

            //Act & Assert
            Assert.True(_sut.CheckInput(rules, variables));
        }

        [Test]
        public void Test_Checkinput_incorrect_baseword()
        {
            //Arrange
            List<string> rules = new();
            BaseVariables variables = new();
            rules.Add("when");
            rules.Add("player");
            rules.Add("nearby");
            rules.Add("player");
            rules.Add("dump");
            rules.Add("attack");

            //Act & Assert
            Assert.False(_sut.CheckInput(rules, variables));
        }

        [Test]
        [TestCase("armor")]
        [TestCase("weapon")]
        [TestCase("comparison")]
        [TestCase("consumables")]
        [TestCase("actions")]
        [TestCase("bitcoinitems")]
        [TestCase("comparables")]
        public void Test_CustomRuleHandleEditorScreenCommands_Help(string resource)
        {
            //Arrange
            var mockEditor = new Mock<EditorScreen>();
            _mockedScreenHandler.Object.Screen = mockEditor.Object;
            _mockedScreenHandler.SetupSequence(x => x.GetScreenInput()).Returns("").Returns("Help " + resource)
                .Returns("When player nearby agent then attack").Returns("no");

            //Act
            var result = _sut.CustomRuleHandleEditorScreenCommands("combat");


            //Assert
            Assert.True(result.Length > 0);
            mockEditor.Verify(x => x.UpdateLastQuestion(It.IsAny<string>()), Times.AtLeastOnce);
            mockEditor.Verify(x => x.ClearScreen(), Times.AtLeastOnce);
            _mockedScreenHandler.Verify(x => x.GetScreenInput(), Times.Exactly(4));

        }
    }
}
