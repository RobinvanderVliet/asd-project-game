using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ASD_Game.Chat.DTO;
using ASD_Game.UserInterface;
using Moq;
using NUnit.Framework;

namespace ASD_Game.Tests.UserInterfaceTests
{
    [ExcludeFromCodeCoverage]
    public class LobbyScreenTest
    {
        private LobbyScreen _sut;
        private Mock<ScreenHandler> _mockedScreenHandler;
        private Mock<ConsoleHelper> _mockedConsoleHelper;

        [SetUp]
        public void Setup()
        {
            _sut = new LobbyScreen();
            _mockedScreenHandler = new Mock<ScreenHandler>();
            _mockedConsoleHelper = new Mock<ConsoleHelper>();
            var screenHandler = _mockedScreenHandler.Object;
            screenHandler.ConsoleHelper = _mockedConsoleHelper.Object;
            _sut.SetScreen(screenHandler);
        }

        [Test]
        public void Test_UpdateWithNewSession_AddsNewSession()
        {
            //Arrange
            List<string[]> clients = new List<string[]>();
            clients.Add(new string[] { "1", "swankie" });

            //Act
            _sut.UpdateLobbyScreen(clients);

            //Assert
            _mockedConsoleHelper.Verify(mock => mock.Write("1. " + clients.ElementAt(0)[1]), Times.Once);
        }

        [Test]
        public void Test_DrawLobbyScreen()
        {
            //Arrange
            var headerText = "Welcome to the lobby, people in the lobby:";

            //Act
            _sut.DrawScreen();

            //Assert
            _mockedConsoleHelper.Verify(mock => mock.Write(headerText), Times.Once);
        }

        [Test]
        public void Test_DrawChatMessages()
        {
            //Arrange
            ChatMessageDTO chatMessageDTO = new ChatMessageDTO("swankie", "hello");
            List<ChatMessageDTO> messages = new List<ChatMessageDTO>();
            messages.Add(chatMessageDTO);

            //Act
            _sut.UpdateChat(messages);

            //Assert
            _mockedConsoleHelper.Verify(mock => mock.Write(chatMessageDTO.UserName + " : " + chatMessageDTO.Message), Times.Once);
        }
    }
}