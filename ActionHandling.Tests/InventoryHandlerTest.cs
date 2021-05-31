using Moq;
using Network;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration;

namespace ActionHandling.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class InventoryHandlerTest
    {
        private InventoryHandler _sut;
        private Mock<IClientController> _mockedClientController;
        private Mock<IWorldService> _mockedWorldService;

        [SetUp]
        public void Setup()
        {
            _mockedClientController = new Mock<IClientController>();
            _mockedWorldService = new Mock<IWorldService>();
            _sut = new InventoryHandler(_mockedClientController.Object, _mockedWorldService.Object);
        }

        [Test]
        public void Test_Search_CallsWorldService()
        {
            //arrange
            string exampleResult = "result";

            _mockedWorldService.Setup(mock => mock.SearchCurrentTile()).Returns(exampleResult);

            //act
            _sut.Search();

            //assert
            _mockedWorldService.Verify(mock => mock.SearchCurrentTile(), Times.Once);
        }



    }
}
