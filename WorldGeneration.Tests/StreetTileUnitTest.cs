using NUnit.Framework;
using WorldGeneration.Models.Interfaces;
using WorldGeneration.Models.TerrainTiles;

namespace WorldGeneration.Tests
{
    public class StreetTileUnitTest
    {
        private ITerrainTile _tile;
        
        [SetUp]
        public void Setup()
        {
            _tile = new StreetTile();
        }
        
        [Test]
        public void Test_InstanceOf_StreetTile()
        {
            Assert.That(_tile, Is.InstanceOf<StreetTile>());
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
        public void Test_TileSymbol_EqualsTo_StreetTileSymbol()
        {
            Assert.That(_tile.Symbol, Is.EqualTo("_"));
        }
    }
}