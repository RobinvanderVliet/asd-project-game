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
    class MessageServiceTest
    {
        private MessageService _sut;
        private Mock<IScreenHandler> _mockedScreenhandler;

        [SetUp]
        public void Setup()
        {
            _mockedScreenhandler = new Mock<IScreenHandler>();
            _sut = new MessageService(_mockedScreenhandler.Object);
        }

        [Test]
        public void Test_AddMessage_MessageIsAdded()
        {
            //Arrange
            var message = "This is a test message";
            Queue<string> result = new Queue<string>();
            result.Enqueue(message);
            _mockedScreenhandler.Setup(mock => mock.ShowMessages(result));
            //Act
            _sut.AddMessage(message);

            //Assert
            _mockedScreenhandler.Verify(mock => mock.ShowMessages(result), Times.Once);           
        }
    }
}
