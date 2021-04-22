using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Player.Model;

namespace Player.Tests
{
    [ExcludeFromCodeCoverage]
    public class BitcoinTest
    {
        private Bitcoin sut;
        
        [SetUp]
        public void Setup()
        {
            sut = new Bitcoin(20);
        }
        
        [Test]
        public void GetAmountGetsAmount()
        {
            Assert.AreEqual(20, sut.Amount);
        }
        
        [Test]
        public void SetAmountSetsAmount()
        {
            var amount = 5;
            sut.Amount = amount;
            
            Assert.AreEqual(amount, sut.Amount);
        }
        
        [Test]
        public void AddAmountAddsGivenAmount()
        {
            sut.AddAmount(20);
            
            Assert.AreEqual(40, sut.Amount);
        }
        
        [Test]
        public void RemoveAmountRemovesGivenAmount()
        {
            sut.RemoveAmount(10);
            
            Assert.AreEqual(10, sut.Amount);
        }
    }
}