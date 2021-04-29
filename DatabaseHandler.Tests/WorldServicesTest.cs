using System.Threading.Tasks;
using DatabaseHandler.Repository;
using DatabaseHandler.Services;
using Moq;
using NUnit.Framework;

namespace DatabaseHandler.Tests
{
    public class WorldServicesTest
    {
        private MockRepository _mockRepository;
        private Mock<IChunkRepository> _repository;
        private IChunkServices _services;
        private ChunkFaker _chunkFaker;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new MockRepository(MockBehavior.Default);
            _repository = _mockRepository.Create<IChunkRepository>();
            _chunkFaker = new ChunkFaker();
        }

        [Test]
        public async Task Test_CreateAsync_Chunk()
        {
            _services = new ChunkServices(null, _repository.Object);
            var generatedChunk = _chunkFaker.Generate();
            var createdChunk = await _services.CreateAsync(generatedChunk);

            Assert.AreEqual(generatedChunk, createdChunk);
        }
    }
}