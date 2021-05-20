using System;
using System.Diagnostics.CodeAnalysis;
using DataTransfer.POCO.World.HazardousTiles;
using DataTransfer.POCO.World.Interfaces;
using NUnit.Framework;

namespace WorldGeneration.Tests
{
    [ExcludeFromCodeCoverage]
    public class SpikeTileUnitTest
    {
        private IHazardousTile _tile;
        private string _tileSymbol;
        
        [SetUp]
        public void Setup()
        {
            _tile = new SpikeTile(1,1);
            _tileSymbol = "\u25B2";
        }
        
        [Test]
        public void Test_InstanceOf_SpikeTile()
        {
            Assert.That(_tile, Is.InstanceOf<SpikeTile>());
        }
        
        [Test]
        public void Test_InstanceOf_HazardousTile()
        {
            Assert.That(_tile, Is.InstanceOf<IHazardousTile>());
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
        public void Test_TileSymbol_EqualsTo_SpikeTileSymbol()
        {
            Assert.That(_tile.Symbol, Is.EqualTo(_tileSymbol));
        }
        
        [Test]
        public void Test_GetDamage_GreaterThan_1()
        {
            const int time = 0;
            Assert.That(_tile.GetDamage(time), Is.GreaterThan(1));
        }
        
        [Test]
        public void Test_GetDamage_LessThan_11()
        {
            const int time = 0;
            Assert.That(_tile.GetDamage(time), Is.LessThan(11));
        }
        
        [Test]
        public void Test_IsAccessible_EqualsTo_True()
        {
            Assert.That(_tile.IsAccessible, Is.EqualTo(true));
        }
    }
}