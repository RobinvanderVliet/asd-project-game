using NUnit.Framework;
using Moq;
using System.Diagnostics.CodeAnalysis;
using Items;
using WorldGeneration.Models;
using WorldGeneration.Models.Interfaces;
using WorldGeneration.Models.LootableTiles;
using WorldGeneration.Models.TerrainTiles;

namespace WorldGeneration.Tests
{
    [ExcludeFromCodeCoverage]  
    [TestFixture]
    public class ItemSpawnerTest
    {
        
        //Declaration and initialisation of constant variables
 
        //Declaration of variables
        private ItemSpawner sut;
        private Chunk _chunk;
        
        //Declaration of mocks
        private Mock<IFastNoise> _mockedNoise;
        private IFastNoise _mockedNoiseObject;
       
        
        [SetUp]
        public void Setup()
        {
            //Initialisation of variables
            _chunk = new Chunk(0,0,new ITile[4],2);
            _chunk.X = 0;
            _chunk.Y = 0;

            //Initialisation of mocks
            _mockedNoise = new Mock<IFastNoise>();

            // _mockedNoise.Setup();
            _mockedNoiseObject = _mockedNoise.Object;
            
            sut = new ItemSpawner(_mockedNoiseObject);

        }

        [Test]
        public void Test_Spawn_CreateChestTileOnRandomCoordinateInChunk()
        {
            //Arrange ---------
            _mockedNoise.Setup(x => x.GetNoise(_chunk.X, _chunk.Y))
            .Returns((float) 0.5).Verifiable();

            for (int i = 0; i < _chunk.Map.Length; i++)
            {
                _chunk.Map[i] = new GrassTile(i, i);
            }
            
            //Act ---------
            sut.Spawn(_chunk);
            
            //Assert ---------
            Assert.IsInstanceOf<ChestTile>(_chunk.Map[1]);
        }

        [Test]
        public void Test_Spawn_CheckIfChestIsFilledWithItem()
        {
            //Arrange ---------
            _mockedNoise.Setup(x => x.GetNoise(_chunk.X, _chunk.Y))
                .Returns((float) 0.5).Verifiable();

            for (int i = 0; i < _chunk.Map.Length; i++)
            {
                _chunk.Map[i] = new GrassTile(i, i);
            }

            //Act ---------
            sut.Spawn(_chunk);

            //Assert ---------
            Assert.IsInstanceOf<Item>(_chunk.Map[1].ItemsOnTile[0]);
        }


    }
}