using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace WorldGeneration.Tests
{
    [ExcludeFromCodeCoverage]  
    [TestFixture]
    public class WorldServiceTest
    {
        //Declaration and initialisation of constant variables
 
        //Declaration of variables
 
        //Declaration of mocks
        private WorldService _sut;

        [SetUp]
        public void Setup()
        {
            //Initialisation of variables
            //Initialisation of mocks
            _sut = new WorldService();
        }
        
        [Test]
        public void Test_GenerateSeed_GeneratesARandomSeed() 
        {
            //Arrange ---------
            //Act ---------
            //Assert ---------
        }

    }
}