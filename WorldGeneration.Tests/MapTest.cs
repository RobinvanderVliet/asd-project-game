using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Moq;
using WorldGeneration.Models;
using WorldGeneration.Models.Interfaces;
using WorldGeneration.Models.TerrainTiles;

namespace WorldGeneration.Tests
{
    
    [ExcludeFromCodeCoverage]  
    [TestFixture]
    public class MapTest
    {
        //Declaration and initialisation of constant variables
 
        //Declaration of variables
        private Map _sut;
 
        //Declaration of mocks
        private INoiseMapGenerator _noiseMapGeneratorMock;
 
        [SetUp]
        public void Setup()
        {
            //Initialisation of variables
            var chunkSize = 2;
            var seed = 5;
            

            var map1 = new ITile[] {new GrassTile(), new GrassTile(), new GrassTile(), new GrassTile()};
            var map2 = new ITile[] {new StreetTile(), new StreetTile(), new StreetTile(), new StreetTile()};
            var map3 = new ITile[] {new WaterTile(), new WaterTile(), new WaterTile(), new WaterTile()};
            var map4 = new ITile[] {new DirtTile(), new DirtTile(), new DirtTile(), new DirtTile()};
            var chunk1 = new Chunk(0, 0, map1, chunkSize);
            var chunk2 = new Chunk(-1, 0, map2, chunkSize);
            var chunk3 = new Chunk(0, -1, map3, chunkSize);
            var chunk4 = new Chunk(-1, -1, map4, chunkSize);
            
            //Initialisation of mocks
            var noiseMapGeneratorMock = new Mock<INoiseMapGenerator>();
            noiseMapGeneratorMock.Setup(p => p.GenerateChunk(0,0, 2, seed)).Returns(chunk1);
            noiseMapGeneratorMock.Setup(p => p.GenerateChunk(-1,0, 2, seed)).Returns(chunk2);
            noiseMapGeneratorMock.Setup(p => p.GenerateChunk(0,-1, 2, seed)).Returns(chunk3);
            noiseMapGeneratorMock.Setup(p => p.GenerateChunk(-1,-1, 2, seed)).Returns(chunk4);
            
            _noiseMapGeneratorMock = noiseMapGeneratorMock.Object;

            _sut = new Map(_noiseMapGeneratorMock, new Database.Database("c:\\temp\\db.db", "test"), chunkSize, seed);

        }
        
        [Test]
        public void Test_Map_DoesntThrowException() 
        {
            //Arrange ---------
            //Act ---------
            var map = new Map(new NoiseMapGenerator(), new Database.Database("c:\\temp\\db.db", "test"),2,51);
            //Assert ---------
        }
        
        [Test]
        public void Test_DisplayMap_DoesntThrowException() 
        {
            //Arrange ---------
            //Act ---------
            _sut.DisplayMap(0,0, 1);
            //Assert ---------
        }
        
        [Test]
        public void Test_DisplayMap_DoesntLoadTooBigArea() 
        {
            //Arrange ---------
            //Act ---------
            _sut.DisplayMap(0,0, 1);
            //Assert ---------
        }
        
        [Test]
        public void Test_DisplayMap_DoesntLoadTooSmallArea() 
        {
            //Arrange ---------
            //Act ---------
            //Assert ---------
        }
    }
}