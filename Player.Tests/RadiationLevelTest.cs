using NUnit.Framework;
using Player.Model;
using System.Diagnostics.CodeAnalysis;

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
            //arrange
            //act
            //assert
            Assert.AreEqual(1, _sut.Level);
        }

        [Test]
        public void Test_SetLevel_SetsLevelSuccessfully()
        {
            //arrange
            var level = 5;
            //act
            _sut.Level = level;
            //assert
            Assert.AreEqual(level, _sut.Level);
        }
    }
}