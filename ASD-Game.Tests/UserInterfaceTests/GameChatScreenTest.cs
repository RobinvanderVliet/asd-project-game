using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ASD_Game.UserInterface;
using Moq;
using NUnit.Framework;

namespace ASD_Game.Tests.UserInterfaceTests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    class GameChatScreenTest
    {
        private GameChatScreen _sut;
        private Mock<ScreenHandler> _mockedScreenHandler;
        private Mock<ConsoleHelper> _mockedConsoleHelper;
        private const int CHAT_X = 0;
        private const int CHAT_Y = 0;
        private const int CHAT_WIDTH = 120 - 2;
        private const int CHAT_HEIGHT = 13;

        [SetUp]
        public void Setup()
        {
            _sut = new GameChatScreen(CHAT_X, CHAT_Y, CHAT_WIDTH, CHAT_HEIGHT);
            _mockedScreenHandler = new Mock<ScreenHandler>();
            _mockedConsoleHelper = new Mock<ConsoleHelper>();
            var screenHandler = _mockedScreenHandler.Object;
            screenHandler.ConsoleHelper = _mockedConsoleHelper.Object;
            _sut.SetScreen(screenHandler);
        }

        [Test]
        public void Test_DrawScreen_DrawsScreen()
        {
            //Arrange            
            var ulCorner = "╔";
            var llCorner = "╚";
            var urCorner = "╗";
            var lrCorner = "╝";

            //Act
            _sut.DrawScreen();

            //Assert
            _mockedConsoleHelper.Verify(mock => mock.Write(ulCorner), Times.Exactly(1));
            _mockedConsoleHelper.Verify(mock => mock.Write(urCorner), Times.Exactly(1));
            _mockedConsoleHelper.Verify(mock => mock.Write(llCorner), Times.Exactly(1));
            _mockedConsoleHelper.Verify(mock => mock.Write(lrCorner), Times.Exactly(1));
        }

        [Test]
        public void Test_ShowMessages_DrawsAllNormalMessages()
        {
            //Arrange
            string messageOne = "This is my test message!";
            string messageTwo = "This is another test message";
            Queue<string> stringQueue = new Queue<string>();
            stringQueue.Enqueue(messageOne);
            stringQueue.Enqueue(messageTwo);

            //Act
            _sut.ShowMessages(stringQueue);

            //Assert
            _mockedConsoleHelper.Verify(mock => mock.Write(messageOne), Times.Once);
            _mockedConsoleHelper.Verify(mock => mock.Write(messageTwo), Times.Once);
        }

        [Test]
        public void Test_ShowMessages_DrawsLongerMessageOnNextLine()
        {
            //Arrange
            string s1 = "";
            string s2 = "";
            for (int i = 0; i < CHAT_WIDTH * 2 - 20; i++)
            {
                if (i < CHAT_WIDTH - 2)
                {
                    s1 += "a";
                }
                else
                {
                    s2 += "b";
                }
            }
            Queue<string> stringQueue = new Queue<string>();
            stringQueue.Enqueue(s1 + s2);

            //Act
            _sut.ShowMessages(stringQueue);

            //Assert
            _mockedConsoleHelper.Verify(mock => mock.Write(s1), Times.Once);
            _mockedConsoleHelper.Verify(mock => mock.Write(s2), Times.Once);
            }

        [Test]
        public void Test_ShowMessages_CutsOfMessageWhenToLong()
        {
            //Arrange        
            int maxSize = (CHAT_WIDTH - 2) * CHAT_HEIGHT;
            string s1 = new string('+', maxSize + 20);
            int lastRow = (CHAT_WIDTH - 2) * (CHAT_HEIGHT - 1);
            string s2 = s1.Substring(lastRow, (CHAT_WIDTH - 2) - 3) + "...";


            Queue<string> stringQueue = new Queue<string>();
            stringQueue.Enqueue(s1);

            //Act
            _sut.ShowMessages(stringQueue);

            //Assert
            _mockedConsoleHelper.Verify(mock => mock.Write(s2), Times.Once);
        }

        [Test]
        public void Test_ShowMessages_DontShowOlderMessages()
        {
            //Arrange
            Queue<string> stringQueue = new Queue<string>();
            for (int i = 0; i <= CHAT_HEIGHT; i++)
            {
                if (i == 0)
                {
                    stringQueue.Enqueue("+");
                }
                else if (i == CHAT_HEIGHT)
                {
                    stringQueue.Enqueue("-");
                }
                else
                {
                    stringQueue.Enqueue("=");
                }
            }
            //Act
            _sut.ShowMessages(stringQueue);

            //Assert
            _mockedConsoleHelper.Verify(mock => mock.Write("="), Times.Exactly(CHAT_HEIGHT - 1));
            _mockedConsoleHelper.Verify(mock => mock.Write("-"), Times.Never);
            _mockedConsoleHelper.Verify(mock => mock.Write("+"), Times.Once);
        }

        [Test]
        public void Test_ShowMessages_LastMessageShouldTakeTwoRows()
        {
            //Arrange
            string message = "This will be on the next line";
            string startMessage = new string('+', (CHAT_WIDTH - 2));
            Queue<string> stringQueue = new Queue<string>();
            for (int i = 0; i <= CHAT_HEIGHT; i++)
            {
                if (i == CHAT_HEIGHT - 1)
                {
                    stringQueue.Enqueue("-");
                }
                else if (i == 0)
                {                 
                    stringQueue.Enqueue(startMessage + message);
                }
                else
                {
                    stringQueue.Enqueue("=");
                }
            }
            //Act
            _sut.ShowMessages(stringQueue);

            //Assert
            _mockedConsoleHelper.Verify(mock => mock.Write("="), Times.Exactly(CHAT_HEIGHT - 2));
            _mockedConsoleHelper.Verify(mock => mock.Write("-"), Times.Never);
            _mockedConsoleHelper.Verify(mock => mock.Write(startMessage), Times.Once);
            _mockedConsoleHelper.Verify(mock => mock.Write(message), Times.Once);
        }
    }
}
