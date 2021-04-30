using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Player.Model;

namespace Player.Tests
{
    [ExcludeFromCodeCoverage]
    public class InventoryTest
    {
        private Inventory sut;
        
        [SetUp]
        public void Setup()
        {
            sut = new Inventory();
        }
        
        [Test]
        public void Test_GetItemList_GetsItemListSuccessfully()
        {
            Assert.AreEqual(new List<Item>(), sut.ItemList);
        }
        
        [Test]
        public void Test_SetItemList_SetsItemListSuccessfully()
        {
            var ItemList = new List<IItem>();
            ItemList.Add(new Item("ItemName", "Description"));
            sut.ItemList = ItemList;
            
            Assert.AreEqual(ItemList, sut.ItemList);
        }
        
        [Test]
        public void Test_GetItem_GetsRightItem()
        {
            Item item = new Item("ItemName", "Description");
            sut.ItemList.Add(item);
            
            Assert.AreEqual(item, sut.GetItem("ItemName"));
        }
        
        [Test]
        public void Test_GetItem_ReturnsNull()
        {
            Item item = new Item("ItemName", "Description");
            sut.ItemList.Add(item);
            
            Assert.AreEqual(null, sut.GetItem("UnexistingItemName"));
        }
        
        [Test]
        public void Test_AddItem_AddsItemSuccessfully()
        {
            Item item = new Item("ItemName", "Description");
            
            sut.AddItem(item);
            
            Assert.AreEqual(item, sut.GetItem("ItemName"));
        }
        
        [Test]
        public void Test_RemoveItem_RemovesItemSuccessfully()
        {
            Item item = new Item("ItemName", "Description");
            sut.ItemList.Add(item);
            
            sut.RemoveItem(item);
            
            Assert.AreEqual(new List<IItem>(), sut.ItemList);
        }
        
        [Test]
        public void Test_EmptyInventory_EmptiesInventorySuccessfully()
        {
            Item item = new Item("ItemName", "Description");
            sut.AddItem(item);
            
            sut.EmptyInventory();
            
            Assert.AreEqual(new List<IItem>(), sut.ItemList);
        }
    }
}