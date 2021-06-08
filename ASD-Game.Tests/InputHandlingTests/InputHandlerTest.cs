using System.Diagnostics.CodeAnalysis;
using InputHandling.Antlr;
using InputHandling.Antlr.Transformer;
using Moq;
using NUnit.Framework;
using Session;
using Session.GameConfiguration;
using UserInterface;
using Messages;

namespace InputHandling.Tests
{
    [ExcludeFromCodeCoverage]
    public class InputHandlerTest
    {
        private IInputHandler _sut;
        private Mock<Pipeline> _mockedPipeline;
        private Mock<IScreenHandler> _mockedScreenHandler;
        private Mock<ISessionHandler> _mockedSessionHandler;
        private Mock<StartScreen> _mockedStartScreen;
        private Mock<SessionScreen> _mockedSessionScreen;
        private Mock<IGameConfigurationHandler> _mockedGameConfigurationHandler;
        private Mock<IMessageService> _mockedMessageService;
        private Mock<IGamesSessionService> _mockedGamesSessionService;
        
        [SetUp]
        public void SetUp()
        {
            _mockedPipeline = new Mock<Pipeline>();
            _mockedPipeline.Object.Evaluator = new Mock<IEvaluator>().Object;
            _mockedScreenHandler = new Mock<IScreenHandler>();
            _mockedSessionHandler = new Mock<ISessionHandler>();
            _mockedStartScreen = new Mock<StartScreen>();
            _mockedSessionScreen = new Mock<SessionScreen>();
            _mockedGameConfigurationHandler = new Mock<IGameConfigurationHandler>();
            _mockedScreenHandler.Object.ConsoleHelper = new Mock<ConsoleHelper>().Object;
            _mockedMessageService = new Mock<IMessageService>();
            _mockedGamesSessionService = new Mock<IGamesSessionService>();

            _sut = new InputHandler(_mockedPipeline.Object, _mockedSessionHandler.Object, _mockedScreenHandler.Object, _mockedMessageService.Object, _mockedGameConfigurationHandler.Object, _mockedGamesSessionService.Object);
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
            var testId = "testId";
            var username = "Gerrit";
            _mockedScreenHandler.Setup(mock => mock.GetScreenInput()).Returns(input);
            _mockedScreenHandler.Setup(mock => mock.Screen).Returns(_mockedSessionScreen.Object);
            _mockedSessionScreen.Setup(mock => mock.GetSessionIdByVisualNumber(0)).Returns(testId);
            _mockedSessionScreen.Setup(mock => mock.GetSessionIdByVisualNumber(1)).Returns("");
            _mockedSessionScreen.Setup(mock => mock.UpdateInputMessage(It.IsAny<string>()));
            
            //Act
            _sut.HandleSessionScreenCommands();
            
            //Assert
            if (input.Equals("return"))
            {
                _mockedScreenHandler.Verify(mock => mock.TransitionTo(It.IsAny<StartScreen>()), Times.Once);
            }
            else
            {
                if (input.Equals("1 Gerrit"))
                {
                    _mockedPipeline.Verify(mock => mock.ParseCommand("join_session \""+testId+"\" " + "\"" + username + "\""));
                }
                else if (input.Equals("2"))
                {
                    _mockedSessionScreen.Verify(mock=> mock.UpdateInputMessage("Provide both a session number and username (example: 1 Gerrit)"));
                }
                else
                {
                    _mockedSessionScreen.Verify(mock=> mock.UpdateInputMessage("Not a valid session, try again!"));
                }
            }
        }
        
        [TestCase("1")]
        [TestCase("")]
        [TestCase("error")]
        [TestCase("return")]
        public void Test_HandleLoadScreenCommands_HandlesInput(string input)
        {
            //Arrange
            var testId = "testId";
            _mockedScreenHandler.Setup(mock => mock.GetScreenInput()).Returns(input);
            _mockedScreenHandler.Setup(mock => mock.GetSessionByPosition(0)).Returns(testId);
            _mockedScreenHandler.Setup(mock => mock.GetSessionByPosition(-1)).Returns("");
            _mockedSessionScreen.Setup(mock => mock.UpdateInputMessage(It.IsAny<string>()));
            
            //Act
            _sut.HandleLoadScreenCommands();
            
            //Assert
            if (input.Equals("return"))
            {
                _mockedScreenHandler.Verify(mock => mock.TransitionTo(It.IsAny<StartScreen>()), Times.Once);
            }
            else
            {
                if (input.Equals("1"))
                {
                    _mockedGamesSessionService.Verify(mock => mock.LoadGame(testId));
                }
                else if (input.Equals(""))
                {
                    _mockedScreenHandler.Verify(mock=> mock.UpdateInputMessage("Session number cannot be left blank, please try again!"));
                }
                else
                {
                    _mockedScreenHandler.Verify(mock=> mock.UpdateInputMessage("Not a valid session number, please try again!"));
                }
            }
        }
    }
}