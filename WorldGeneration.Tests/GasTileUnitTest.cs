using NUnit.Framework;
using WorldGeneration.Models.HazardousTiles;
using WorldGeneration.Models.Interfaces;

namespace WorldGeneration.Tests
{
    public class GasTileUnitTest
    {
        private IHazardousTile _tile;
        
        [SetUp]
        public void Setup()
        {
            _tile = new GasTile(3);
        }
        
        [Test]
        public void Test_InstanceOf_GasTile()
        {
            Assert.That(_tile, Is.InstanceOf<GasTile>());
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
        public void Test_TileSymbol_EqualsTo_GasTileSymbol()
        {
            Assert.That(_tile.Symbol, Is.EqualTo("%"));
        }
        
        [Test]
        public void Test_GetDamage_WithRadius1And5SecondsEqualsTo_5Damage()
        {
            _tile = new GasTile(1);
            const int time = 5; // 5 seconds
            Assert.That(_tile.GetDamage(time), Is.EqualTo(5));
        }
        
        [Test]
        public void Test_GetDamage_WithRadius2And5SecondsEqualsTo_10Damage()
        {
            _tile = new GasTile(2);
            const int time = 5; // 5 seconds
            Assert.That(_tile.GetDamage(time), Is.EqualTo(10));
        }
        
        [Test]
        public void Test_GetDamage_WithRadius3And5SecondsEqualsTo_15Damage()
        {
            _tile = new GasTile(3);
            const int time = 5; // 5 seconds
            Assert.That(_tile.GetDamage(time), Is.EqualTo(15));
        }
    }
}