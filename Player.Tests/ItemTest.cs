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
        public void Test_GetItemName_GetsItemNameSuccessfully()
        {
            Assert.AreEqual("Naam", sut.ItemName);
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
            Assert.AreEqual("Beschrijving", sut.Description);
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