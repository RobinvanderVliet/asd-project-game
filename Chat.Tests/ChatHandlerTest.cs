using System;
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
        private ChatHandler _sut;
        private PacketDTO _packetDTO;
        private ChatDTO _chatDTO;

        //Declaration of mocks
        private Mock<IClientController> _mockedClientController;

        [SetUp]
        public void Setup()
        {
            _mockedClientController = new Mock<IClientController>();
            _sut = new ChatHandler(_mockedClientController.Object);
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
            _sut.SendSay(message);
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
            _sut.SendShout(message);
            //Assert ---------
            _mockedClientController.Verify(mock => mock.SendPayload(payload, PacketType.Chat), Times.Once());
        }

        [Test]
        public void Test_HandlePacket_HandleInvalidChatTypeProperly()
        {
            //Arrange ---------
            var payload = "{\"ChatType\":8,\"Message\":\"Hello World\",\"OriginId\":null}";
            _packetDTO.Payload = payload;

            //Act ---------
            HandlerResponseDTO actualResult = _sut.HandlePacket(_packetDTO);

            //Assert ---------
            HandlerResponseDTO ExpectedResult = new HandlerResponseDTO(SendAction.Ignore, null);
            Assert.AreEqual(ExpectedResult, actualResult);
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
                HandlerResponseDTO actualResult = _sut.HandlePacket(_packetDTO);

                //Assert ---------
                HandlerResponseDTO ExpectedResult = new HandlerResponseDTO(SendAction.SendToClients, null);
                string expected = string.Format(" said: Hello World\r\n", Environment.NewLine);
                Assert.AreEqual(expected, sw.ToString());
                Assert.AreEqual(ExpectedResult, actualResult);
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
                HandlerResponseDTO actualResult = _sut.HandlePacket(_packetDTO);

                //Assert ---------
                HandlerResponseDTO ExpectedResult = new HandlerResponseDTO(SendAction.SendToClients, null);
                string expected = string.Format(" shouted: Hello World\r\n", Environment.NewLine);
                Assert.AreEqual(expected, sw.ToString());
                Assert.AreEqual(ExpectedResult, actualResult);
            }
        }
    }
}
