using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ASD_Game.Messages;
using ASD_Game.UserInterface;
using Moq;
using NUnit.Framework;

namespace ASD_Game.Tests.MessagesTests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    class MessageModelTest
    {
        private MessageModel _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new MessageModel();
        }

        [Test]
        public void Test_DisplayMessages_ShowsFourtyMessages()
        {
            //Arrange
            var message = "This is a test message ";
            int messageAmount = 40;
            int extraMessages = 10;
            Stack<string> expected = new Stack<string>();
            for (int i = 1; i <= messageAmount + extraMessages; i++)
            {
                _sut.AddMessage(message + i);
                if(i > extraMessages)
                {
                    expected.Push(message + i);
                }
            }
            //Act
            Queue<string> result = _sut.GetLatestMessages(messageAmount);

            //Assert
            Assert.AreEqual(expected, result);
        }
    }
}
