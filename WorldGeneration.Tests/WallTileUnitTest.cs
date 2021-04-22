using System;
using NUnit.Framework;
using WorldGeneration.Models.BuildingTiles;
using WorldGeneration.Models.Interfaces;

namespace WorldGeneration.Tests
{
    public class WallTileUnitTest
    {
        private IBuildingTile _tile;
        private string _tileSymbol;
        
        [SetUp]
        public void Setup()
        {
            _tile = new WallTile();
            _tileSymbol = "\u25A0";
        }
        
        [Test]
        public void Test_InstanceOf_WallTile()
        {
            Assert.That(_tile, Is.InstanceOf<WallTile>());
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
        public void Test_TileSymbol_EqualsTo_WallTileSymbol()
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
    }
}