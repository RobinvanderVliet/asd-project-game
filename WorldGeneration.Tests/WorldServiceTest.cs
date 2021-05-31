using System.Diagnostics.CodeAnalysis;
using DataTransfer.DTO.Character;
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

        //Declaration of mocks
        private WorldService _sut;
        private World _world;
        
        [SetUp]
        public void Setup()
        {
            //Initialisation of variables
            //Initialisation of mocks
            _sut = new WorldService();
            _world = new World(42, 6, new MapFactory());
            _mapCharacterDTOPlayer = new CharacterDTO(0, 0, "b", "b", "#");
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
        public void Test_getCurrentCharacterPositions()
        {
            //Arrange ---------
            _world.CurrentPlayer = _mapCharacterDTOPlayer;

            //Act ---------
            _sut.AddCharacterToWorld(_mapCharacterDTOPlayer, true);

            //Assert ---------
            Assert.That(_sut.getCurrentCharacterPositions().Equals(_world.CurrentPlayer));
        }

    }
}