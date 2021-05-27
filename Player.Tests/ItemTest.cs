using NUnit.Framework;
using Player.Model;
using System.Diagnostics.CodeAnalysis;

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
            //arrange
            //act
            //assert
            Assert.AreEqual("ItemName", _sut.ItemName);
        }

        [Test]
        public void Test_SetItemName_SetsItemNameSuccessfully()
        {
            //arrange
            var itemName = "New name";
            _sut.ItemName = itemName;
            //act
            //assert
            Assert.AreEqual(itemName, _sut.ItemName);
        }

        [Test]
        public void Test_GetDescription_GetsDescriptionSuccessfully()
        {
            //arrange
            //act
            //assert
            Assert.AreEqual("Description", _sut.Description);
        }

        [Test]
        public void Test_SetDescription_SetsDescriptionSuccessfully()
        {
            //arrange
            var description = "New description";
            _sut.Description = description;
            //act
            //assert
            Assert.AreEqual(description, _sut.Description);
        }
    }
}