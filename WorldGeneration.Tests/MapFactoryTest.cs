using System.Diagnostics.CodeAnalysis;
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
 
        [SetUp]
        public void Setup()
        {
            //Initialisation of variables
            //Initialisation of mocks
        }
        
        [Test]
        public void Test_GenerateSeed_GeneratesARandomSeed() 
        {
            //Arrange ---------
            //Act ---------
            var seed1 = MapFactory.GenerateSeed();
            var seed2 = MapFactory.GenerateSeed();
            //Assert ---------
            Assert.That(seed1 != seed2);
        }
    }
}