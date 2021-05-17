using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using Moq;
using Network;
using Chat.DTO;
using Newtonsoft.Json;

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
        private Mock<IClientController> _mockedClientController;
   

        [SetUp]
        public void Setup()
        {
            _mockedClientController = new Mock<IClientController>();
            _chatHandler = new ChatHandler(_mockedClientController.Object);
            
        }

        [Test]
        public void Test_SendSay_SendsTheMessageAndPacketTypeToClientController()
        {
            //Arrange ---------
            string message = "Hello World";
            ChatDTO chatDTO = new ChatDTO(ChatType.Say, message);
            var payload = JsonConvert.SerializeObject(chatDTO);
            _mockedClientController.Setup(mock => mock.SendPayload(payload, PacketType.Chat));

            //Act ---------
            _chatHandler.SendSay(message);
            //Assert ---------
            _mockedClientController.Verify(mock => mock.SendPayload(payload, PacketType.Chat), Times.Once());
        }

        [Test]
        public void Test_SendShout_SendsTheMessageAndPacketTypeToClientController()
        {
            //Arrange ---------
            string message = "Hello World";
            ChatDTO chatDTO = new ChatDTO(ChatType.Shout, message);
            var payload = JsonConvert.SerializeObject(chatDTO);
            _mockedClientController.Setup(mock => mock.SendPayload(payload, PacketType.Chat));

            //Act ---------
            _chatHandler.SendShout(message);
            //Assert ---------
            _mockedClientController.Verify(mock => mock.SendPayload(payload, PacketType.Chat), Times.Once());
        }
    }
}
