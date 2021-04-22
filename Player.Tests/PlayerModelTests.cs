/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD-Project.
 
    This file is created by team: 2.
     
    Goal of this file: testing changing the position of the player after input.
     
*/

using NUnit.Framework;

namespace Player.Tests
{
    [TestFixture]
    public class PlayerModelTests
    {

        private Player movementComponent;

        [SetUp]
        public void Setup()
        {
            movementComponent = new Player();
        }


        [Test]
        public void TestMoveRight()
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
        public void TestMoveLeft()
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
        public void TestMoveForward()
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
        public void TestMoveBackward()
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
        public void HandleDirectionTest()
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