using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using ASD_Game.Session;
using ASD_Game.Messages;
using NUnit.Framework;
using Moq;

namespace ASD_Game.Tests.SessionTests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    class HeartbeatHandlerTest
    {
        //Declaration and initialisation of constant variables
        private HeartbeatHandler _sut;
        private StringWriter _stringWriter;
        private TextWriter _originalOutput;
        private Mock<IMessageService> _mockedMessageService;

        [SetUp]
        public void Setup()
        {
            _mockedMessageService = new Mock<IMessageService>();
            _sut = new HeartbeatHandler(_mockedMessageService.Object);
            _stringWriter = new StringWriter();
            _originalOutput = Console.Out;
            Console.SetOut(_stringWriter);

        }

        [Test]
        public void Test_ReceiveHeartbeat_Success()
        {
            //arrange
            var message = "Agents are enabled";
            _mockedMessageService.Setup(mock => mock.AddMessage(message));

            // Act
            _sut.ReceiveHeartbeat(message);

            _mockedMessageService.Verify(mock => mock.AddMessage(message), Times.Never);           
        }

        [Test]
        public void Test_ReceiveHeartbeat_Fail()
        {
            //arrange
            var message = "Agents are enabled";
            _mockedMessageService.Setup(mock => mock.AddMessage(message));


                //Act ---------
                _sut.ReceiveHeartbeat("test");
                _sut.ReceiveHeartbeat("test2");
                Thread.Sleep(2000);
                _sut.ReceiveHeartbeat("test2");

            //Assert ---------
            _mockedMessageService.Verify(mock => mock.AddMessage(message), Times.Once);        
        }
    }
}
