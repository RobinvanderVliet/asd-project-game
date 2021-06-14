using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using ASD_Game.UserInterface;
using Moq;
using NUnit.Framework;

namespace ASD_Game.Tests.UserInterfaceTests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    class GameScreenTest
    {
        private GameScreen _sut;
        private Mock<ScreenHandler> _mockedScreenHandler;
        private Mock<ConsoleHelper> _mockedConsoleHelper;
        private Mock<IGameChatScreen> _mockedGameChatScreen;
        private Mock<IGameStatScreen> _mockedGameStatScreen;
        private Mock<IGameWorldScreen> _mockedGameWorldScreen;

        [SetUp]
        public void Setup()
        {
            _sut = new GameScreen();
            _mockedScreenHandler = new Mock<ScreenHandler>();
            _mockedConsoleHelper = new Mock<ConsoleHelper>();
            _mockedGameChatScreen = new Mock<IGameChatScreen>();
            _mockedGameStatScreen = new Mock<IGameStatScreen>();
            _mockedGameWorldScreen = new Mock<IGameWorldScreen>();
            var screenHandler = _mockedScreenHandler.Object;
            screenHandler.ConsoleHelper = _mockedConsoleHelper.Object;
            _sut.SetScreen(screenHandler);
            _sut.setScreens(_mockedGameStatScreen.Object, _mockedGameChatScreen.Object, _mockedGameWorldScreen.Object);
        }

        [Test]
        public void Test_DrawScreen_DrawsScreen()
        {
            //Arrange
            _mockedGameStatScreen.Setup(mock => mock.DrawScreen());
            _mockedGameChatScreen.Setup(mock => mock.DrawScreen());
            _mockedGameWorldScreen.Setup(mock => mock.DrawScreen());
            var ulCorner = "╔";
            var llCorner = "╚";
            var urCorner = "╗";
            var lrCorner = "╝";

            //Act
            _sut.DrawScreen();

            //Assert
            _mockedGameStatScreen.Verify(mock => mock.DrawScreen(), Times.Once);
            _mockedGameChatScreen.Verify(mock => mock.DrawScreen(), Times.Once);
            _mockedGameWorldScreen.Verify(mock => mock.DrawScreen(), Times.Once);
            _mockedConsoleHelper.Verify(mock => mock.Write(ulCorner), Times.Exactly(1));
            _mockedConsoleHelper.Verify(mock => mock.Write(urCorner), Times.Exactly(1));
            _mockedConsoleHelper.Verify(mock => mock.Write(llCorner), Times.Exactly(1));
            _mockedConsoleHelper.Verify(mock => mock.Write(lrCorner), Times.Exactly(1));
        }

        [Test]
        public void Test_RedrawInputBox_DrawsInputBox()
        {
            //Arrange
            var ulCorner = "╔";
            var llCorner = "╚";
            var urCorner = "╗";
            var lrCorner = "╝";

            //Act
            _sut.RedrawInputBox();

            //Assert        
            _mockedConsoleHelper.Verify(mock => mock.Write(ulCorner), Times.Exactly(1));
            _mockedConsoleHelper.Verify(mock => mock.Write(urCorner), Times.Exactly(1));
            _mockedConsoleHelper.Verify(mock => mock.Write(llCorner), Times.Exactly(1));
            _mockedConsoleHelper.Verify(mock => mock.Write(lrCorner), Times.Exactly(1));
        }

        [Test]
        public void Test_ShowMessages_SendsMessagesToGameChatScreen()
        {
            //Arrange
            string message = "TestMessage";
            Queue<String> messages = new Queue<string>();
            messages.Enqueue(message);
            _mockedGameChatScreen.Setup(mock => mock.ShowMessages(messages));

            //Act
            _sut.ShowMessages(messages);

            //Assert        
            _mockedGameChatScreen.Verify(mock => mock.ShowMessages(messages), Times.Once);
        }

        [Test]
        public void Test_UpdateWorld_SendsMapToGameMapScreen()
        {
            //Arrange
            char[,] map = new char[13, 13];
            _mockedGameWorldScreen.Setup(mock => mock.UpdateWorld(map));

            //Act
            _sut.UpdateWorld(map);

            //Assert        
            _mockedGameWorldScreen.Verify(mock => mock.UpdateWorld(map), Times.Once);
        }

        [Test]
        public void Test_setStatValue_SendsStatsToGameStatScreen()
        {
            //Arrange
            string name = "TestName";
            int score = 0;
            int health = 100;
            int stamina = 100;
            int armor = 10;
            int radiation = 100;
            string helm = "Bandana";
            string body = "Jacket";
            string weapon = "Knife";
            string slotOne = "Bandage";
            string slotTwo = "Morphine";
            string slotThree = "Empty";
            _mockedGameStatScreen.Setup(mock => mock.SetStatValues(name, score, health, stamina, armor, radiation, helm, body, weapon, slotOne, slotTwo, slotThree));

            //Act
            _sut.SetStatValues(name, score, health, stamina, armor, radiation, helm, body, weapon, slotOne, slotTwo, slotThree);

            //Assert        
            _mockedGameStatScreen.Verify(mock => mock.SetStatValues(name, score, health, stamina, armor, radiation, helm, body, weapon, slotOne, slotTwo, slotThree), Times.Once);
        }
    }
}
