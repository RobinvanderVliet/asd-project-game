using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Unicode;
using ASD_Game.ActionHandling;
using ASD_Game.ActionHandling.DTO;
using ASD_Game.Items;
using ASD_Game.Items.Services;
using ASD_Game.UserInterface;
using ASD_Game.World;
using ASD_Game.World.Models;
using ASD_Game.World.Models.Characters;
using ASD_Game.World.Models.TerrainTiles;
using Moq;
using NUnit.Framework;

namespace ASD_Game.Tests.WorldTests
{
    [ExcludeFromCodeCoverage]  
    [TestFixture]
    public class WorldTest
    {
        //Declaration and initialisation of constant variables
 
        //Declaration of variables
        private Player _friendlyPlayer;
        private Player _enemyPlayer;
        private Monster _creature;
        private List<ItemSpawnDTO> _itemSpawnDTOs;
 
        //Declaration of mocks
        private IMapFactory _mapFactoryMockObject;
        private Mock<IMapFactory> _mapFactoryMock;
        private IMap _mapMockObject;
        private Mock<IMap> _mapMock;
        private Mock<IScreenHandler> _screenHandlerMock;
        private IScreenHandler _screenHandlerMockObject;
        private Mock<IItemService> _itemServiceMock;
        private IItemService _itemServiceMockObject;
        private Mock<ISpawnHandler> _spawnHandlerMock;
        private ISpawnHandler _spawnHandlerMockObject;
        private World.World _sut;

        [SetUp]
        public void Setup()
        {
            //Initialisation of variables
            _friendlyPlayer = new Player("A", 0, 0, CharacterSymbol.CURRENT_PLAYER, "1");
            _enemyPlayer = new Player("B", 1, 1, CharacterSymbol.ENEMY_PLAYER, "2");
            _creature = new Monster("B", 1, 0, "!", "2");
            //Initialisation of mocks
            _mapMock = new Mock<IMap>();
            _mapMock.Setup(map => map.DeleteMap()).Verifiable();
            _mapMock.Setup(map => map.GetCharArrayMapAroundCharacter(It.IsAny<Character>(), It.IsAny<int>(), It.IsAny<List<Character>>()))
                .Returns(new char[5,5]).Verifiable();
            _mapMock.Setup(map => map.GetLoadedTileByXAndY(It.IsAny<int>(), It.IsAny<int>())).Verifiable();
            _mapMockObject = _mapMock.Object;
            
            _mapFactoryMock = new Mock<IMapFactory>();
            _mapFactoryMock.Setup(mapFactory => mapFactory.GenerateMap(It.IsAny<IItemService>(), It.IsAny<List<ItemSpawnDTO>>(), It.IsAny<int>())).Returns(_mapMockObject).Verifiable();
            _mapFactoryMock.Setup(mapFactory => mapFactory.GenerateSeed()).Returns(11246).Verifiable();
            _mapFactoryMockObject = _mapFactoryMock.Object;

            _screenHandlerMock = new Mock<IScreenHandler>();
            _screenHandlerMock.Setup(handler => handler.UpdateWorld(It.IsAny<char[,]>())).Verifiable();
            _screenHandlerMockObject = _screenHandlerMock.Object;

            _spawnHandlerMock = new Mock<ISpawnHandler>();
            _spawnHandlerMockObject = _spawnHandlerMock.Object;
            
            _itemServiceMock = new Mock<IItemService>();
            _itemServiceMock.Setup(itemService => itemService.GetSpawnHandler()).Returns(_spawnHandlerMockObject);
;           _itemServiceMockObject = _itemServiceMock.Object;

            _sut = new World.World(5, 2, _mapFactoryMockObject, _screenHandlerMockObject, _itemServiceMockObject);
        }
        
        [Test]
        public void Test_WorldConstructor_SetsMapUpCorrectly() 
        {
            //Arrange ---------
            var chunkSize = 8;
            var seed = 10;
            //Act ---------
            //Assert ---------
            Assert.DoesNotThrow(() =>
            {
                var world = new World.World(seed, 55, _mapFactoryMockObject, _screenHandlerMockObject, _itemServiceMockObject);
            });
            _mapFactoryMock.Verify(mapFactory => mapFactory.GenerateMap(_itemServiceMockObject, It.IsAny<List<ItemSpawnDTO>>(), seed), Times.Once);
        }

        [Test]
        public void Test_DisplayWorld_DoesNothingWithoutCurrentPlayer() 
        {
            //Arrange ---------
            //Act ---------
            _sut.UpdateMap();
            //Assert ---------
            _mapMock.Verify(map => map.GetCharArrayMapAroundCharacter(It.IsAny<Player>(), It.IsAny<int>(), It.IsAny<List<Character>>()), Times.AtMost(0));
        }
        
        [Test]
        public void Test_UpdateMap_CallsGetMapAroundCharacterWhenGivenCharacters() 
        {
            //Arrange ---------
            //Act ---------
            _sut.AddPlayerToWorld(_friendlyPlayer, true);
            _sut.UpdateMap();
            //Assert ---------
            _mapMock.Verify(map => map.GetCharArrayMapAroundCharacter(It.IsAny<Player>(), It.IsAny<int>(), It.IsAny<List<Character>>()), Times.Exactly(2));

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
            _sut.AddPlayerToWorld(_friendlyPlayer, true);
            //Assert ---------
            Assert.That(_sut.CurrentPlayer.XPosition != _enemyPlayer.XPosition);
        }
        
        [Test]
        public void Test_GetMapAroundCharacter_WithListOfPlayersAndCreatures() 
        {
            //Arrange ---------
            var characters = new List<Character> {_friendlyPlayer, _enemyPlayer, _creature};
            _sut.AddPlayerToWorld(_friendlyPlayer);
            _sut.AddPlayerToWorld(_enemyPlayer);
            _sut.AddCreatureToWorld(_creature);
            //Act ---------
            var results = _sut.GetMapAroundCharacter(_friendlyPlayer);
            //Assert ---------
            _mapMock.Verify(map => map.GetCharArrayMapAroundCharacter(_friendlyPlayer, 2, characters));
        }
        
        [Test]
        public void Test_DeleteMap_DeleteEntireMap() 
        {
            //Arrange ---------
            //Act ---------
            _sut.DeleteMap();
            //Assert ---------
            _mapMock.Verify(map => map.DeleteMap());
        }
        
        [Test]
        public void Test_AddItemToWorld_AddsItemToMap() 
        {
            //Arrange ---------
            var itemSpawnDto = new ItemSpawnDTO
            {
                Item = ItemFactory.GetMilitaryHelmet()
            };
            //Act ---------
            _sut.AddItemToWorld(itemSpawnDto);
            //Assert ---------
            Assert.That(1 == _sut.Items.Count);
        }
        
        [Test]
        public void Test_GetLoadedTileByXAndY_ExecutesGetLoadedTileByXAndYInsideMap() 
        {
            //Arrange ---------
            const int x = 0;
            const int y = 0;
            //Act ---------
            var result = _sut.GetLoadedTileByXAndY(x, y);
            //Assert ---------
            _mapMock.Verify(map => map.GetLoadedTileByXAndY(x, y));
        }
        
        [Test]
        public void Test_CheckIfCharacterOnTile_ChecksIfCharacterIsOnTile() 
        {
            //Arrange ---------
            var tile = new StreetTile(0, 0);
            _sut.AddPlayerToWorld(_friendlyPlayer);
            //Act ---------
            var result = _sut.CheckIfCharacterOnTile(tile);
            //Assert ---------
            Assert.AreEqual(true, result);
        }
        
        [Test]
        public void Test_GetAllPlayers_GetsAllThePlayersInTheWorld() 
        {
            //Arrange ---------
            _sut.AddPlayerToWorld(_friendlyPlayer);
            _sut.AddPlayerToWorld(_enemyPlayer);
            //Act ---------
            var result = _sut.GetAllPlayers().Count;
            //Assert ---------
            Assert.AreEqual(2, result);
        }
        [Test]
        public void Test_GetCurrentTile_GetsTileByPlayerXAndY() 
        {
            //Arrange ---------
            _sut.AddPlayerToWorld(_friendlyPlayer,true);
            //Act ---------
            var result = _sut.GetCurrentTile();
            //Assert ---------
            _mapMock.Verify(map => map.GetLoadedTileByXAndY(_friendlyPlayer.XPosition, _friendlyPlayer.YPosition));
        }
    }
}