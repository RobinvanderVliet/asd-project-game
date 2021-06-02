using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Moq;
using NUnit.Framework;

namespace WorldGeneration.Tests
{
    [ExcludeFromCodeCoverage]  
    [TestFixture]
    public class WorldTest
    {
        //Declaration and initialisation of constant variables
 
        //Declaration of variables
        private Player _mapCharacterDTOPlayer;
        private Player _mapCharacterDTOEnemy;
 
        //Declaration of mocks
        private IMapFactory _mapFactoryMockObject;
        private Mock<IMapFactory> _mapFactoryMock;
        private IMap _mapMockObject;
        private Mock<IMap> _mapMock;

        private World _sut;


        [SetUp]
        public void Setup()
        {
            //Initialisation of variables
            _mapCharacterDTOPlayer = new Player("A", 0, 0, "#", "1");
            _mapCharacterDTOEnemy = new Player("B", 0, 0, "!", "2");
            //Initialisation of mocks
            _mapMock = new Mock<IMap>();
            _mapMock.Setup(Map => Map.DeleteMap()).Verifiable();
            _mapMockObject = _mapMock.Object;
            
            _mapFactoryMock = new Mock<IMapFactory>();
            _mapFactoryMock.Setup(mapFactory => mapFactory.GenerateMap(It.IsAny<int>())).Returns(_mapMockObject).Verifiable();
            _mapFactoryMock.Setup(mapFactory => mapFactory.GenerateSeed()).Returns(11246).Verifiable();
            _mapFactoryMockObject = _mapFactoryMock.Object;

            _sut = new World(5, 2, _mapFactoryMockObject);
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
                var world = new World(seed, 55, _mapFactoryMockObject);
            });
            _mapFactoryMock.Verify(mapFactory => mapFactory.GenerateMap(seed), Times.AtLeastOnce);
            _mapMock.Verify(map => map.DeleteMap(), Times.Exactly(2));
        }

        [Test]
        public void Test_DisplayWorld_DoesNothingWithoutCharacters() 
        {
            //Arrange ---------
            //Act ---------
            _sut.DisplayWorld();
            //Assert ---------
            _mapMock.Verify(map => map.DisplayMap(It.IsAny<Player>(), It.IsAny<int>(), It.IsAny<List<Player>>()), Times.AtMost(0));
        }
        
        [Test]
        public void Test_DisplayWorld_CallsDisplayMapWhenGivenCharacters() 
        {
            //Arrange ---------
            //Act ---------
            _sut.AddPlayerToWorld(_mapCharacterDTOPlayer, true);
            _sut.DisplayWorld();
            //Assert ---------
            _mapMock.Verify(map => map.DisplayMap(It.IsAny<Player>(), It.IsAny<int>(), It.IsAny<List<Player>>()), Times.Once);

        }
        
        [Test]
        public void Test_AddCharacterToWorld_AddsNotCurrentPlayerCharacter() 
        {
            //Arrange ---------
            //Act ---------
            _sut.AddPlayerToWorld(_mapCharacterDTOPlayer, true);
            _sut.AddPlayerToWorld(_mapCharacterDTOEnemy, false);
            //Assert ---------
            Assert.That(_sut.CurrentPlayer != _mapCharacterDTOEnemy);
        }
        
        [Test]
        public void Test_AddCharacterToWorld_AddsCurrentPlayerCharacter() 
        {
            //Arrange ---------
            //Act ---------
            _sut.AddPlayerToWorld(_mapCharacterDTOPlayer, true);
            _sut.AddPlayerToWorld(_mapCharacterDTOEnemy, false);
            //Assert ---------
            Assert.That(_sut.CurrentPlayer == _mapCharacterDTOPlayer);
        }
        
        [Test]
        public void Test_UpdateCharacterPosition_UpdatesCurrentPlayer() 
        {
            //Arrange ---------
            _sut.AddPlayerToWorld(_mapCharacterDTOPlayer, true);
            _sut.AddPlayerToWorld(_mapCharacterDTOEnemy, false);
            _mapCharacterDTOEnemy.XPosition = 2;
            //Act ---------
            _sut.UpdateCharacterPosition(_mapCharacterDTOPlayer.Id, _mapCharacterDTOPlayer.XPosition, _mapCharacterDTOPlayer.YPosition);
            //Assert ---------
            Assert.That(_sut.CurrentPlayer.XPosition == _mapCharacterDTOPlayer.XPosition);
        }
        
        [Test]
        public void Test_UpdateCharacterPosition_UpdatesOtherPlayer() 
        {
            //Arrange ---------
            _sut.AddPlayerToWorld(_mapCharacterDTOPlayer, true);
            _sut.AddPlayerToWorld(_mapCharacterDTOEnemy, false);
            _mapCharacterDTOEnemy.XPosition = 2;
            //Act ---------
            _sut.UpdateCharacterPosition(_mapCharacterDTOEnemy.Id, _mapCharacterDTOEnemy.XPosition, _mapCharacterDTOEnemy.YPosition);
            //Assert ---------
            Assert.That(_sut.CurrentPlayer.XPosition != _mapCharacterDTOEnemy.XPosition);
        }
    }
}