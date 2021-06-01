using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Moq;
using NUnit.Framework;

namespace WorldGeneration.Tests
{
    [ExcludeFromCodeCoverage]  
    [TestFixture]
    public class WorldServiceTest
    {
        //Declaration and initialisation of constant variables

        //Declaration of variables
        private Player _mapCharacterDTOPlayer;
        private Player _mapCharacterDTOOtherPlayer;

        //Declaration of mocks
        
        private WorldService _sut;
        
        [SetUp]
        public void Setup()
        {
            //Initialisation of variables
            _sut = new WorldService();
            _mapCharacterDTOPlayer = new Player("A", 0, 0 , "#", "1");
            _mapCharacterDTOOtherPlayer = new Player("B", 12, 12, "!", "2");

            //Initialisation of mocks

        }

        [Test]
        public void Test_GetCurrentPlayer() 
        {
            //Arrange ---------
            var currentPlayer = _mapCharacterDTOPlayer;

            //Act ---------
            _sut.AddPlayerToWorld(_mapCharacterDTOPlayer, true);

            //Assert ---------
            Assert.That(_sut.getCurrentPlayer().Equals(currentPlayer));
        }

        [Test]
        public void Test_updateCharacterPositions()
        {
            //Arrange ---------
            var currentPlayer = _mapCharacterDTOPlayer;
            _sut.AddPlayerToWorld(currentPlayer, true);

            //Act ---------
            currentPlayer.XPosition = 2;
            currentPlayer.YPosition = 5;
            _sut.UpdateCharacterPosition(currentPlayer.Id, currentPlayer.XPosition, currentPlayer.YPosition);

            //Assert ---------
            Assert.That(_sut.getCurrentPlayer().XPosition.Equals(2));
            Assert.That(_sut.getCurrentPlayer().YPosition.Equals(5));
        }

        [Test]
        public void Test_updateRightCharacter()
        {
            //Arrange
            _sut.AddPlayerToWorld(_mapCharacterDTOPlayer, false);
            _sut.AddPlayerToWorld(_mapCharacterDTOOtherPlayer, true);

            //Act
            _sut.UpdateCharacterPosition(_mapCharacterDTOOtherPlayer.Id, 15, 20);

            //Assert
            Assert.That(_sut.GetWorld().CurrentPlayer.XPosition != 15);
            Assert.That(_sut.GetWorld().CurrentPlayer.YPosition != 20);
        }

        
    }
}