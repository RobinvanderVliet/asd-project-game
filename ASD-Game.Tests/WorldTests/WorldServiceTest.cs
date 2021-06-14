using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ASD_Game.Items;
using ASD_Game.Items.Services;
using ASD_Game.UserInterface;
using ASD_Game.World;
using ASD_Game.World.Models;
using ASD_Game.World.Models.Characters;
using ASD_Game.World.Models.TerrainTiles;
using ASD_Game.World.Services;
using Moq;
using NUnit.Framework;

namespace ASD_Game.Tests.WorldTests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class WorldServiceTest
    {
        //Declaration and initialisation of constant variables

        //Declaration of variables
        private Player _player;
        private Monster _monster;
        private List<Monster> _creatures;

        //Declaration of mocks
        private WorldService _sut;
        private Mock<IWorld> _worldMock;
        private IWorld _worldObject;
        private Mock<IScreenHandler> _screenHandlerMock;
        private IScreenHandler _screenHandlerObject;
        private Mock<IItemService> _itemServiceMock;
        private IItemService _itemServiceObject;

        [SetUp]
        public void Setup()
        {
            //Initialisation of variables
            _player = new Player("Ugur Ekim", 0, 0, CharacterSymbol.CURRENT_PLAYER, "1");
            _monster = new Monster("Mark Brouwer", 1, 1, CharacterSymbol.TERMINATOR, "2");
            _creatures = new List<Monster>();
            //Initialisation of mocks
            _worldMock = new Mock<IWorld>();
            _worldMock.Setup(world => world.CurrentPlayer).Returns(_player);
            _worldMock.Setup(world => world.Creatures).Returns(_creatures);
            _worldMock.Setup(world => world.AddCreatureToWorld(It.IsAny<Monster>())).Verifiable();
            _worldMock.Setup(world => world.AddPlayerToWorld(It.IsAny<Player>(), It.IsAny<bool>())).Verifiable();
            _worldMock.Setup(world => world.GetCurrentTile().ItemsOnTile);
            _worldMock.Setup(world => world.GetTileForPlayer(It.IsAny<Player>()).ItemsOnTile);
            _worldObject = _worldMock.Object;
            _screenHandlerMock = new Mock<IScreenHandler>();
            _screenHandlerMock.Setup(screenHandler => 
                screenHandler.SetStatValues(
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>())).Verifiable();
            _screenHandlerObject = _screenHandlerMock.Object;
            _itemServiceMock = new Mock<IItemService>();
            _itemServiceObject = _itemServiceMock.Object;
            _sut = new WorldService(_screenHandlerObject, _itemServiceObject);
            _sut.SetWorld(_worldObject);
        }

        [Test]
        public void Test_UpdateCharacterPosition_CallsUpdateCharacterPositionFromWorld()
        {
            //Arrange ---------
            var userId = Guid.NewGuid().ToString();
            const int x = 0;
            const int y = 0;
            //Act ---------
            _sut.UpdateCharacterPosition(userId, x, y);
            //Assert ---------
            _worldMock.Verify(world => world.UpdateCharacterPosition(userId, x, y), Times.Once);
        }

        [Test]
        public void Test_AddPlayerToWorld_AddsPlayerToWorld()
        {
            //Arrange ---------
            const bool isCurrentPlayer = true;
            //Act ---------
            _sut.AddPlayerToWorld(_player, isCurrentPlayer);
            //Assert ---------
            _worldMock.Verify(world => world.AddPlayerToWorld(_player, isCurrentPlayer), Times.Once);
        }

        [Test]
        public void Test_AddCreatureToWorld_AddsCreatureToWorld()
        {
            //Arrange ---------

            //Act ---------
            _sut.AddCreatureToWorld(_monster);
            //Assert ---------
            _worldMock.Verify(world => world.AddCreatureToWorld(_monster), Times.Once);
        }

        [Test]
        public void Test_DisplayWorld_CallsUpdateMapOnWorldToDisplay()
        {
            //Arrange ---------

            //Act ---------
            _sut.DisplayWorld();
            //Assert ---------
            _worldMock.Verify(world => world.UpdateMap(), Times.Once);
        }

        [Test]
        public void Test_DeleteMap_CallsDeleteMapOnWorld()
        {
            //Arrange ---------

            //Act ---------
            _sut.DeleteMap();
            //Assert ---------
            _worldMock.Verify(world => world.DeleteMap(), Times.Once);
        }

        [Test]
        public void Test_GetCurrentPlayer_GetsTheCurrentPlayerFromWorld()
        {
            //Arrange ---------
            //Act ---------
            var currentPlayer = _sut.GetCurrentPlayer();
            //Assert ---------
            Assert.AreEqual(_player, currentPlayer);
        }

        [Test]
        public void Test_GetMapAroundCharacter_VerifyThatGetMapAroundCharacterIsCalledFromWorld()
        {
            //Arrange ---------
            //Act ---------
            _sut.GetMapAroundCharacter(_player);
            //Assert ---------
            _worldMock.Verify(world => world.GetMapAroundCharacter(_player), Times.Once);
        }
        
        [Test]
        public void Test_GetMonsters_GetsTheCreatureListFromWorld()
        {
            //Arrange ---------
            //Act ---------
            _sut.GetMonsters();
            //Assert ---------
            _worldMock.Verify(world => world.Creatures, Times.Once);
        }
        
        [Test]
        public void Test_GetCreatureMoves_ReturnsNullBecauseWorldIsNull()
        {
            //Arrange ---------
            _sut.SetWorld(null);
            //Act ---------
            var actual = _sut.GetCreatureMoves();
            //Assert ---------
            Assert.AreEqual(null, actual);
        }
        
        [Test]
        public void Test_GetCreatureMoves_CallsUpdateAI()
        {
            //Arrange ---------
            //Act ---------
            _sut.GetCreatureMoves();
            //Assert ---------
            _worldMock.Verify(world => world.UpdateAI(), Times.Once);
        }
        
        [Test]
        public void Test_GetAllPlayers_GetsAllThePlayersInTheWorld()
        {
            //Arrange ---------
            //Act ---------
            _sut.GetAllPlayers();
            
            //Assert ---------
            _worldMock.Verify(world => world.Players);
        }
        
        [Test]
        public void Test_IsDead_CharacterIsNotDead()
        {
            //Arrange ---------
            _player.Health = 50;
            //Act ---------
            //Assert ---------
            Assert.That(!_sut.IsDead(_player));
        }
        [Test]
        public void Test_IsDead_CharacterIsDead()
        {
            //Arrange ---------
            _player.Health = 0;
            //Act ---------
            //Assert ---------
            Assert.That(_sut.IsDead(_player));
        }
        
        [Test]
        public void Test_LoadArea_VerifiesThatLoadAreaFromWorldIsExecuted()
        {
            //Arrange ---------
            const int x = 0;
            const int y = 0;
            const int viewDistance = 5;
            //Act ---------
            _sut.LoadArea(x, y, viewDistance);
            //Assert ---------
            _worldMock.Verify(world => world.LoadArea(x, y, viewDistance), Times.Once);
        }
        
        [Test]
        public void Test_SearchCurrentTile_ReturnsItemsOnTileInAString()
        {
            //Arrange ---------
            //Act ---------
            //Assert ---------
        }
        
        [Test]
        public void Test_GetPlayer_GetsAPlayerByUserID()
        {
            //Arrange ---------
            //Act ---------
            _sut.GetPlayer(_player.Id);
            //Assert ---------
            _worldMock.Verify(world => world.GetPlayer(_player.Id));
        }
        
        [Test]
        public void Test_GetAI_VerifyThatGetAIFromWorldIsExecuted()
        {
            //Arrange ---------
            const string id = "2";
            //Act ---------
            _sut.GetAI(id);
            //Assert ---------
            _worldMock.Verify(world => world.GetAI(id), Times.Once);
        }
        
        [Test]
        public void Test_GetTile_VerifyThatGetLoadedTileByXAndYFromWorldIsExecuted()
        {
            //Arrange ---------
            const int x = 0;
            const int y = 0;
            //Act ---------
            _sut.GetTile(x, y);
            //Assert ---------
            _worldMock.Verify(world => world.GetLoadedTileByXAndY(x, y), Times.Once);
        }    
        
        [Test]
        public void Test_CheckIfCharacterOnTile_VerifyThatCheckIfCharacterOnTileFromWorldIsExecuted()
        {
            //Arrange ---------
            var tile = new StreetTile(0, 0);
            //Act ---------
            _sut.CheckIfCharacterOnTile(tile);
            //Assert ---------
            _worldMock.Verify(world => world.CheckIfCharacterOnTile(tile), Times.Once);
        }
        
        [Test]
        public void Test_GetItemsOnCurrentTile_VerifyThatGetCurrentTileFromWorldIsExecuted()
        {
            //Arrange ---------
            //Act ---------
            _sut.GetItemsOnCurrentTile();
            //Assert ---------
            _worldMock.Verify(world => world.GetCurrentTile(), Times.Once);
        }
        
        [Test]
        public void Test_GetItemsOnCurrentTile_VerifyThatGetCurrentTileFromWorldIsExecutedWithPlayer()
        {
            //Arrange ---------
            //Act ---------
            _sut.GetItemsOnCurrentTile(_player);
            //Assert ---------
            _worldMock.Verify(world => world.GetTileForPlayer(_player), Times.Once);
            
        }
        
        [Test]
        public void Test_DisplayStats_VerifyThatSetStatValuesFromScreenHandlerIsExecutedWithPlayer()
        {
            //Arrange ---------
            _sut.DisplayStats();
            //Assert ---------
            _worldMock.Verify(world => world.CurrentPlayer, Times.Once);
            _screenHandlerMock.Verify(screenHandler => screenHandler.SetStatValues(
                _player.Name,
                0,
                _player.Health,
                _player.Stamina,
                _player.GetArmorPoints(),
                _player.RadiationLevel,
                _player.Inventory.Helmet.ItemName ?? "Empty",
                "Empty",
                _player.Inventory.Weapon.ItemName ?? "Empty",
                "Empty",
                "Empty",
                "Empty"));
            
        }
    }
}