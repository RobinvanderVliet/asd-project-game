using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Player.Model;

namespace Player.Tests
{
    [ExcludeFromCodeCoverage]
    public class InventoryTest
    {
        private Inventory _sut;
        
        [SetUp]
        public void Setup()
        {
            _sut = new Inventory();
        }
        
        [Test]
        public void Test_GetItemList_GetsItemListSuccessfully()
        {
            //arrange
            //act
            //assert
            Assert.AreEqual(new List<Item>(), _sut.ItemList);
        }
        
        [Test]
        public void Test_SetItemList_SetsItemListSuccessfully()
        {
            //arrange
            var ItemList = new List<IItem>();
            ItemList.Add(new Item("ItemName", "Description"));
            _sut.ItemList = ItemList;
            //act
            //assert
            Assert.AreEqual(ItemList, _sut.ItemList);
        }
        
        [Test]
        public void Test_GetItem_GetsRightItem()
        {
            //arrange
            Item item = new Item("ItemName", "Description");
            _sut.ItemList.Add(item);
            //act
            //assert
            Assert.AreEqual(item, _sut.GetItem("ItemName"));
        }
        
        [Test]
        public void Test_GetItem_ReturnsNull()
        {
            //arrange
            Item item = new Item("ItemName", "Description");
            _sut.ItemList.Add(item);
            //act
            //assert
            Assert.AreEqual(null, _sut.GetItem("UnexistingItemName"));
        }
        
        [Test]
        public void Test_AddItem_AddsItemSuccessfully()
        {
            //arrange
            Item item = new Item("ItemName", "Description");
            //act
            _sut.AddItem(item);
            //assert
            Assert.AreEqual(item, _sut.GetItem("ItemName"));
        }
        
        [Test]
        public void Test_RemoveItem_RemovesItemSuccessfully()
        {
            //arrange
            Item item = new Item("ItemName", "Description");
            _sut.ItemList.Add(item);
            //act
            _sut.RemoveItem(item);
            //assert
            Assert.AreEqual(new List<IItem>(), _sut.ItemList);
        }
        
        [Test]
        public void Test_EmptyInventory_EmptiesInventorySuccessfully()
        {
            //arrange
            Item item = new Item("ItemName", "Description");
            _sut.AddItem(item);
            //act
            _sut.EmptyInventory();
            //assert
            Assert.AreEqual(new List<IItem>(), _sut.ItemList);
        }
    }
}