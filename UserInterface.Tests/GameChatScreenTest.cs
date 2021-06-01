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
        private const int CHAT_X = 0;
        private const int CHAT_Y = 0;
        private const int CHAT_WIDTH = (120 - 2) - (25 + 2);
        private const int CHAT_HEIGHT = 13;

        [SetUp]
        public void Setup()
        {
            _sut = new GameChatScreen(CHAT_X, CHAT_Y, CHAT_WIDTH, CHAT_HEIGHT);
        }

        [Test]
        public void Test_DrawScreen_DrawsTheScreenProperly()
        {
            //Arrange ---------

            //Act ---------

            //Assert ---------
            
        }
    }
}
