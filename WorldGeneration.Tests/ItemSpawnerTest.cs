using NUnit.Framework;
using Moq;
using System.Diagnostics.CodeAnalysis;
using WorldGeneration.Models;
using WorldGeneration.Models.Interfaces;

namespace WorldGeneration.Tests
{
    [ExcludeFromCodeCoverage]  
    [TestFixture]
    
    public class ItemSpawnerTest
    {
        
        //Declaration and initialisation of constant variables
 
        //Declaration of variables
        private ItemSpawner sut;
        private Chunk _chunk;
        
        //Declaration of mocks
        private Mock<IFastNoise> _mockedNoise;
        private IFastNoise _mockedNoiseObject;
       
        
        [SetUp]
        public void Setup()
        {
            //Initialisation of variables
            sut = new ItemSpawner(_mockedNoiseObject);
            _chunk = new Chunk();

            //Initialisation of mocks
            _mockedNoise = new Mock<IFastNoise>();

            // _mockedNoise.Setup();
            
            _mockedNoiseObject = _mockedNoise.Object;

        }
        
        
        
        
    }
}