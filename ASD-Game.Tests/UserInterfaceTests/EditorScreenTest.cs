using System.Diagnostics.CodeAnalysis;
using ASD_Game.UserInterface;
using Moq;
using NUnit.Framework;

namespace ASD_Game.Tests.UserInterfaceTests
{
    [ExcludeFromCodeCoverage]
    public class EditorScreenTest
    {

        EditorScreen sut;
        Mock<ConsoleHelper> mockedConsole = new();
        Mock<ScreenHandler> mockedScreen = new();

        [SetUp]
        public void setup()
        {

            mockedScreen.Object.ConsoleHelper = mockedConsole.Object;
            sut = new EditorScreen();
            sut.SetScreen(mockedScreen.Object);
        }

        [Test]
        public void Test_UpdateLastQuestion()
        {
            //Arrange


            //Act
            sut.UpdateLastQuestion(It.IsAny<string>());

            //Assert
            mockedConsole.Verify(x => x.WriteLine(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Test]
        public void Test_PrintWarning()
        {
            //Arrange


            //Act
            sut.PrintWarning(It.IsAny<string>());

            //Assert
            mockedConsole.Verify(x => x.WriteLine(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void Test_ClearScreen()
        {
            //Arrange


            //Act
            sut.ClearScreen();

            //Assert
            mockedConsole.Verify(x => x.ClearConsole(), Times.Once);
        }

    }
}
