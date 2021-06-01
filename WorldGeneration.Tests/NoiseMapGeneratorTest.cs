using System.Diagnostics.CodeAnalysis;
using Moq;
using NUnit.Framework;
using WorldGeneration.Models.HazardousTiles;
using WorldGeneration.Models.Interfaces;
using WorldGeneration.Models.TerrainTiles;

namespace WorldGeneration.Tests
{
    [ExcludeFromCodeCoverage]  
    [TestFixture]
    public class NoiseMapGeneratorTest
    {
        //Declaration and initialisation of constant variables
 
        //Declaration of variables
        private NoiseMapGenerator sut;
        private int _coordinateX;
        private int _coordinateY;
 
        //Declaration of mocks
        private Mock<IFastNoise> _mockedNoise;
        private IFastNoise _mockedNoiseObject;
 
        [SetUp]
        public void Setup()
        {
            //Initialisation of variables
            sut = new NoiseMapGenerator();
            //Initialisation of mocks
            _mockedNoise = new Mock<IFastNoise>();
            _mockedNoiseObject = _mockedNoise.Object;
            
            sut.SetNoise(_mockedNoiseObject);
        }
        
        
        [Test]
        public void Test_Function_GetWaterTileFromNoise() 
        {
            //Arrange ---------
            _coordinateX = 2;
            _coordinateY = 3;
            float _noise = (float) -0.81;

            //Act ---------
            var result = sut.GetTileFromNoise(_noise, _coordinateX, _coordinateY);
            
            //Assert ---------
            Assert.IsInstanceOf<WaterTile>(result);
        }
        
        [Test]
        public void Test_Function_GetDirtTileFromNoise() 
        {
            //Arrange ---------
            _coordinateX = 2;
            _coordinateY = 3;
            float _noise = (float) -0.41;

            //Act ---------
            var result = sut.GetTileFromNoise(_noise, _coordinateX, _coordinateY);
            
            //Assert ---------
            Assert.IsInstanceOf<DirtTile>(result);
        }
        
        [Test]
        public void Test_Function_GetGrassTileFromNoise() 
        {
            //Arrange ---------
            _coordinateX = 2;
            _coordinateY = 3;
            float _noise = (float) 0.1;

            //Act ---------
            var result = sut.GetTileFromNoise(_noise, _coordinateX, _coordinateY);
            
            //Assert ---------
            Assert.IsInstanceOf<GrassTile>(result);
        }
        
        [Test]
        public void Test_Function_GetSpikeTileFromNoise() 
        {
            //Arrange ---------
            _coordinateX = 2;
            _coordinateY = 3;
            float _noise = (float) 0.24;

            //Act ---------
            var result = sut.GetTileFromNoise(_noise, _coordinateX, _coordinateY);
            
            //Assert ---------
            Assert.IsInstanceOf<SpikeTile>(result);
        }
        
        [Test]
        public void Test_Function_GetStreetTileFromNoise() 
        {
            //Arrange ---------
            _coordinateX = 2;
            _coordinateY = 3;
            float _noise = (float) 0.44;

            //Act ---------
            var result = sut.GetTileFromNoise(_noise, _coordinateX, _coordinateY);
            
            //Assert ---------
            Assert.IsInstanceOf<StreetTile>(result);
        }
        
        
        [Test]
        public void Test_Function_GetGasTileFromNoise() 
        {
            //Arrange ---------
            _coordinateX = 2;
            _coordinateY = 3;
            float _noise = (float) 1;

            //Act ---------
            var result = sut.GetTileFromNoise(_noise, _coordinateX, _coordinateY);
            
            //Assert ---------
            Assert.IsInstanceOf<GasTile>(result);
        }
                
        [Test]
        public void Test_Function_GetTileFromNoiseOutlier() 
        {
            //Arrange ---------
            _coordinateX = 2;
            _coordinateY = 3;
            float _noise = (float) 0;

            //Act ---------
            var result = sut.GetTileFromNoise(_noise, _coordinateX, _coordinateY);
            
            //Assert ---------
            Assert.IsInstanceOf<GrassTile>(result);
        }
        
        [Test]
        public void Test_Function_DoesThing() 
        {
            //Arrange ---------
            
            //Act ---------
            
            //Assert ---------
            Assert.That(true);
        }
        
        
        
    }
}