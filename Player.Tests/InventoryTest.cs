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
        public void GetItemListGetsItemList()
        {
            Assert.AreEqual(new List<Item>(), sut.ItemList);
        }
        
        [Test]
        public void SetItemListSetsItemList()
        {
            var ItemList = new List<Item>();
            ItemList.Add(new Item("Naam", "Beschrijving"));
            sut.ItemList = ItemList;
            
            Assert.AreEqual(ItemList, sut.ItemList);
        }
        
        [Test]
        public void GetItemReturnsItem()
        {
            Item item = new Item("Naam", "Beschrijving");
            sut.ItemList.Add(item);
            
            Assert.AreEqual(item, sut.GetItem("Naam"));
        }
        
        [Test]
        public void GetItemReturnsNull()
        {
            // Item item = new Item("Naam", "Beschrijving");
            // sut.ItemList.Add(item);
            
            Assert.AreEqual(null, sut.GetItem("Naam"));
        }
        
        [Test]
        public void AddItemAddsItemToInventory()
        {
            Item item = new Item("Naam", "Beschrijving");
            
            sut.AddItem(item);
            
            Assert.AreEqual(item, sut.GetItem("Naam"));
        }
        
        [Test]
        public void RemoveItemIsActuallyRemoved()
        {
            Item item = new Item("Naam", "Beschrijving");
            sut.ItemList.Add(item);
            
            sut.RemoveItem(item);
            
            Assert.AreEqual(new List<Item>(), sut.ItemList);
        }
        
        [Test]
        public void EmptyInventoryEmptiesInventory()
        {
            Item item = new Item("Naam", "Beschrijving");
            sut.AddItem(item);
            
            sut.EmptyInventory();
            
            Assert.AreEqual(new List<Item>(), sut.ItemList);
        }
    }
}