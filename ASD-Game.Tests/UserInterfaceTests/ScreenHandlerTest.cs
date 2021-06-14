using System.Diagnostics.CodeAnalysis;
using ASD_Game.UserInterface;
using Moq;
using NUnit.Framework;

namespace ASD_Game.Tests.UserInterfaceTests
{
    [ExcludeFromCodeCoverage]
    public class ScreenHandlerTest
    {
            private IScreenHandler _sut;
            private Mock<StartScreen> _mockedStartScreen;
            private Mock<ConsoleHelper> _mockedConsoleHelper;
            [SetUp]
            public void Setup()
            {
                _sut = new ScreenHandler();
                _mockedStartScreen = new Mock<StartScreen>();
                _mockedConsoleHelper = new Mock<ConsoleHelper>();
                _sut.ConsoleHelper = _mockedConsoleHelper.Object;
            }

            [Test]
            public void Test_TransitionTo_ChangesAndDrawsScreen()
            {
                //Arrange
                var startScreen = _mockedStartScreen.Object;
                //Act
                _sut.TransitionTo(startScreen);
                //Assert
                Assert.True(_sut.Screen.Equals(startScreen));
                _mockedStartScreen.Verify(mock => mock.DrawScreen(), Times.Once);
            }
        }
    }