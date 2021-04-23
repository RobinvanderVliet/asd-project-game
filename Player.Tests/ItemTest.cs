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
            sut = new Item("ItemName", "Description");
        }
        
        [Test]
        public void Test_GetItemName_GetsItemNameSuccessfully()
        {
            Assert.AreEqual("ItemName", sut.ItemName);
        }
        
        [Test]
        public void Test_SetItemName_SetsItemNameSuccessfully()
        {
            var itemName = "New name";
            sut.ItemName = itemName;
            
            Assert.AreEqual(itemName, sut.ItemName);
        }
        
        [Test]
        public void Test_GetDescription_GetsDescriptionSuccessfully()
        {
            Assert.AreEqual("Description", sut.Description);
        }
        
        [Test]
        public void Test_SetDescription_SetsDescriptionSuccessfully()
        {
            var description = "New description";
            sut.Description = description;
            
            Assert.AreEqual(description, sut.Description);
        }
    }
}