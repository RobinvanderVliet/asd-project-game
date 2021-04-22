using NUnit.Framework;
using WorldGeneration.Models.Interfaces;
using WorldGeneration.Models.TerrainTiles;

namespace WorldGeneration.Tests
{
    public class StreetTileUnitTest
    {
        private ITerrainTile _tile;
        private string _tileSymbol;
        
        [SetUp]
        public void Setup()
        {
            _tile = new StreetTile();
            _tileSymbol = "_";
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
        public void Test_SetX_EqualsTo_5()
        {
            _tile.X = 5;
            Assert.That(_tile.X, Is.EqualTo(5));
        }
        
        [Test]
        public void Test_SetY_EqualsTo_5()
        {
            _tile.Y = 5;
            Assert.That(_tile.Y, Is.EqualTo(5));
        }
        
        [Test]
        public void Test_TileSymbol_EqualsTo_StreetTileSymbol()
        {
            Assert.That(_tile.Symbol, Is.EqualTo(_tileSymbol));
        }
        
        [Test]
        public void Test_IsAccessible_EqualsTo_True()
        {
            Assert.That(_tile.IsAccessible, Is.EqualTo(true));
        }
    }
}