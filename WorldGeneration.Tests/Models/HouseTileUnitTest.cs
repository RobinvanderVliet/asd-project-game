using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using WorldGeneration.Models.BuildingTiles;
using WorldGeneration.Models.Interfaces;

namespace WorldGeneration.Tests
{
    [ExcludeFromCodeCoverage]
    public class HouseTileUnitTest
    {
        private IBuildingTile _tile;
        private string _tileSymbol;
        
        [SetUp]
        public void Setup()
        {
            _tile = new HouseTile();
            _tileSymbol = "+";
        }

        [Test]
        public void Test_InstanceOf_HouseTile()
        {
            Assert.That(_tile, Is.InstanceOf<HouseTile>());
        }
        
        [Test]
        public void Test_InstanceOf_BuildingTile()
        {
            Assert.That(_tile, Is.InstanceOf<IBuildingTile>());
        }
        
        [Test]
        public void Test_InstanceOf_Tile()
        {
            Assert.That(_tile, Is.InstanceOf<ITile>());
        }
        
        [Test]
        public void Test_SetX_EqualsTo_5()
        {
            _tile.XPosition = 5;
            Assert.That(_tile.XPosition, Is.EqualTo(5));
        }
        
        [Test]
        public void Test_SetY_EqualsTo_5()
        {
            _tile.YPosition = 5;
            Assert.That(_tile.YPosition, Is.EqualTo(5));
        }
        
        [Test]
        public void Test_TileSymbol_EqualsTo_HouseTileSymbol()
        {
            Assert.That(_tile.Symbol, Is.EqualTo(_tileSymbol));
        }
        
        [Test]
        public void Test_DrawBuilding_CanDrawBuilding()
        {
            Assert.Throws<NotImplementedException>(() =>
            {
                _tile.DrawBuilding();
            });
        }
        
        [Test]
        public void Test_IsAccessible_EqualsTo_True()
        {
            Assert.That(_tile.IsAccessible, Is.EqualTo(true));
        }
    }
}