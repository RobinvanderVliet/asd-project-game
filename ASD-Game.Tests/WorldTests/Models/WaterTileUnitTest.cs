using System.Diagnostics.CodeAnalysis;
using ASD_Game.World.Models.Interfaces;
using ASD_Game.World.Models.TerrainTiles;
using NUnit.Framework;

namespace ASD_Game.Tests.WorldTests.Models
{
    [ExcludeFromCodeCoverage]
    public class WaterTileUnitTest
    {
        private ITerrainTile _tile;
        private string _tileSymbol;
        
        [SetUp]
        public void Setup()
        {
            _tile = new WaterTile(1,1);
            _tileSymbol = "~";
        }
        
        [Test]
        public void Test_InstanceOf_WaterTile()
        {
            //arrange
            //act
            //assert
            Assert.That(_tile, Is.InstanceOf<WaterTile>());
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
        public void Test_TileSymbol_EqualsTo_WaterTileSymbol()
        {
            //arrange
            //act
            //assert
            Assert.That(_tile.Symbol, Is.EqualTo(_tileSymbol));
        }
        
        [Test]
        public void Test_IsAccessible_EqualsTo_False()
        {
            //arrange
            //act
            //assert
            Assert.That(_tile.IsAccessible, Is.EqualTo(false));
        }
    }
}