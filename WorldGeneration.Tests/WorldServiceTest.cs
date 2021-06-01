using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using DataTransfer.DTO.Character;
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
        private MapCharacterDTO _mapCharacterDTOPlayer;
        private MapCharacterDTO _mapCharacterDTOOtherPlayer;

        //Declaration of mocks
        private IMapFactory _mapFactoryMockObject;
        private Mock<IMapFactory> _mapFactoryMock;
        private IMap _mapMockObject;
        private Mock<IMap> _mapMock;

        
        private WorldService _sut;
        private World _world;
        
        [SetUp]
        public void Setup()
        {
            //Initialisation of variables
            _sut = new WorldService();
            _world = new World(42, 6, new MapFactory());
            _mapCharacterDTOPlayer = new CharacterDTO(0, 0, "b", "b", "#");
            _mapCharacterDTOOtherPlayer = new CharacterDTO(12, 12, "a", "r", "!");
            
            //Initialisation of mocks
            _mapMock = new Mock<IMap>();
            _mapMock.Setup(Map => Map.DeleteMap()).Verifiable();
            _mapMockObject = _mapMock.Object;

            _mapFactoryMock = new Mock<IMapFactory>();
            _mapFactoryMock.Setup(mapFactory => mapFactory.GenerateMap(It.IsAny<int>())).Returns(_mapMockObject).Verifiable();
            _mapFactoryMock.Setup(mapFactory => mapFactory.GenerateSeed()).Returns(11246).Verifiable();
            _mapFactoryMockObject = _mapFactoryMock.Object;
        }
        
        [Test]
        public void Test_GenerateWorldWithSeed() 
        {
            //Arrange ---------
            int seed = 42;
            //Act ---------
            _sut.GenerateWorld(seed);
            //Assert ---------
            Assert.That(_sut.GetWorld().Equals(_world));
        }
        
        [Test]
        public void Test_getCurrentCharacterPositions() 
        {
            //Arrange ---------
            _world.CurrentPlayer = _mapCharacterDTOPlayer;

            //Act ---------
            _sut.AddCharacterToWorld(_mapCharacterDTOPlayer, true);

            //Assert ---------
            Assert.That(_sut.getCurrentCharacterPositions().Equals(_world.CurrentPlayer));
        }

        [Test]
        public void Test_updateCharacterPositions()
        {
            //Arrange ---------
            _world.CurrentPlayer = _mapCharacterDTOPlayer;
            _sut.AddCharacterToWorld(_mapCharacterDTOPlayer, true);

            //Act ---------
            _mapCharacterDTOPlayer.XPosition = 2;
            _mapCharacterDTOPlayer.YPosition = 5;
            _sut.UpdateCharacterPosition(_mapCharacterDTOPlayer);

            //Assert ---------
            Assert.That(_mapCharacterDTOPlayer.XPosition == 2);
            Assert.That(_mapCharacterDTOPlayer.YPosition == 5);
        }

        [Test]
        public void Test_updateRightCharacter()
        {
            //Arrange
            _sut.AddCharacterToWorld(_mapCharacterDTOPlayer, false);
            _sut.AddCharacterToWorld(_mapCharacterDTOOtherPlayer, true);

            //Act
            _sut.UpdateCharacterPosition(_mapCharacterDTOOtherPlayer);

            //Assert
            Assert.That(_sut.GetWorld().CurrentPlayer.XPosition != _mapCharacterDTOOtherPlayer.XPosition);
            Assert.That(_sut.GetWorld().CurrentPlayer.YPosition != _mapCharacterDTOOtherPlayer.YPosition);
        }

        [Test]
        public void Test_DisplayWorld_DoesNothingWithoutCharacters()
        {
            //Arrange ---------
            //Act ---------
            _sut.DisplayWorld();
            //Assert ---------
            _mapMock.Verify(map => map.DisplayMap(It.IsAny<MapCharacterDTO>(), It.IsAny<int>(), It.IsAny<IList<MapCharacterDTO>>()), Times.AtMost(0));
        }

        [Test]
        public void Test_DisplayWorld_CallsDisplayMapWhenGivenCharacters()
        {
            //Arrange ---------
            _sut.AddCharacterToWorld(_mapCharacterDTOPlayer, true);
            //Act ---------
            _sut.DisplayWorld();
            //Assert ---------
            _mapMock.Verify(map => map.DisplayMap(It.IsAny<MapCharacterDTO>(), It.IsAny<int>(), It.IsAny<IList<MapCharacterDTO>>()), Times.Once);
        }
                    
        
    }
}