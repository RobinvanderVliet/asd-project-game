using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Player.Model;

namespace Player.Tests
{
    [ExcludeFromCodeCoverage]
    public class ItemTest
    {
        private Item sut;
        
        [SetUp]
        public void Setup()
        {
            sut = new Item("Naam", "Beschrijving");
        }
        
        [Test]
        public void GetItemNameGetsItemName()
        {
            Assert.AreEqual("Naam", sut.ItemName);
        }
        
        [Test]
        public void SetItemNameSetsItemName()
        {
            var itemName = "New name";
            sut.ItemName = itemName;
            
            Assert.AreEqual(itemName, sut.ItemName);
        }
        
        [Test]
        public void GetDescriptionGetsDescription()
        {
            Assert.AreEqual("Beschrijving", sut.Description);
        }
        
        [Test]
        public void SeDescriptionSetsDescription()
        {
            var description = "New description";
            sut.Description = description;
            
            Assert.AreEqual(description, sut.Description);
        }
    }
}