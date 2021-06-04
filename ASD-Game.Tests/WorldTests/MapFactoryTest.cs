using System.Diagnostics.CodeAnalysis;
using Moq;
using NUnit.Framework;

namespace WorldGeneration.Tests
{
    [ExcludeFromCodeCoverage]  
    [TestFixture]
    public class MapFactoryTest
    {
        //Declaration and initialisation of constant variables
 
        //Declaration of variables
 
        //Declaration of mocks
        private MapFactory _sut;

        [SetUp]
        public void Setup()
        {
            //Initialisation of variables
            //Initialisation of mocks
            _sut = new MapFactory();
        }
        
        [Test]
        public void Test_GenerateSeed_GeneratesARandomSeed() 
        {
            //Arrange ---------
            //Act ---------
            var seed1 = _sut.GenerateSeed();
            var seed2 = _sut.GenerateSeed();
            //Assert ---------
            Assert.That(seed1 != seed2);
        }
    }
}