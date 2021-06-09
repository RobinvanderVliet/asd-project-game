using System.Diagnostics.CodeAnalysis;
using ASD_Game.Chat;
using ASD_Game.Chat.DTO;
using ASD_Game.Messages;
using ASD_Game.Network;
using ASD_Game.Network.DTO;
using ASD_Game.Network.Enum;
using ASD_Game.World.Models.Characters;
using ASD_Game.World.Services;
using Moq;
using Newtonsoft.Json;
using System.IO;
using WorldGeneration;
using Messages;
using UserInterface;
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
        private Mock<IScreenHandler> _mockedScreenHandler;
        private Mock<GameScreen> _mockedGameScreen;
        private Mock<IWorldService> _mockedWorldService;
        private Mock<IMessageService> _mockedMessageService;

        [SetUp]
        public void Setup()
        {
            _mockedClientController = new();
            _mockedWorldService = new();
            _mockedMessageService = new();

            _sut = new ChatHandler(_mockedClientController.Object, _mockedWorldService.Object, _mockedMessageService.Object);
            _mockedScreenHandler = new Mock<IScreenHandler>();
            _mockedGameScreen = new Mock<GameScreen>();
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
            _chatDTO.OriginId = "TestID";
            string resultMessage = "TestID said: Hello World";
            var payload = JsonConvert.SerializeObject(_chatDTO);
            _packetDTO.Payload = payload;           
           /* _mockedScreenHandler.Setup(mock => mock.Screen).Returns(_mockedGameScreen.Object);
            _mockedGameScreen.Setup(mock => mock.AddMessage(message));

            //Act ---------
            HandlerResponseDTO actualResult = _sut.HandlePacket(_packetDTO);

            //Assert ---------
            _mockedGameScreen.Verify(mock => mock.AddMessage(resultMessage), Times.Once());*/
        }

        //TODO Test fixen
        [Test]
        public void Test_HandlePacket_HandleShoutProperly()
        {
            //Arrange ---------
            string originId = "origin1";
            string message = "Hello World";
            _chatDTO = new ChatDTO(ChatType.Shout, message);
            _chatDTO.OriginId = "TestID";
            string resultMessage = "TestID shouted: Hello World";
            var payload = JsonConvert.SerializeObject(_chatDTO);
            _packetDTO.Payload = payload;
            /*_mockedScreenHandler.Setup(mock => mock.Screen).Returns(_mockedGameScreen.Object);
            _mockedGameScreen.Setup(mock => mock.AddMessage(message));

            //Act ---------
            HandlerResponseDTO actualResult = _sut.HandlePacket(_packetDTO);

            //Assert ---------
            _mockedGameScreen.Verify(mock => mock.AddMessage(resultMessage), Times.Once());*/
        }


    }
}

