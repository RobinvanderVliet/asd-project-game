using Moq;
using NUnit.Framework;
using Player.Model;
using Player.Services;

namespace Player.Tests
{
    public class PlayerServiceTest
    {
        private PlayerService sut;
        private Mock<IPlayerModel> _mockedPlayerModel;
        
        [SetUp]
        public void Setup()
        {
            _mockedPlayerModel = new Mock<IPlayerModel>();
            sut = new PlayerService(_mockedPlayerModel.Object);
        }
        
        [Test]
        public void Test_GetCurrentPosition_VerifyPlayerModelMoqWorks1()
        {
            var direction_right = "right";
            int steps = 5;
            int[] newMovement = {steps, 0};
            int[] playerPosition = {31, 11};
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.SetNewPlayerPosition(newMovement));
            _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.CurrentPosition).Returns(playerPosition);

            sut.HandleDirection(direction_right, steps);
            
            _mockedPlayerModel.Verify(_mockedPlayerModel => _mockedPlayerModel.SetNewPlayerPosition(newMovement), Times.Once);
        }
        // [Test]
        // public void Test_MoveRight_AssertsDirectionPositionsCorrectlyToTheRight()
        // {
        //     // Arrange
        //     var direction_right = "right";
        //     int steps = 5;
        //     int[] playerPosition = {31, 11};
        //     int[] newMovement = {steps, 0};
        //     _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.CurrentPosition).Returns(playerPosition);
        //     _mockedPlayerModel.Setup(_mockedPlayerModel => _mockedPlayerModel.SetNewPlayerPosition(newMovement));
        //
        //     // Act
        //     sut.HandleDirection(direction_right, steps);
        //
        //     // Assert
        //     
        //     Assert.AreEqual( playerPosition, _mockedPlayerModel.Object.CurrentPosition);
        // }

        // [Test]
        // public void Test_MoveLeft_AssertsDirectionPositionsCorrectlyToTheLeft()
        // {
        //     // Arrange
        //     var direction_left = "left";
        //     int steps = 5;
        //
        //     // Act
        //     _playerModel.HandleDirection(direction_left, steps);
        //
        //     // Assert
        //     int[] playerPosition = new int[2];
        //     playerPosition[0] = 21;
        //     playerPosition[1] = 11;
        //     Assert.AreEqual(_playerModel.GetNewPosition, playerPosition);
        // }
        //
        // [Test]
        // public void Test_MoveForward_AssertsDirectionPositionsCorrectlyToTheNorth()
        // {
        //     // Arrange
        //     var direction_forward = "forward";
        //     int steps = 5;
        //
        //     // Act
        //     _playerModel.HandleDirection(direction_forward, steps);
        //
        //     // Assert
        //     int[] playerPosition = new int[2];
        //     playerPosition[0] = 26;
        //     playerPosition[1] = 6;
        //     Assert.AreEqual(_playerModel.GetNewPosition, playerPosition);
        // }
        //
        // [Test]
        // public void Test_MoveBackward_AssertsDirectionPositionsCorrectlyToTheSouth()
        // {
        //     // Arrange
        //     var direction_backward = "backward";
        //     int steps = 5;
        //
        //     // Act
        //     _playerModel.HandleDirection(direction_backward, steps);
        //
        //     // Assert
        //     int[] playerPosition = new int[2];
        //     playerPosition[0] = 26;
        //     playerPosition[1] = 16;
        //     Assert.AreEqual(_playerModel.GetNewPosition, playerPosition);
        // }
        //
        // [Test]
        // public void Test_HandleDirection_AssertsPositionsCorrectly()
        // {
        //     // Arrange
        //     int[] newMovement = new int[2];
        //     newMovement[0] = 0;
        //     newMovement[1] = 3;
        //
        //     // Act
        //     var result = _playerModel.SetNewPlayerPosition(newMovement);
        //
        //     // Assert
        //     int[] playerPosition = new int[2];
        //     playerPosition[0] = 26;
        //     playerPosition[1] = 14;
        //     Assert.AreEqual(result, playerPosition);
        // }
    }
}