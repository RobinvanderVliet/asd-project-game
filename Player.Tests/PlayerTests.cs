using NUnit.Framework;

namespace Player.Tests
{
    [TestFixture]
    public class PlayerTests
    {

        private PlayerModel movementComponent;

        [SetUp]
        public void Setup()
        {
            movementComponent = new PlayerModel();
        }


        [Test]
        public void Test_MoveRight_AssertsDirectionPositionsCorrectlyToTheRight()
        {
            // Arrange
            var direction_right = "right";
            int steps = 5;

            // Act
            movementComponent.HandleDirection(direction_right, steps);

            // Assert
            int[] playerPosition = new int[2];
            playerPosition[0] = 31;
            playerPosition[1] = 11;
            Assert.AreEqual(movementComponent.GetNewPosition, playerPosition);
        }

        [Test]
        public void Test_MoveLeft_AssertsDirectionPositionsCorrectlyToTheLeft()
        {
            // Arrange
            var direction_left = "left";
            int steps = 5;

            // Act
            movementComponent.HandleDirection(direction_left, steps);

            // Assert
            int[] playerPosition = new int[2];
            playerPosition[0] = 21;
            playerPosition[1] = 11;
            Assert.AreEqual(movementComponent.GetNewPosition, playerPosition);
        }

        [Test]
        public void Test_MoveForward_AssertsDirectionPositionsCorrectlyToTheNorth()
        {
            // Arrange
            var direction_forward = "forward";
            int steps = 5;

            // Act
            movementComponent.HandleDirection(direction_forward, steps);

            // Assert
            int[] playerPosition = new int[2];
            playerPosition[0] = 26;
            playerPosition[1] = 6;
            Assert.AreEqual(movementComponent.GetNewPosition, playerPosition);
        }

        [Test]
        public void Test_MoveBackward_AssertsDirectionPositionsCorrectlyToTheSouth()
        {
            // Arrange
            var direction_backward = "backward";
            int steps = 5;

            // Act
            movementComponent.HandleDirection(direction_backward, steps);

            // Assert
            int[] playerPosition = new int[2];
            playerPosition[0] = 26;
            playerPosition[1] = 16;
            Assert.AreEqual(movementComponent.GetNewPosition, playerPosition);
        }

        [Test]
        public void Test_HandleDirection_AssertsPositionsCorrectly()
        {
            // Arrange
            int[] newMovement = new int[2];
            newMovement[0] = 0;
            newMovement[1] = 3;

            // Act
            var result = movementComponent.SendNewPosition(newMovement);

            // Assert
            int[] playerPosition = new int[2];
            playerPosition[0] = 26;
            playerPosition[1] = 14;
            Assert.AreEqual(result, playerPosition);
        }
    }
}