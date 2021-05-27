using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using DatabaseHandler.Services;
using DataTransfer.DTO.Character;
using DataTransfer.Model.World;
using DataTransfer.Model.World.Interfaces;
using DataTransfer.Model.World.LootableTiles;
using DataTransfer.Model.World.TerrainTiles;
using Display;
using NUnit.Framework;
using Moq;
//using WorldGeneration.DatabaseFunctions;

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
        private IDatabaseService<Chunk> _databaseServiceMock;
        private MapCharacterDTO _mapCharacterDTO;
        private List<MapCharacterDTO> _mapCharacterDTOList;
        private IConsolePrinter _consolePrinterMock;
        private IList<Chunk> _chunks;
 
        [SetUp]
        public void Setup()
        {
            //Initialisation of variables
            var chunkSize = 2;
            var seed = 5;
            

            var map1 = new ITile[] {new GrassTile(1,1), new GrassTile(1,2), new GrassTile(1,3), new GrassTile(1,4)};
            var map2 = new ITile[] {new StreetTile(1,1), new StreetTile(1,2), new StreetTile(1,3), new StreetTile(1,4)};
            var map3 = new ITile[] {new WaterTile(1,1), new WaterTile(1,2), new WaterTile(1,3), new WaterTile(1,4)};
            var map4 = new ITile[] {new DirtTile(1,1), new DirtTile(1,2), new DirtTile(1,3), new DirtTile(1,4)};
            var map5 = new ITile[] {new DirtTile(1,1), new DirtTile(1,2), new DirtTile(1,3), new DirtTile(1,4)};
            var chunk1 = new Chunk(0, 0, map1, chunkSize);
            var chunk2 = new Chunk(-1, 0, map2, chunkSize);
            var chunk3 = new Chunk(0, -1, map3, chunkSize);
            var chunk4 = new Chunk(-1, -1, map4, chunkSize);
            _chunks = new List<Chunk>() {} ;

            //Initialisation of mocks
            var noiseMapGeneratorMock = new Mock<INoiseMapGenerator>();
            noiseMapGeneratorMock.Setup(p => p.GenerateChunk(0,0, 2, seed)).Returns(chunk1);
            noiseMapGeneratorMock.Setup(p => p.GenerateChunk(-1,0, 2, seed)).Returns(chunk2);
            noiseMapGeneratorMock.Setup(p => p.GenerateChunk(0,-1, 2, seed)).Returns(chunk3);
            noiseMapGeneratorMock.Setup(p => p.GenerateChunk(-1,-1, 2, seed)).Returns(chunk4);
            noiseMapGeneratorMock.Setup(p => p.GenerateChunk(It.IsAny<int>(),It.IsAny<int>(), 2, seed))
                .Returns((int x, int y, int size, int seed) => new Chunk(x, y, map5, chunkSize));
            
            _noiseMapGeneratorMock = noiseMapGeneratorMock.Object;
            var databaseMock = new Mock<IDatabaseService<Chunk>>();
            databaseMock.Setup(p => p.CreateAsync(It.IsAny<Chunk>())).ReturnsAsync((Chunk item) =>
            {
                _chunks.Add(item);
                return true;
            });
            databaseMock.Setup(p => p.GetAllAsync()).ReturnsAsync(_chunks);
            databaseMock.Setup(a => a.DeleteAllAsync()).Verifiable();
            
            _databaseServiceMock = databaseMock.Object;

            var consolePrinterMock = new Mock<IConsolePrinter>();
            _consolePrinterMock = consolePrinterMock.Object;
            
            _sut = new Map(_noiseMapGeneratorMock, chunkSize, seed, _databaseServiceMock, _consolePrinterMock, _chunks);

            _mapCharacterDTO = new MapCharacterDTO(0, 0, "naam", "d");
            
            _mapCharacterDTOList = new List<MapCharacterDTO>();
            _mapCharacterDTOList.Add(_mapCharacterDTO);
        }
        
        [Test]
        public void Test_MapConstructor_DoesntThrowException() 
        {
            //Arrange ---------
            //Act ---------
            var map = new Map(new NoiseMapGenerator(),2,51, _databaseServiceMock,new ConsolePrinter(), _chunks);
            //Assert ---------
        }
        
        [Test]
        public void Test_DisplayMap_DoesntThrowException() 
        {
            //Arrange ---------
            //Act ---------
            var ab = _chunks;
            _sut.DisplayMap(_mapCharacterDTO,1, _mapCharacterDTOList);
            //Assert ---------
        }
        
        [Test]
        public void Test_DisplayMap_DoesntLoadTooBigArea() 
        {
            //Arrange ---------
            //Act ---------
            //_sut.DisplayMap(0,0, 1);
            //Assert ---------
        }
        
        [Test]
        public void Test_DisplayMap_DoesntLoadTooSmallArea() 
        {
            //Arrange ---------
            //Act ---------
            //Assert ---------
        }
        
        [Test]
        public void Test_DeleteMap_PassesCommandThrough() 
        {
            //Arrange ---------
            //Act ---------
            _sut.DeleteMap();
            //Assert ---------
            //_databaseServiceMock.ver(_sut.DeleteMap(), Times.AtLeastOnce());
        }
    }
}