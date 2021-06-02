using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using WorldGeneration.Models.Interfaces;
using WorldGeneration.Models.TerrainTiles;

namespace WorldGeneration.Tests
{
    [ExcludeFromCodeCoverage]
    public class StreetTileUnitTest
    {
        private ITerrainTile _tile;
        private string _tileSymbol;

        [SetUp]
        public void Setup()
        {
            _tile = new StreetTile(1, 1);
            _tileSymbol = "\u2591";
        }

        [Test]
        public void Test_InstanceOf_StreetTile()
        {
            //arrange
            //act
            //assert
            Assert.That(_tile, Is.InstanceOf<StreetTile>());
        }

        [Test]
        public void Test_InstanceOf_TerrainTile()
        {
            //arrange
            //act
            //assert
            Assert.That(_tile, Is.InstanceOf<ITerrainTile>());
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
        public void Test_TileSymbol_EqualsTo_StreetTileSymbol()
        {
            //arrange
            //act
            //assert
            Assert.That(_tile.Symbol, Is.EqualTo(_tileSymbol));
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