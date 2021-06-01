using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using WorldGeneration.Models.BuildingTiles;
using WorldGeneration.Models.Interfaces;

namespace WorldGeneration.Tests
{
    [ExcludeFromCodeCoverage]
    public class DoorTileUnitTest
    {
        private IBuildingTile _tile;
        private string _tileSymbol;
        
        [SetUp]
        public void Setup()
        {
            _tile = new DoorTile();
            _tileSymbol = "/";
        }
        
        [Test]
        public void Test_InstanceOf_DoorTile()
        {
            //arrange
            //act
            //assert
            Assert.That(_tile, Is.InstanceOf<DoorTile>());
        }
        
        [Test]
        public void Test_InstanceOf_BuildingTile()
        {
            //arrange
            //act
            //assert
            Assert.That(_tile, Is.InstanceOf<IBuildingTile>());
        }
        
        [Test]
        public void Test_InstanceOf_Tile()
        {
            //arrange
            //act
            //assert
            Assert.That(_tile, Is.InstanceOf<ITile>());
        }
        
        [Test]
        public void Test_SetX_EqualsTo_5()
        {
            //arrange
            //act
            _tile.XPosition = 5;
            //assert
            Assert.That(_tile.XPosition, Is.EqualTo(5));
        }
        
        [Test]
        public void Test_SetY_EqualsTo_5()
        {
            //arrange
            //act
            _tile.YPosition = 5;
            //assert
            Assert.That(_tile.YPosition, Is.EqualTo(5));
        }
        
        [Test]
        public void Test_TileSymbol_EqualsTo_DoorTileSymbol()
        {
            //arrange
            //act
            //assert
            Assert.That(_tile.Symbol, Is.EqualTo(_tileSymbol));
        }
        
        [Test]
        public void Test_DrawBuilding_CanDrawBuilding()
        {
            //arrange
            //act
            //assert
            Assert.Throws<NotImplementedException>(() =>
            {
                _tile.DrawBuilding();
            });
        }
        
        [Test]
        public void Test_IsAccessible_EqualsTo_True()
        {
            //arrange
            //act
            //assert
            Assert.That(_tile.IsAccessible, Is.EqualTo(true));
        }
    }
}