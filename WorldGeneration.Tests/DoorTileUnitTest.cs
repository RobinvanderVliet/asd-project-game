using System;
using NUnit.Framework;
using WorldGeneration.Models.BuildingTiles;
using WorldGeneration.Models.Interfaces;

namespace WorldGeneration.Tests
{
    public class DoorTileUnitTest
    {
        private IBuildingTile _tile;
        
        [SetUp]
        public void Setup()
        {
            _tile = new DoorTile();
        }
        
        [Test]
        public void Test_InstanceOf_DoorTile()
        {
            Assert.That(_tile, Is.InstanceOf<DoorTile>());
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
        public void Test_TileSymbol_EqualsTo_DoorTileSymbol()
        {
            Assert.That(_tile.Symbol, Is.EqualTo("/"));
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