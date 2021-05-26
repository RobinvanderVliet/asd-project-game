using System;
using Moq;
using NUnit.Framework;

namespace Session.Tests
{
    public class GuidServiceTest
    {

        private GuidService _sut;
        private Mock<IGuidService> _mockedGuidService;
        
        [SetUp]
        public void Setup()
        {
            _mockedGuidService = new Mock<IGuidService>();
            _sut = new GuidService();
        }


        [Test]
        public void Test_NewGuidReturnsGuid()
        {
            //Arrange
            var result = _sut.NewGuid();
            
            //Assert
            Assert.That(result, Is.TypeOf<Guid>());
        }
    }
}