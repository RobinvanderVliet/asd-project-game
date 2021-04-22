using System;
using NUnit.Framework;
using WorldGeneration.Models.HazardousTiles;
using WorldGeneration.Models.Interfaces;

namespace WorldGeneration.Tests
{
    public class SpikeTileUnitTest
    {
        private IHazardousTile _tile;
        
        [SetUp]
        public void Setup()
        {
            _tile = new SpikeTile();
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
        public void Test_Symbol_EqualsTo_SpikeTileSymbol()
        {
            Assert.That(_tile.Symbol, Is.EqualTo("^"));
        }
        
        [Test]
        public void Test_GetDamage_GreaterThan_1()
        {
            var time = 0;
            Assert.That(_tile.GetDamage(time), Is.GreaterThan(1));
        }
        
        [Test]
        public void Test_GetDamage_LessThan_11()
        {
            var time = 0;
            Assert.That(_tile.GetDamage(time), Is.LessThan(11));
        }
    }
}