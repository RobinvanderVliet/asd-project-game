using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Network.DTO;
using NUnit.Framework;

namespace Network.Tests
{
    public class BackupHostServiceTest
    {

        private BackupHostService _sut;
        public Mock<ILiteDatabaseAsync> mockedConnection;
        public Mock<BackupHostService> mockedSut;

        [SetUp]
        public void SetUp()
        {
            _sut = new BackupHostService();
            mockedSut = new();
            mockedConnection = new();
        }


        [Test]
        public void Test_UpdateBackupDatabase_Update()
        {
            //Arrange
            _sut._db = mockedConnection.Object;
            PacketDTO packet = new()
            {
                Header = new PacketHeaderDTO(),
                Payload = "",
                HandlerResponse = new HandlerResponseDTO(SendAction.Ignore, "")
            };

            mockedSut.Setup(x => x.ConvertDataToPoco(It.IsAny<PacketDTO>())).Returns();
            mockedSut.Setup(x => x.CheckExists(It.IsAny<Object>())).Returns(true);
            mockedSut.Setup(x => x.AlterBackupDatabase(It.IsAny<Object>(), It.IsAny<ILiteDatabaseAsync>())).Returns();

            //Act
            mockedSut.Object.UpdateBackupDatabase(packet);

            //Assert
            mockedConnection.Verify(x => x.Close(), Times.Once);
            mockedConnection.Verify(x => x.GetConnectionAsync(), Times.Once);

            mockedSut.Verify(x => x.ConvertDataToPoco(It.IsAny<PacketDTO>()), Times.Once);
            mockedSut.Verify(x => x.CheckExists(It.IsAny<Object>()), Times.Once);
            mockedSut.Verify(x => x.AlterBackupDatabase(It.IsAny<Object>(), It.IsAny<ILiteDatabaseAsync>()), Times.Once);
        }

        [Test]
        public void Test_UpdateBackupDatabase_Add()
        {
            //Arrange
            _sut._db = mockedConnection.Object;
            PacketDTO packet = new()
            {
                Header = new PacketHeaderDTO(),
                Payload = "",
                HandlerResponse = new HandlerResponseDTO(SendAction.Ignore, "")
            };

            mockedSut.Setup(x => x.ConvertDataToPoco(It.IsAny<PacketDTO>())).Returns();
            mockedSut.Setup(x => x.CheckExists(It.IsAny<Object>())).Returns(false);
            mockedSut.Setup(x => x.AddToBackupDatabase(It.IsAny<Object>(), It.IsAny<ILiteDatabaseAsync>())).Returns();

            //Act
            mockedSut.Object.UpdateBackupDatabase(packet);

            //Assert
            mockedConnection.Verify(x => x.Close(), Times.Once);
            mockedConnection.Verify(x => x.GetConnectionAsync(), Times.Once);

            mockedSut.Verify(x => x.ConvertDataToPoco(It.IsAny<PacketDTO>()), Times.Once);
            mockedSut.Verify(x => x.CheckExists(It.IsAny<Object>()), Times.Once);
            mockedSut.Verify(x => x.AddToBackupDatabase(It.IsAny<Object>(), It.IsAny<ILiteDatabaseAsync>()), Times.Once);
        }

        [Test]
        public void Test_UpdateBackupDatabase_Catch()
        {
            //Arrange
            _sut._db = mockedConnection.Object;
            PacketDTO packet = new()
            {
                Header = new PacketHeaderDTO(),
                Payload = "",
                HandlerResponse = new HandlerResponseDTO(SendAction.Ignore, "")
            };

            mockedSut.Setup(x => x.CheckExists(It.IsAny<Object>())).Throws(new Exception());

            //Assert + Act
            Assert.Throws<Exception>(() => _sut.UpdateBackupDatabase(packet));
        }

        [Test]
        public void Test_ConvertDataToPoco()
        {
            //Arrange
            PacketDTO packet = new()
            {
                Header = new PacketHeaderDTO() { PacketType = PacketType.Move},
                Payload = "",
                HandlerResponse = new HandlerResponseDTO(SendAction.Ignore, "")
            };

            //Act
            var result = _sut.ConvertDataToPoco(packet);

            //Assert
            Assert.Equals(result, new {});
        }


        [Test]
        public void Test_ConvertDataToPoco_Error()
        {
            //Arrange
            PacketDTO packet = new()
            {
                Header = new PacketHeaderDTO() { PacketType = PacketType.Chat},
                Payload = "",
                HandlerResponse = new HandlerResponseDTO(SendAction.Ignore, "")
            };

            //Assert + Act
            Assert.Throws<Exception>(() => _sut.ConvertDataToPoco(packet));
        }

        [Test]
        public void Test_AlterBackupDatabase()
        {
            //Arrange
            _sut._db = mockedConnection.Object;
            Mock<Collection> mockedCollection = new();
            mockedConnection.Setup(x => x.GetCollection<String>(It.IsAny<Object>())).Returns(mockedCollection.Object);

            //Act
            _sut.AlterBackupDatabase(new { }, mockedConnection.Object.GetConnectionAsync());

            //Assert
            mockedConnection.Verify(x => x.GetCollection<String>(It.IsAny<Object>()), Times.Once);
            mockedCollection.Verify(x => x.Update(It.IsAny<Object>), Times.Once);
        }

        [Test]
        public void Test_AddToBackupDatabase()
        {
            //Arrange
            _sut._db = mockedConnection.Object;
            Mock<Collection> mockedCollection = new();
            mockedConnection.Setup(x => x.GetCollection<String>(It.IsAny<Object>())).Returns(mockedCollection.Object);

            //Act
            _sut.AddToBackupDatabase(new { }, mockedConnection.Object.GetConnectionAsync());

            //Assert
            mockedConnection.Verify(x => x.GetCollection<String>(It.IsAny<Object>()), Times.Once);
            mockedCollection.Verify(x => x.Insert(It.IsAny<Object>), Times.Once);
        }

        [Test]
        public void Test_CheckExists()
        {
            //Arrange
            PacketDTO packet = new()
            {
                Header = new PacketHeaderDTO() { PacketType = PacketType.Move },
                Payload = "",
                HandlerResponse = new HandlerResponseDTO(SendAction.Ignore, "")
            };
            
            _sut._db = mockedConnection.Object;
            Mock<Collection> mockedCollection = new();
            mockedConnection.Setup(x => x.GetCollection<String>(It.IsAny<Object>())).Returns(mockedCollection.Object);

            //Act
            var result = _sut.CheckExists(packet);

            //Assert
            Assert.True(result);

        }

        [Test]
        public void Test_EnableDisableBackupHost()
        {
            //Arrange


            //Act + Assert
            Assert.False(_sut.IsBackupHost());
            _sut.EnableBackupHost();
            Assert.True(_sut.IsBackupHost());
            _sut.DisableBackupHost();
            Assert.False(_sut.IsBackupHost());
        }
    }
}
