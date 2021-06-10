using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ASD_Game.Chat;
using ASD_Game.Chat.DTO;
using ASD_Game.Messages;
using ASD_Game.Network;
using ASD_Game.Network.DTO;
using ASD_Game.Network.Enum;
using ASD_Game.Session;
using ASD_Game.World.Models.Characters;
using ASD_Game.World.Services;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace ASD_Game.Tests.ChatTests
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
        private Mock<IMessageService> _mockedMessageService;
        private Mock<ISessionHandler> _mockedSessionHandler;

        [SetUp]
        public void Setup()
        {
            _mockedClientController = new();
            _mockedMessageService = new();
            _mockedSessionHandler = new();

            _sut = new ChatHandler(_mockedClientController.Object, _mockedMessageService.Object, _mockedSessionHandler.Object);
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
            string name = "arie";
            _chatDTO = new ChatDTO(ChatType.Say, message);
            _chatDTO.OriginId = originId;
            var payload = JsonConvert.SerializeObject(_chatDTO);
            _packetDTO.Payload = payload;
            string[] player = { originId, name };
            List<string[]> players = new List<string[]>();
            players.Add(player);
            _mockedSessionHandler.Setup(mock => mock.GetAllClients()).Returns(players);


            //Act ---------
            HandlerResponseDTO actualResult = _sut.HandlePacket(_packetDTO);

            //Assert ---------
            HandlerResponseDTO ExpectedResult = new HandlerResponseDTO(SendAction.SendToClients, null);
            string expected = $"{name} said: {message}";
            Assert.AreEqual(ExpectedResult, actualResult);
            _mockedMessageService.Verify(mock => mock.AddMessage(expected), Times.Once);
        }

        //TODO Test fixen
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
            string[] player = { originId, null };
            List<string[]> players = new List<string[]>();
            players.Add(player);
            _mockedSessionHandler.Setup(mock => mock.GetAllClients()).Returns(players);


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
            string name = "arie";
            _chatDTO = new ChatDTO(ChatType.Shout, message);
            _chatDTO.OriginId = originId;
            var payload = JsonConvert.SerializeObject(_chatDTO);
            _packetDTO.Payload = payload;
            string[] player = { originId, name };
            List<string[]> players = new List<string[]>();
            players.Add(player);
            _mockedSessionHandler.Setup(mock => mock.GetAllClients()).Returns(players);


            //Act ---------
            HandlerResponseDTO actualResult = _sut.HandlePacket(_packetDTO);

            //Assert ---------
            HandlerResponseDTO ExpectedResult = new HandlerResponseDTO(SendAction.SendToClients, null);
            string expected = $"{name} shouted: {message}";
            Assert.AreEqual(ExpectedResult, actualResult);
            _mockedMessageService.Verify(mock => mock.AddMessage(expected), Times.Once);
        }


    }
}

