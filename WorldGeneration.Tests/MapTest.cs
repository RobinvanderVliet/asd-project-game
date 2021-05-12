using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Moq;

namespace WorldGeneration.Tests
{
    
    [ExcludeFromCodeCoverage]  
    [TestFixture]
    public class MapTest
    {
        //Declaration and initialisation of constant variables
 
        //Declaration of variables
        private Map sut;
 
        //Declaration of mocks
 
        [SetUp]
        public void Setup()
        {
            //Initialisation of variables
            sut = new Map();
            //Initialisation of mocks
        }
        
        [Test]
        public void Test_Map_DoesntThrowException() 
        {
            //Arrange ---------
            //Act ---------
            var map1 = new Map();
            var map2 = new Map(2,51,"c:\\temp\\db.db", "test");
            //Assert ---------
        }
        
        [Test]
        public void Test_DisplayMap_DoesntThrowException() 
        {
            //Arrange ---------
            //Act ---------
            var map1 = new Map();
            var map2 = new Map(2,51,"c:\\temp\\db.db", "test");
            //Assert ---------
        }
        
        [Test]
        public void Test_DisplayMap_DoesntLoadTooBigArea() 
        {
            //Arrange ---------
            //Act ---------
            var map1 = new Map();
            var map2 = new Map(2,51,"c:\\temp\\db.db", "test");
            //Assert ---------
        }
        
        [Test]
        public void Test_DisplayMap_DoesntLoadTooSmallArea() 
        {
            //Arrange ---------
            //Act ---------
            var map1 = new Map();
            var map2 = new Map(2,51,"c:\\temp\\db.db", "test");
            //Assert ---------
        }
    }
}