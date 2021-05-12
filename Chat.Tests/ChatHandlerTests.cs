using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using Moq;
using Network;

namespace Chat.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    class ChatHandlerTests
    {
        //Declaration and initialisation of constant variables
        private ChatHandler _chatHandler;
        //Declaration of variables


        //Declaration of mocks
        private Mock<ClientController> _mockedClientController;
   

        [SetUp]
        public void Setup()
        {
            _mockedClientController = new Mock<ClientController>();
            _chatHandler = new ChatHandler(_mockedClientController.Object);
            
        }

        [Test]
        public void Test_SendSay_SendsTheMessageAndPacketTypeToClientController()
        {
            //Arrange ---------
            string message = "Hello World";
            _mockedClientController.Setup(mock => mock.SendPayload(message, PacketType.Chat));

            //Act ---------
            _chatHandler.SendSay(message);
            //Assert ---------
            _mockedClientController.Verify(mock => mock.SendPayload(message, PacketType.Chat), Times.Once());
        }

        [Test]
        public void Test_SendShout_SendsTheMessageAndPacketTypeToClientController()
        {
            //Arrange ---------
            string message = "Hello World";
            _mockedClientController.Setup(mock => mock.SendPayload(message, PacketType.Chat));

            //Act ---------
            _chatHandler.SendShout(message);
            //Assert ---------
            _mockedClientController.Verify(mock => mock.SendPayload(message, PacketType.Chat), Times.Once());
        }
    }
}
