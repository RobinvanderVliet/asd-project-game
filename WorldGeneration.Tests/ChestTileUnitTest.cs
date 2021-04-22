using System;
using NUnit.Framework;
using WorldGeneration.Models.Interfaces;
using WorldGeneration.Models.LootableTiles;

namespace WorldGeneration.Tests
{
    public class ChestTileUnitTest
    {
        private ILootAbleTile _tile;
        
        [SetUp]
        public void Setup()
        {
            _tile = new ChestTile();
        }
        
        [Test]
        public void Test_InstanceOf_ChestTile()
        {
            Assert.That(_tile, Is.InstanceOf<ChestTile>());
        }
        
        [Test]
        public void Test_InstanceOf_LootAbleTile()
        {
            Assert.That(_tile, Is.InstanceOf<ILootAbleTile>());
        }
        
        [Test]
        public void Test_InstanceOf_Tile()
        {
            Assert.That(_tile, Is.InstanceOf<ITile>());
        }
        
        [Test]
        public void Test_Symbol_EqualsTo_ChestTileSymbol()
        {
            Assert.That(_tile.Symbol, Is.EqualTo("n"));
        }
        
        [Test]
        public void Test_GenerateLoot_CanGenerateLoot()
        {
            Assert.Throws<NotImplementedException>(() =>
            {
                _tile.GenerateLoot();
            });
        }
        
        [Test]
        public void Test_LootItem_CanLootItem()
        {
            Assert.Throws<NotImplementedException>(() =>
            {
                var itemId = 0;
                _tile.LootItem(itemId);
            });
        }
    }
}