using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using WorldGeneration.Models.Interfaces;
using WorldGeneration.Models.LootableTiles;

namespace WorldGeneration.Tests
{
    [ExcludeFromCodeCoverage]
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
            //arrange
            //act
            //assert
            Assert.That(_tile, Is.InstanceOf<ChestTile>());
        }

        [Test]
        public void Test_InstanceOf_LootAbleTile()
        {
            //arrange
            //act
            //assert
            Assert.That(_tile, Is.InstanceOf<ILootAbleTile>());
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
        public void Test_TileSymbol_EqualsTo_ChestTileSymbol()
        {
            //arrange
            //act
            //assert
            Assert.That(_tile.Symbol, Is.EqualTo(_tileSymbol));
        }

        [Test]
        public void Test_GenerateLoot_CanGenerateLoot()
        {
            //arrange
            //act
            //assert
            Assert.Throws<NotImplementedException>(() =>
            {
                _tile.GenerateLoot();
            });
        }

        [Test]
        public void Test_LootItem_CanLootItem()
        {
            //arrange
            //act
            //assert
            Assert.Throws<NotImplementedException>(() =>
            {
                const int itemId = 0;
                _tile.LootItem(itemId);
            });
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