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
using Network.DTO;
using Newtonsoft.Json;
using System.IO;

namespace Chat.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    class ChatHandlerTest
    {
        //Declaration and initialisation of constant variables

        //Declaration of variables
        private ChatHandler _chatHandler;
        private PacketDTO _packetDTO;
        private ChatDTO _chatDTO;

        //Declaration of mocks
        private Mock<IClientController> _mockedClientController;

        [SetUp]
        public void Setup()
        {
            _mockedClientController = new Mock<IClientController>();
            _chatHandler = new ChatHandler(_mockedClientController.Object);
            _packetDTO = new PacketDTO();

        }

        [Test]
        public void Test_SendSay_SendsTheMessageAndPacketTypeToClientController()
        {
            //Arrange ---------
            string message = "Hello World";
            _chatDTO = new ChatDTO(ChatType.Say, message);
            var payload = JsonConvert.SerializeObject(_chatDTO);
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
            _chatDTO = new ChatDTO(ChatType.Shout, message);
            var payload = JsonConvert.SerializeObject(_chatDTO);
            _mockedClientController.Setup(mock => mock.SendPayload(payload, PacketType.Chat));

            //Act ---------
            _chatHandler.SendShout(message);
            //Assert ---------
            _mockedClientController.Verify(mock => mock.SendPayload(payload, PacketType.Chat), Times.Once());
        }

        [Test]
        public void Test_HandlePacket_HandleSayProperly()
        {
            //Arrange ---------
            string message = "Hello World";
            _chatDTO = new ChatDTO(ChatType.Say, message);
            var payload = JsonConvert.SerializeObject(_chatDTO);
            _packetDTO.Payload = payload;

            using (StringWriter sw = new StringWriter())
            {
                //Act ---------
                Console.SetOut(sw);
                _chatHandler.HandlePacket(_packetDTO);

                //Assert ---------
                string expected = string.Format(" said: Hello World\r\n", Environment.NewLine);
                Assert.AreEqual(expected, sw.ToString());
            }                 
        }

        [Test]
        public void Test_HandlePacket_HandleShoutProperly()
        {
            //Arrange ---------
            string message = "Hello World";
            _chatDTO = new ChatDTO(ChatType.Shout, message);
            var payload = JsonConvert.SerializeObject(_chatDTO);
            _packetDTO.Payload = payload;

            using (StringWriter sw = new StringWriter())
            {
                //Act ---------
                Console.SetOut(sw);
                _chatHandler.HandlePacket(_packetDTO);

                //Assert ---------
                string expected = string.Format(" shouted: Hello World\r\n", Environment.NewLine);
                Assert.AreEqual(expected, sw.ToString());
            }
        }
    }
}
