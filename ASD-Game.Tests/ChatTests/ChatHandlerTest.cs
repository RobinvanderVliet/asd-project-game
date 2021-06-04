using System;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using Moq;
using Network;
using Chat.DTO;
using Network.DTO;
using Newtonsoft.Json;
using System.IO;
using WorldGeneration;
using Messages;

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
        private Mock<IWorldService> _mockedWorldService;
        private Mock<IMessageService> _mockedMessageService;

        [SetUp]
        public void Setup()
        {
            _mockedClientController = new();
            _mockedWorldService = new();
            _mockedMessageService = new();

            _sut = new ChatHandler(_mockedClientController.Object, _mockedWorldService.Object, _mockedMessageService.Object);
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
            string originId = "origin1";
            string message = "Hello World";
            _chatDTO = new ChatDTO(ChatType.Say, message);
            _chatDTO.OriginId = originId;
            var payload = JsonConvert.SerializeObject(_chatDTO);
            _packetDTO.Payload = payload;
            Player player = new("arie", 0, 0, "#", originId);
            _mockedWorldService.Setup(mock => mock.GetPlayer(player.Id)).Returns(player);


            //Act ---------
            HandlerResponseDTO actualResult = _sut.HandlePacket(_packetDTO);

            //Assert ---------
            HandlerResponseDTO ExpectedResult = new HandlerResponseDTO(SendAction.SendToClients, null);
            string expected = $"{player.Name} said: {message}";
            Assert.AreEqual(ExpectedResult, actualResult);
            _mockedMessageService.Verify(mock => mock.AddMessage(expected), Times.Once);
        }

        [Test]
        public void Test_HandlePacket_HandleSayNoNameSetProperly()
        {
            //Arrange ---------
            string originId = "origin1";
            string message = "Hello World";
            _chatDTO = new ChatDTO(ChatType.Say, message);
            _chatDTO.OriginId = originId;
            var payload = JsonConvert.SerializeObject(_chatDTO);
            _packetDTO.Payload = payload;
            Player player = new(null, 0, 0, "#", originId);
            _mockedWorldService.Setup(mock => mock.GetPlayer(player.Id)).Returns(player);


            //Act ---------
            HandlerResponseDTO actualResult = _sut.HandlePacket(_packetDTO);

            //Assert ---------
            HandlerResponseDTO ExpectedResult = new HandlerResponseDTO(SendAction.SendToClients, null);
            string expected = $"player with id '{originId}' said: {message}";
            Assert.AreEqual(ExpectedResult, actualResult);
            _mockedMessageService.Verify(mock => mock.AddMessage(expected), Times.Once);
        }

        [Test]
        public void Test_HandlePacket_HandleShoutProperly()
        {
            //Arrange ---------
            string originId = "origin1";
            string message = "Hello World";
            _chatDTO = new ChatDTO(ChatType.Shout, message);
            _chatDTO.OriginId = originId;
            var payload = JsonConvert.SerializeObject(_chatDTO);
            _packetDTO.Payload = payload;
            Player player = new("arie", 0, 0, "#", originId);
            _mockedWorldService.Setup(mock => mock.GetPlayer(player.Id)).Returns(player);


            //Act ---------
            HandlerResponseDTO actualResult = _sut.HandlePacket(_packetDTO);

            //Assert ---------
            HandlerResponseDTO ExpectedResult = new HandlerResponseDTO(SendAction.SendToClients, null);
            string expected = $"{player.Name} shouted: {message}";
            Assert.AreEqual(ExpectedResult, actualResult);
            _mockedMessageService.Verify(mock => mock.AddMessage(expected), Times.Once);
        }


    }
}
