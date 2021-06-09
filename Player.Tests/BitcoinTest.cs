using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Player.Model;

namespace Player.Tests
{
    [ExcludeFromCodeCoverage]
    public class BitcoinTest
    {
        private Bitcoin _sut;
        
        [SetUp]
        public void Setup()
        {
            _sut = new Bitcoin(20);
        }
        
        [Test]
        public void Test_GetAmount_GetsAmountSuccessfully()
        {
            //arrange
            //act
            //assert
            Assert.AreEqual(20, _sut.Amount);
        }
        
        [Test]
        public void Test_SetAmount_SetsAmountSuccessfully()
        {
            //arrange
            var amount = 5;
            _sut.Amount = amount;
            //act
            //assert
            Assert.AreEqual(amount, _sut.Amount);
        }
        
        [Test]
        public void Test_AddAmount_AddsGivenAmountSuccessfully()
        {
            //arrange
            _sut.AddAmount(20);
            //act
            //assert
            Assert.AreEqual(40, _sut.Amount);
        }
        
        [Test]
        public void Test_RemoveAmount_RemovesGivenAmountSuccessfully()
        {
            //arrange
            _sut.RemoveAmount(10);
            //act
            //assert
            Assert.AreEqual(10, _sut.Amount);
        }
    }
}