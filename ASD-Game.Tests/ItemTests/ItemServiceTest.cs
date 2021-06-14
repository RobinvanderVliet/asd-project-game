using System.Diagnostics.CodeAnalysis;
using ASD_Game.ActionHandling;
using ASD_Game.Items;
using ASD_Game.Items.Services;
using ASD_Game.World;
using Moq;
using NUnit.Framework;

namespace ASD_Game.Tests.ItemTests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class ItemServiceTest
    {
        //Declaration and initialisation of variables
        private ItemService _sut;
        //Declaration of mocks
        private Mock<ISpawnHandler> _spawnHandlerMock;
        private Mock<IRandomItemGenerator> _randomItemGeneratorMock;

        [SetUp]
        public void Setup()
        {
            _spawnHandlerMock = new Mock<ISpawnHandler>();
            _randomItemGeneratorMock = new Mock<IRandomItemGenerator>();
            _sut = new ItemService(_spawnHandlerMock.Object, _randomItemGeneratorMock.Object);
        }

        [Test]
        public void GetSpawnHandlerTest()
        {
            // Act
            var result = _sut.GetSpawnHandler();
            // Assert
            Assert.AreEqual(_spawnHandlerMock.Object, result);
        }

        [Test]
        public void GenerateItemFromNoiseNullItemTest()
        {
            // Arrange
            _randomItemGeneratorMock.Setup(mock => mock.GetRandomItem(It.IsAny<float>())).Returns(null as Item);
            // Act
            var result = _sut.GenerateItemFromNoise(0f, 0, 0);
            // Assert
            Assert.IsNull(result);
        }
        
        [Test]
        public void GenerateItemFromNoiseActualItemTest()
        {
            // Arrange
            _randomItemGeneratorMock.Setup(mock => mock.GetRandomItem(It.IsAny<float>())).Returns(ItemFactory.GetAK47());
            // Act
            var result = _sut.GenerateItemFromNoise(0f, 0, 0);
            // Assert
            _spawnHandlerMock.Verify(mock => mock.SendSpawn(0, 0, It.IsAny<Item>()));
        }
        
    }
}