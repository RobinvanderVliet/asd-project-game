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
            Assert.AreEqual(new List<Item>(), _sut.ConsumableItemList);
        }
        
        [Test]
        public void Test_SetItemList_SetsItemListSuccessfully()
        {
            var ItemList = new List<IItem>();
            ItemList.Add(new Item("ItemName", "Description"));
            _sut.ConsumableItemList = ItemList;
            
            Assert.AreEqual(ItemList, _sut.ConsumableItemList);
        }
        
        [Test]
        public void Test_GetItem_GetsRightItem()
        {
            Item item = new Item("ItemName", "Description");
            _sut.ConsumableItemList.Add(item);
            
            Assert.AreEqual(item, _sut.GetConsumableItem("ItemName"));
        }
        
        [Test]
        public void Test_GetItem_ReturnsNull()
        {
            Item item = new Item("ItemName", "Description");
            _sut.ConsumableItemList.Add(item);
            
            Assert.AreEqual(null, _sut.GetConsumableItem("UnexistingItemName"));
        }
        
        [Test]
        public void Test_AddItem_AddsItemSuccessfully()
        {
            Item item = new Item("ItemName", "Description");
            
            _sut.AddConsumableItem(item);
            
            Assert.AreEqual(item, _sut.GetConsumableItem("ItemName"));
        }
        
        [Test]
        public void Test_RemoveItem_RemovesItemSuccessfully()
        {
            Item item = new Item("ItemName", "Description");
            _sut.ConsumableItemList.Add(item);
            
            _sut.RemoveConsumableItem(item);
            
            Assert.AreEqual(new List<IItem>(), _sut.ConsumableItemList);
        }
        
        [Test]
        public void Test_EmptyInventory_EmptiesInventorySuccessfully()
        {
            Item item = new Item("ItemName", "Description");
            _sut.AddConsumableItem(item);
            
            _sut.EmptyConsumableItemList();
            
            Assert.AreEqual(new List<IItem>(), _sut.ConsumableItemList);
        }
    }
}