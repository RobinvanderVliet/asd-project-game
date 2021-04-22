using NUnit.Framework;
using WorldGeneration.Models.Interfaces;
using WorldGeneration.Models.TerrainTiles;

namespace WorldGeneration.Tests
{
    public class WaterTileUnitTest
    {
        private ITerrainTile _tile;
        
        [SetUp]
        public void Setup()
        {
            _tile = new WaterTile();
        }
        
        [Test]
        public void Test_InstanceOf_WaterTile()
        {
            Assert.That(_tile, Is.InstanceOf<WaterTile>());
        }
        
        [Test]
        public void Test_InstanceOf_TerrainTile()
        {
            Assert.That(_tile, Is.InstanceOf<ITerrainTile>());
        }
        
        [Test]
        public void Test_InstanceOf_Tile()
        {
            Assert.That(_tile, Is.InstanceOf<ITile>());
        }
        
        [Test]
        public void Test_Symbol_EqualsTo_WaterTileSymbol()
        {
            Assert.That(_tile.Symbol, Is.EqualTo("~"));
        }
    }
}