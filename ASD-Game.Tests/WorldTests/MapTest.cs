using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ASD_Game.Items;
using ASD_Game.World;
using ASD_Game.World.Models;
using ASD_Game.World.Models.Characters;
using ASD_Game.World.Models.Interfaces;
using ASD_Game.World.Models.TerrainTiles;
using NUnit.Framework;
using Moq;
using Range = Moq.Range;

namespace ASD_Game.Tests.WorldTests
{
    
    [ExcludeFromCodeCoverage]  
    [TestFixture]
    public class MapTest
    {
        //Declaration and initialisation of constant variables
 
        //Declaration of variables
        private int _chunkSize;
        private List<Character> _characterList;
        private Player _character1;
        private Player _character2;
        private IList<Chunk> _chunks;
        private Map _sut;
        
        //Declaration of mocks
        private INoiseMapGenerator _noiseMapGeneratorMockObject;
        private Mock<INoiseMapGenerator> _noiseMapGeneratorMock;


        [SetUp]
        public void Setup()
        {
            //Initialisation of variables
            _chunkSize = 2;

            var tileWithItem = new GrassTile(1, -1);
            tileWithItem.ItemsOnTile.Add(new Armor());
            
            var map1 = new ITile[] {new GrassTile(0,0), tileWithItem, new GrassTile(0,0), new GrassTile(1,-1)};
            var map2 = new ITile[] {new StreetTile(-2,0), new StreetTile(-1,-1), new StreetTile(-2,0), new StreetTile(-2,-1)};
            var map3 = new ITile[] {new WaterTile(0,-2), new WaterTile(1,-2), new WaterTile(0,-2), new WaterTile(1,-3)};
            var map4 = new ITile[] {new DirtTile(-2,-2), new DirtTile(-1,-2), new DirtTile(-2,-2), new DirtTile(-1,-2)};
            var map5 = new ITile[] {new DirtTile(1,1), new DirtTile(1,2), new DirtTile(1,3), new DirtTile(1,4)};

            var chunk1 = new Chunk(0, 0, map1, _chunkSize);
            var chunk2 = new Chunk(-1, 0, map2, _chunkSize);
            var chunk3 = new Chunk(0, -1, map3, _chunkSize);
            var chunk4 = new Chunk(-1, -1, map4, _chunkSize);
            _chunks = new List<Chunk>() {chunk1, chunk2, chunk3, chunk4} ;

            //Initialisation of mocks
            _noiseMapGeneratorMock = new Mock<INoiseMapGenerator>();
            _noiseMapGeneratorMock.Setup(noiseMapGenerator => noiseMapGenerator.GenerateChunk(0,0, 2)).Returns(chunk1).Verifiable();
            _noiseMapGeneratorMock.Setup(noiseMapGenerator => noiseMapGenerator.GenerateChunk(-1,0, 2)).Returns(chunk2).Verifiable();
            _noiseMapGeneratorMock.Setup(noiseMapGenerator => noiseMapGenerator.GenerateChunk(0,-1, 2)).Returns(chunk3).Verifiable();
            _noiseMapGeneratorMock.Setup(noiseMapGenerator => noiseMapGenerator.GenerateChunk(-1,-1, 2)).Returns(chunk4).Verifiable();
            _noiseMapGeneratorMock.Setup(noiseMapGenerator => noiseMapGenerator.GenerateChunk(It.IsAny<int>(),It.IsAny<int>(), 2))
                .Returns((int x, int y, int size) => new Chunk(x, y, map5, _chunkSize)).Verifiable();
            _noiseMapGeneratorMockObject = _noiseMapGeneratorMock.Object;

            _character1 = new Player("naam1", 0, 0, CharacterSymbol.CURRENT_PLAYER, "a");
            _character2 = new Player("naam2", 1, 1, CharacterSymbol.ENEMY_PLAYER, "b");
            
            _characterList = new List<Character>();
            _characterList.Add(_character1);
            _characterList.Add(_character2);
            
            _sut = new Map(_noiseMapGeneratorMockObject, _chunkSize, _chunks);
        }
        
        [Test]
        public void Test_MapConstructor_DoesntThrowException() 
        {
            //Arrange ---------
            //Act ---------
            //Assert ---------
            Assert.DoesNotThrow(() =>
            {
                var map = new Map(_noiseMapGeneratorMockObject,21, _chunks);
            });
        }
        
        [Test]
        public void Test_GetCharArrayMapAroundCharacter_DoesntThrowException() 
        {
            //Arrange ---------
            //Act ---------
            //Assert ---------
            Assert.DoesNotThrow(() =>
            {
                _sut.GetCharArrayMapAroundCharacter(_character1, 1, _characterList);
            });
        }
        
        [Test]
        public void Test_GetCharArrayMapAroundCharacter_DisplaysRightSize() 
        {
            //Arrange ---------
            var viewDistance = 4;
            var screenSize = viewDistance * 2 + 1;
            //Act ---------
            _sut.GetCharArrayMapAroundCharacter(_character1,2, _characterList);
            var mapLength = _sut.GetCharArrayMapAroundCharacter(_character1, viewDistance, _characterList).Length;
            //Assert ---------
            Assert.That(mapLength == screenSize * screenSize);

        }
        
        [Test]
        public void Test_GetCharArrayMapAroundCharacter_DoesntLoadTooBigArea() 
        {
            //Arrange ---------
            var viewDistance = 2;
            var maxLoadingLimit = (int)(Math.Pow(viewDistance, 4)  * _chunkSize / _chunkSize * 4);
            //Act ---------
            _sut.GetCharArrayMapAroundCharacter(_character1,viewDistance, _characterList);
            //Assert ---------
            _noiseMapGeneratorMock.Verify(noiseMapGenerator => noiseMapGenerator.GenerateChunk(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Between(0, maxLoadingLimit, Range.Inclusive));
        }
        
        [Test]
        public void Test_DeleteMap_PassesCommandThrough() 
        {
            //Arrange ---------
            _sut.GetCharArrayMapAroundCharacter(_character1,1, _characterList);
            //Act ---------
            _sut.DeleteMap();
            //Assert ---------
            Assert.That(_chunks.Count == 0);
        }
        
        [Test]
        public void Test_MapConstructor_ThrowsWhenGivenNegativeChunkSize() 
        {
            //Arrange ---------
            //Act ---------
            //Assert ---------
            Assert.Throws<InvalidOperationException>(() =>
            {
                var map = new Map(_noiseMapGeneratorMockObject,-21);
            });
        }
        
        [Test]
        public void Test_GetCharArrayMapAroundCharacter_ThrowsWhenGivenNegativeDisplaySize() 
        {
            //Arrange ---------
            //Act ---------
            //Assert ---------
            Assert.Throws<InvalidOperationException>(() =>
            {
                _sut.GetCharArrayMapAroundCharacter(_character1,-1, _characterList);
            });
        }
        
        [Test]
        public void Test_GetCharArrayMapAroundCharacter_DisplaysCurrentCharacterOnTile() 
        {
            //Arrange ---------
            //Act ---------
            var tileMap = _sut.GetCharArrayMapAroundCharacter(_character1,2, _characterList);
            var tileMapList = tileMap.Cast<char>().ToList();
            //Assert ---------
            Assert.That(tileMapList.Exists(c => c.ToString().Equals(_character1.Symbol)));
        }
        
        [Test]
        public void Test_GetCharArrayMapAroundCharacter_DisplaysNotCurrentCharacterOnTile() 
        {
            //Arrange ---------
            //Act ---------
            var tileMap = _sut.GetCharArrayMapAroundCharacter(_character1,2, _characterList);
            var tileMapList = tileMap.Cast<char>().ToList();
            //Assert ---------
            Assert.That(tileMapList.Exists(c => c.ToString().Equals(_character2.Symbol)));
        }
        
        [Test]
        public void Test_GetCharArrayMapAroundCharacter_DisplaysItemsOnTiles() 
        {
            //Arrange ---------
            //Act ---------
            var tileMap = _sut.GetCharArrayMapAroundCharacter(_character1,1, _characterList);
            var tileMapList = tileMap.Cast<char>().ToList();
            //Assert ---------
            Assert.That(tileMapList.Exists(c => c.ToString().Equals(TileSymbol.CHEST)));
        }
        
        [Test]
        public void Test_GetCharArrayMapAroundCharacter_() 
        {
            //Arrange ---------
            //Act ---------
            //Assert ---------
        }
        /*
        [Test]
        public void Test_GetCharArrayMapAroundCharacter_() 
        {
            //Arrange ---------
            //Act ---------
            //Assert ---------
        }*/
    }
}