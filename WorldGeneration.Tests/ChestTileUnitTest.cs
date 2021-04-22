using System;
using NUnit.Framework;
using WorldGeneration.Models.Interfaces;
using WorldGeneration.Models.LootableTiles;

namespace WorldGeneration.Tests
{
    public class ChestTileUnitTest
    {
        private ILootAbleTile _tile;
        private string _tileSymbol;
        
        [SetUp]
        public void Setup()
        {
            _tile = new ChestTile();
            _tileSymbol = "n";
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
        public void Test_TileSymbol_EqualsTo_ChestTileSymbol()
        {
            Assert.That(_tile.Symbol, Is.EqualTo(_tileSymbol));
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
                const int itemId = 0;
                _tile.LootItem(itemId);
            });
        }

        [Test]
        public void Test_IsAccessible_EqualsTo_True()
        {
            Assert.That(_tile.IsAccessible, Is.EqualTo(true));
        }
    }
}