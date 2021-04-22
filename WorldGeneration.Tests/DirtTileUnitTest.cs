using NUnit.Framework;
using WorldGeneration.Models.Interfaces;
using WorldGeneration.Models.TerrainTiles;

namespace WorldGeneration.Tests
{
    public class DirtTileUnitTest
    {
        private ITile _tile;
        
        [SetUp]
        public void Setup()
        {
            _tile = new DirtTile();
        }
        
        [Test]
        public void Test_InstanceOf_DirtTile()
        {
            Assert.That(_tile, Is.InstanceOf<DirtTile>());
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
        public void Test_TileSymbol_EqualsTo_DirtTileSymbol()
        {
            Assert.That(_tile.Symbol, Is.EqualTo("."));
        }
    }
}