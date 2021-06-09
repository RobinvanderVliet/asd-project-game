using System.Diagnostics.CodeAnalysis;
using ASD_Game.World.Models.HazardousTiles;
using ASD_Game.World.Models.Interfaces;
using NUnit.Framework;

namespace ASD_Game.Tests.WorldTests.Models
{
    [ExcludeFromCodeCoverage]
    public class GasTileUnitTest
    {
        private IHazardousTile _tile;
        private string _tileSymbol;
        
        [SetUp]
        public void Setup()
        {
            _tile = new GasTile(1,1,3);
            _tileSymbol = "&";
        }
        
        [Test]
        public void Test_InstanceOf_GasTile()
        {
            //arrange
            //act
            //assert
            Assert.That(_tile, Is.InstanceOf<GasTile>());
        }
        
        [Test]
        public void Test_InstanceOf_HazardousTile()
        {
            //arrange
            //act
            //assert
            Assert.That(_tile, Is.InstanceOf<IHazardousTile>());
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
        public void Test_TileSymbol_EqualsTo_GasTileSymbol()
        {
            //arrange
            //act
            //assert
            Assert.That(_tile.Symbol, Is.EqualTo(_tileSymbol));
        }
        
        [Test]
        public void Test_GetDamage_WithRadius1And5SecondsEqualsTo_5Damage()
        {
            //arrange
            _tile = new GasTile(1,1,1);
            const int time = 5; // 5 seconds
            //act & assert
            Assert.That(_tile.GetDamage(time), Is.EqualTo(5));
        }
        
        [Test]
        public void Test_GetDamage_WithRadius2And5SecondsEqualsTo_10Damage()
        {
            //arrange
            _tile = new GasTile(1,1,2);
            const int time = 5; // 5 seconds
            //act & assert
            Assert.That(_tile.GetDamage(time), Is.EqualTo(10));
        }
        
        [Test]
        public void Test_GetDamage_WithRadius3And5SecondsEqualsTo_15Damage()
        {
            //arrange
            _tile = new GasTile(1,1,3);
            const int time = 5; // 5 seconds
            //act & assert
            Assert.That(_tile.GetDamage(time), Is.EqualTo(15));
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