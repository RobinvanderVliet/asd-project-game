using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Player.Model;

namespace Player.Tests
{
    [ExcludeFromCodeCoverage]
    public class ItemTest
    {
        private Item _sut;
        
        [SetUp]
        public void Setup()
        {
            _sut = new Item("ItemName", "Description");
        }
        
        [Test]
        public void Test_GetItemName_GetsItemNameSuccessfully()
        {
            Assert.AreEqual("ItemName", _sut.ItemName);
        }
        
        [Test]
        public void Test_SetItemName_SetsItemNameSuccessfully()
        {
            var itemName = "New name";
            _sut.ItemName = itemName;
            
            Assert.AreEqual(itemName, _sut.ItemName);
        }
        
        [Test]
        public void Test_GetDescription_GetsDescriptionSuccessfully()
        {
            Assert.AreEqual("Description", _sut.Description);
        }
        
        [Test]
        public void Test_SetDescription_SetsDescriptionSuccessfully()
        {
            var description = "New description";
            _sut.Description = description;
            
            Assert.AreEqual(description, _sut.Description);
        }
    }
}