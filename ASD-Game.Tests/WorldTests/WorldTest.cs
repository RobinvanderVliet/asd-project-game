using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Moq;
using NUnit.Framework;
using UserInterface;

namespace WorldGeneration.Tests
{
    [ExcludeFromCodeCoverage]  
    [TestFixture]
    public class WorldTest
    {
        //Declaration and initialisation of constant variables
 
        //Declaration of variables
        private Player _friendlyPlayer;
        private Player _enemyPlayer;
        private Creature _creature;
 
        //Declaration of mocks
        private IMapFactory _mapFactoryMockObject;
        private Mock<IMapFactory> _mapFactoryMock;
        private IMap _mapMockObject;
        private Mock<IMap> _mapMock;
        private Mock<IScreenHandler> _screenHandlerMock;
        private IScreenHandler _screenHandlerMockObject;

        private World _sut;


        [SetUp]
        public void Setup()
        {
            //Initialisation of variables
            _friendlyPlayer = new Player("A", 0, 0, "#", "1");
            _enemyPlayer = new Player("B", 0, 1, "!", "2");
            _creature = new Creature("B", 1, 0, "!", "2");
            //Initialisation of mocks
            _mapMock = new Mock<IMap>();
            _mapMock.Setup(Map => Map.DeleteMap()).Verifiable();
            _mapMockObject = _mapMock.Object;
            
            _mapFactoryMock = new Mock<IMapFactory>();
            _mapFactoryMock.Setup(mapFactory => mapFactory.GenerateMap(It.IsAny<int>())).Returns(_mapMockObject).Verifiable();
            _mapFactoryMock.Setup(mapFactory => mapFactory.GenerateSeed()).Returns(11246).Verifiable();
            _mapFactoryMockObject = _mapFactoryMock.Object;

            _screenHandlerMock = new Mock<IScreenHandler>();
            // _screenHandlerMock.Setup(handler => handler.UpdateWorld(It.IsAny<Char[,]>())).Verifiable();
            _screenHandlerMockObject = _screenHandlerMock.Object;

            _sut = new World(5, 2, _mapFactoryMockObject, _screenHandlerMockObject);
        }
        
        [Test]
        public void Test_WorldConstructor_SetsMapUpCorrectly() 
        {
            //Arrange ---------
            var seed = 10;
            //Act ---------
            //Assert ---------
            Assert.DoesNotThrow(() =>
            {
                var world = new World(seed, 55, _mapFactoryMockObject, _screenHandlerMockObject);
            });
            _mapFactoryMock.Verify(mapFactory => mapFactory.GenerateMap(seed), Times.Once);
        }

        [Test]
        public void Test_DisplayWorld_DoesNothingWithoutCurrentPlayer() 
        {
            //Arrange ---------
            //Act ---------
            _sut.UpdateMap();
            //Assert ---------
            _mapMock.Verify(map => map.GetMapAroundCharacter(It.IsAny<Player>(), It.IsAny<int>(), It.IsAny<List<Character>>()), Times.AtMost(0));
        }
        
        [Test]
        public void Test_UpdateMap_CallsGetMapAroundCharacterWhenGivenCharacters() 
        {
            //Arrange ---------
            //Act ---------
            _sut.AddPlayerToWorld(_friendlyPlayer, true);
            _sut.UpdateMap();
            //Assert ---------
            _mapMock.Verify(map => map.GetMapAroundCharacter(It.IsAny<Player>(), It.IsAny<int>(), It.IsAny<List<Character>>()), Times.Exactly(2));

        }
        
        [Test]
        public void Test_AddCharacterToWorld_AddsCurrentPlayerCharacter() 
        {
            //Arrange ---------
            //Act ---------
            _sut.AddPlayerToWorld(_friendlyPlayer, true);
            _sut.AddPlayerToWorld(_enemyPlayer, false);
            //Assert ---------
            Assert.That(_sut.CurrentPlayer == _friendlyPlayer);
        }
        
        [Test]
        public void Test_AddCharacterToWorld_AddsNotCurrentPlayerCharacter() 
        {
            //Arrange ---------
            //Act ---------
            _sut.AddPlayerToWorld(_friendlyPlayer, true);
            _sut.AddPlayerToWorld(_enemyPlayer, false);
            //Assert ---------
            Assert.That(_sut.CurrentPlayer != _enemyPlayer);
        }
        
        [Test]
        public void Test_UpdateCharacterPosition_UpdatesCurrentPlayer() 
        {
            //Arrange ---------
            _sut.AddPlayerToWorld(_friendlyPlayer, true);
            _sut.AddPlayerToWorld(_enemyPlayer, false);
            _enemyPlayer.XPosition = 2;
            //Act ---------
            _sut.UpdateCharacterPosition(_friendlyPlayer.Id, _friendlyPlayer.XPosition, _friendlyPlayer.YPosition);
            //Assert ---------
            Assert.That(_sut.CurrentPlayer.XPosition == _friendlyPlayer.XPosition);
        }
        
        [Test]
        public void Test_UpdateCharacterPosition_UpdatesOtherPlayer() 
        {
            //Arrange ---------
            _sut.AddPlayerToWorld(_friendlyPlayer, true);
            _sut.AddPlayerToWorld(_enemyPlayer, false);
            _enemyPlayer.XPosition = 2;
            //Act ---------
            _sut.UpdateCharacterPosition(_enemyPlayer.Id, _enemyPlayer.XPosition, _enemyPlayer.YPosition);
            //Assert ---------
            Assert.That(_sut.CurrentPlayer.XPosition != _enemyPlayer.XPosition);
        }
        
        [Test]
        public void Test_DeleteMap_PassesThroughDeleteCommand() 
        {
            //Arrange ---------
            _sut.AddPlayerToWorld(_friendlyPlayer, true);
            _sut.AddPlayerToWorld(_enemyPlayer, false);
            _enemyPlayer.XPosition = 2;
            //Act ---------
            _sut.UpdateCharacterPosition(_enemyPlayer.Id, _enemyPlayer.XPosition, _enemyPlayer.YPosition);
            //Assert ---------
            Assert.That(_sut.CurrentPlayer.XPosition != _enemyPlayer.XPosition);
        }
        
        [Test]
        public void Test_AddCreatureToWorld_AddsCreature() 
        {
            //Arrange ---------
            _sut.AddCreatureToWorld(_creature);
            //Act ---------
            _sut.UpdateCharacterPosition(_creature.Id, _creature.XPosition, _creature.YPosition);
            //Assert ---------
            Assert.That(_sut.CurrentPlayer.XPosition != _enemyPlayer.XPosition);
        }
    }
}