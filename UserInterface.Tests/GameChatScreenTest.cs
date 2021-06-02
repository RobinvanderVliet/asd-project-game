using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using UserInterface;
using System.IO;

namespace UserInterface.Tests
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
            //var messageLong = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc pretium posuere orci in posuere. Integer sit amet imperdiet lacus, sed suscipit dolor. Pellentesque venenatis urna vitae nisl consequat vestibulum. Morbi eu est sagittis, pellentesque elit ut, gravida urna. Pellentesque cursus eget ex ut efficitur. Aliquam in massa quis arcu finibus rutrum. Mauris efficitur nec lacus sit amet porta. Donec tincidunt augue quis gravida mattis. In ultricies et dolor sed dignissim. Quisque dapibus faucibus velit eget vestibulum. Mauris eros dolor, elementum ac sollicitudin sagittis, maximus vitae metus.Sed ac ipsum fringilla, tincidunt enim et, fermentum odio. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Nulla facilisi. Interdum et malesuada fames ac ante ipsum primis in faucibus. Nulla ut diam nec lorem congue congue faucibus a justo. Proin elit lectus, semper at odio non, vulputate fringilla lorem. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus vehicula nisi vel magna semper laoreet. Phasellus et ex lorem. Nullam sollicitudin leo dui. Maecenas gravida gravida sodales. Quisque fermentum massa eu ante finibus, sed posuere quam feugiat. Nunc eget dapibus diam, vel dignissim magna. Sed cursus eleifend urna ut sodales. Mauris volutpat ipsum quam, sit amet laoreet est tristique ut. Integer hendrerit congue malesuada.Nulla magna neque, eleifend quis ex ac, condimentum tempor arcu. Nunc auctor velit eget erat tempus sodales. Donec tempor scelerisque consequat. Fusce accumsan diam eget sapien aliquam fringilla. Cras interdum mattis libero, vitae iaculis diam. Sed est augue, sollicitudin non nulla sit amet, lobortis sodales est. Ut et semper augue, sed faucibus lorem. Vestibulum eu risus tincidunt, sagittis libero sed, eleifend metus. Pellentesque sit amet congue nisi. Cras sem orci, feugiat eget iaculis non, sagittis quis nisi.Nunc molestie, orci nec accumsan congue, ex justo ullamcorper libero, id euismod magna elit tempor eros. Sed elit lacus, imperdiet sed sem id, vestibulum tincidunt tortor. Suspendisse lacinia rutrum neque, non dapibus orci tincidunt quis. Nunc ac nisl fermentum, aliquet libero nec, aliquet mauris. Proin augue lacus, feugiat in volutpat eget, elementum eu ipsum. Proin dictum ligula fringilla lorem hendrerit vestibulum. Aenean arcu lorem, mattis ut purus in, dictum pellentesque risus. Nunc posuere ultricies mauris, et porta dui finibus auctor. Cras ante enim, facilisis ut dapibus sed, fringilla a metus.";
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
            //_mockedConsoleHelper.Verify(mock => mock.Write(messageTwo), Times.Once);
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
                if (i == CHAT_HEIGHT - 1)
                {
                    stringQueue.Enqueue("+");
                }
                else if (i == 0)
                {
                    stringQueue.Enqueue("-");
                }
                else
                {
                    stringQueue.Enqueue("0");
                }
            }
            //Act
            _sut.ShowMessages(stringQueue);

            //Assert
            _mockedConsoleHelper.Verify(mock => mock.Write("0"), Times.Exactly(CHAT_HEIGHT - 1));
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
                    stringQueue.Enqueue(startMessage + message);
                }
                else if (i == 0)
                {
                    stringQueue.Enqueue("-");
                }
                else
                {
                    stringQueue.Enqueue("0");
                }
            }
            //Act
            _sut.ShowMessages(stringQueue);

            //Assert
            _mockedConsoleHelper.Verify(mock => mock.Write("0"), Times.Exactly(CHAT_HEIGHT - 2));
            _mockedConsoleHelper.Verify(mock => mock.Write("-"), Times.Never);
            _mockedConsoleHelper.Verify(mock => mock.Write(startMessage), Times.Once);
            _mockedConsoleHelper.Verify(mock => mock.Write(message), Times.Once);
        }
    }
}
