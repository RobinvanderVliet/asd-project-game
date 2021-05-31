using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using DatabaseHandler.Services;
using DataTransfer.DTO.Character;
using Display;
using NUnit.Framework;
using Moq;
using WorldGeneration.Models;
using WorldGeneration.Models.Interfaces;
using WorldGeneration.Models.TerrainTiles;
using Range = Moq.Range;

//using WorldGeneration.DatabaseFunctions;

namespace WorldGeneration.Tests
{
    
    [ExcludeFromCodeCoverage]  
    [TestFixture]
    public class MapTest
    {
        //Declaration and initialisation of constant variables
 
        //Declaration of variables
        private int _seed;
        private int _chunkSize;
        private List<MapCharacterDTO> _mapCharacterDTOList;
        private MapCharacterDTO _mapCharacter1DTO;
        private MapCharacterDTO _mapCharacter2DTO;
        private IList<Chunk> _chunks;
        private Map _sut;
        
        //Declaration of mocks
        private INoiseMapGenerator _noiseMapGeneratorMockObject;
        private IDatabaseService<Chunk> _databaseServiceMockObject;
        private IConsolePrinter _consolePrinterMockObject;
        private Mock<INoiseMapGenerator> _noiseMapGeneratorMock;
        private Mock<IDatabaseService<Chunk>> _databaseServiceMock;
        private Mock<IConsolePrinter> _consolePrinterMock;
        
 
        [SetUp]
        public void Setup()
        {
            //Initialisation of variables
            _seed = 5;
            _chunkSize = 2;
            
            var map1 = new ITile[] {new GrassTile(1,1), new GrassTile(1,2), new GrassTile(1,3), new GrassTile(1,4)};
            var map2 = new ITile[] {new StreetTile(1,1), new StreetTile(1,2), new StreetTile(1,3), new StreetTile(1,4)};
            var map3 = new ITile[] {new WaterTile(1,1), new WaterTile(1,2), new WaterTile(1,3), new WaterTile(1,4)};
            var map4 = new ITile[] {new DirtTile(1,1), new DirtTile(1,2), new DirtTile(1,3), new DirtTile(1,4)};
            var map5 = new ITile[] {new DirtTile(1,1), new DirtTile(1,2), new DirtTile(1,3), new DirtTile(1,4)};
            var chunk1 = new Chunk(0, 0, map1, _chunkSize);
            var chunk2 = new Chunk(-1, 0, map2, _chunkSize);
            var chunk3 = new Chunk(0, -1, map3, _chunkSize);
            var chunk4 = new Chunk(-1, -1, map4, _chunkSize);
            _chunks = new List<Chunk>() {chunk1, chunk2, chunk3, chunk4} ;

            //Initialisation of mocks
            _noiseMapGeneratorMock = new Mock<INoiseMapGenerator>();
            _noiseMapGeneratorMock.Setup(noiseMapGenerator => noiseMapGenerator.GenerateChunk(0,0, 2, _seed)).Returns(chunk1).Verifiable();
            _noiseMapGeneratorMock.Setup(noiseMapGenerator => noiseMapGenerator.GenerateChunk(-1,0, 2, _seed)).Returns(chunk2).Verifiable();
            _noiseMapGeneratorMock.Setup(noiseMapGenerator => noiseMapGenerator.GenerateChunk(0,-1, 2, _seed)).Returns(chunk3).Verifiable();
            _noiseMapGeneratorMock.Setup(noiseMapGenerator => noiseMapGenerator.GenerateChunk(-1,-1, 2, _seed)).Returns(chunk4).Verifiable();
            _noiseMapGeneratorMock.Setup(noiseMapGenerator => noiseMapGenerator.GenerateChunk(It.IsAny<int>(),It.IsAny<int>(), 2, _seed))
                .Returns((int x, int y, int size, int seed) => new Chunk(x, y, map5, _chunkSize)).Verifiable();
            _noiseMapGeneratorMockObject = _noiseMapGeneratorMock.Object;
            
            _databaseServiceMock = new Mock<IDatabaseService<Chunk>>();
            _databaseServiceMock.Setup(databaseService => databaseService.CreateAsync(It.IsAny<Chunk>())).Verifiable();
            _databaseServiceMock.Setup(databaseService => databaseService.GetAllAsync()).ReturnsAsync(_chunks);
            _databaseServiceMock.Setup(databaseService => databaseService.DeleteAllAsync()).Verifiable();
            _databaseServiceMockObject = _databaseServiceMock.Object;

            _consolePrinterMock = new Mock<IConsolePrinter>();
            _consolePrinterMock.Setup(consolePrinter => consolePrinter.PrintText(It.IsAny<string>())).Verifiable();
            _consolePrinterMock.Setup(consolePrinter => consolePrinter.NextLine()).Verifiable();
            _consolePrinterMockObject = _consolePrinterMock.Object;
            

            _mapCharacter1DTO = new MapCharacterDTO(0, 0, "naam1", "d", CharacterSymbol.FRIENDLY_PLAYER);
            _mapCharacter2DTO = new MapCharacterDTO(0, 0, "naam2", "a", CharacterSymbol.FRIENDLY_PLAYER);
            
            _mapCharacterDTOList = new List<MapCharacterDTO>();
            _mapCharacterDTOList.Add(_mapCharacter1DTO);
            _mapCharacterDTOList.Add(_mapCharacter2DTO);
            
            _sut = new Map(_noiseMapGeneratorMockObject, _chunkSize, _seed, _consolePrinterMockObject, _databaseServiceMockObject);
        }
        
        [Test]
        public void Test_MapConstructor_DoesntThrowException() 
        {
            //Arrange ---------
            //Act ---------
            //Assert ---------
            Assert.DoesNotThrow(() =>
            {
                var map = new Map(_noiseMapGeneratorMockObject,21,51, _consolePrinterMockObject, _databaseServiceMockObject, _chunks);
            });
        }
        
        [Test]
        public void Test_DisplayMap_DoesntThrowException() 
        {
            //Arrange ---------
            //Act ---------
            //Assert ---------
            Assert.DoesNotThrow(() =>
            {
                _sut.DisplayMap(_mapCharacter1DTO, 1, _mapCharacterDTOList);
            });
        }
        
        [Test]
        public void Test_DisplayMap_DisplaysRightSize() 
        {
            //Arrange ---------
            //Act ---------
            _sut.DisplayMap(_mapCharacter1DTO,2, _mapCharacterDTOList);
            //Assert ---------
            _consolePrinterMock.Verify(consolePrinterMock => consolePrinterMock.PrintText(It.IsAny<string>()), Times.Exactly(25));

        }
        
        [Test]
        public void Test_DisplayMap_DoesntLoadTooBigArea() 
        {
            //Arrange ---------
            var viewDistance = 2;
            var maxLoadingLimit = (int)(Math.Pow(viewDistance, 4)  * _chunkSize / _chunkSize * 4);
            //Act ---------
            _sut.DisplayMap(_mapCharacter1DTO,viewDistance, _mapCharacterDTOList);
            //Assert ---------
            _databaseServiceMock.Verify(databaseService => databaseService.CreateAsync(It.IsAny<Chunk>()), Times.Between(0, maxLoadingLimit, Range.Inclusive));
        }
        
        [Test]
        public void Test_DeleteMap_PassesCommandThrough() 
        {
            //Arrange ---------
            _sut.DisplayMap(_mapCharacter1DTO,1, _mapCharacterDTOList);
            //Act ---------
            _sut.DeleteMap();
            //Assert ---------
            _databaseServiceMock.Verify( databaseService => databaseService.DeleteAllAsync(), Times.Once);
        }
        
        [Test]
        public void Test_MapConstructor_ThrowsWhenGivenNegativeChunkSize() 
        {
            //Arrange ---------
            //Act ---------
            //Assert ---------
            Assert.Throws<InvalidOperationException>(() =>
            {
                var map = new Map(_noiseMapGeneratorMockObject,-21,51, _consolePrinterMockObject, _databaseServiceMockObject, _chunks);
            });
        }
        
        [Test]
        public void Test_DisplayMap_ThrowsWhenGivenNegativeDisplaySize() 
        {
            //Arrange ---------
            //Act ---------
            //Assert ---------
            Assert.Throws<InvalidOperationException>(() =>
            {
                _sut.DisplayMap(_mapCharacter1DTO,-1, _mapCharacterDTOList);
            });
        }
        
        [Test]
        public void Test_DisplayMap_UsesChunksIfTheyAreFoundInDatabase() 
        {
            //Arrange ---------
            //Act ---------
            _sut.DisplayMap(_mapCharacter1DTO,2, _mapCharacterDTOList);
            //Assert ---------
            _consolePrinterMock.Verify( consolePrinter => consolePrinter.PrintText(" " + _chunks[0].Map[0].Symbol), Times.AtLeast(1));
            _consolePrinterMock.Verify( consolePrinter => consolePrinter.PrintText(" " + _chunks[1].Map[0].Symbol), Times.AtLeast(1));
            _consolePrinterMock.Verify( consolePrinter => consolePrinter.PrintText(" " + _chunks[2].Map[0].Symbol), Times.AtLeast(1));
            _consolePrinterMock.Verify( consolePrinter => consolePrinter.PrintText(" " + _chunks[3].Map[0].Symbol), Times.AtLeast(1));
        }
    }
}