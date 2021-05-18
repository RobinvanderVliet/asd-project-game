using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Player.Model;

namespace Player.Tests
{
    [ExcludeFromCodeCoverage]
    public class RadiationLevelTest
    {
        private RadiationLevel _sut;
        
        [SetUp]
        public void Setup()
        {
            _sut = new RadiationLevel(1);
        }
        
        [Test]
        public void Test_GetLevel_GetsLevelSuccessfully()
        {
            Assert.AreEqual(1, _sut.Level);
        }
        
        [Test]
        public void Test_SetLevel_SetsLevelSuccessfully()
        {
            var level = 5;
            _sut.Level = level;
            
            Assert.AreEqual(level, _sut.Level);
        }
    }
}