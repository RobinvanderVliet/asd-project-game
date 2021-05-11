using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DatabaseHandler.Repository;
using DatabaseHandler.Services;
using Moq;
using NUnit.Framework;
using WorldGeneration.Models;

namespace DatabaseHandler.Tests
{
    [ExcludeFromCodeCoverage]  
    [TestFixture]
    public class ChunkServicesTest
    {
        private ChunkFaker _chunkFaker;
        private MockRepository _mockRepository;
        private Mock<IChunkRepository> _repository;
        private IChunkServices _services;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new MockRepository(MockBehavior.Default);
            _repository = _mockRepository.Create<IChunkRepository>();
            _chunkFaker = new ChunkFaker();
        }
        
        /// <summary>
        /// CreateAsync(Chunk)
        ///
        /// The CreateAsync method from ChunkServices
        /// should successfully insert a record.
        /// </summary>
        [Test]
        public void Test_CreateAsync_Chunk()
        {
            // Arrange
            var chunk = _chunkFaker.Generate();
            _repository.Setup(chunkRepo => chunkRepo.CreateAsync(It.IsAny<Chunk>())).ReturnsAsync(1);
            
            // Act
            _services = new ChunkServices(_repository.Object);
            var result = _services.CreateAsync(chunk);

            // Assert
            Assert.AreEqual(1, result.Result);
        }
        
        /// <summary>
        /// ReadAsync(Chunk)
        ///
        /// The ReadAsync method from ChunkServices
        /// should successfully return a Chunk object.
        /// </summary>
        [Test]
        public void Test_ReadAsync_Chunk()
        {
            // Arrange
            var chunk = _chunkFaker.Generate();
            _repository.Setup(chunkRepo => chunkRepo.ReadAsync(It.IsAny<Chunk>())).ReturnsAsync(
                new Chunk {
                    X = chunk.X,
                    Y = chunk.Y,
                    RowSize = chunk.RowSize,
                    Map = chunk.Map
                }
            );
            
            // Act
            _services = new ChunkServices(_repository.Object);
            var result = _services.ReadAsync(chunk).Result;

            // Assert
            Assert.AreEqual(chunk.X, result.X);
            Assert.AreEqual(chunk.Y, result.Y);
            Assert.AreEqual(chunk.RowSize, result.RowSize);
            // arrays not supported
            // Assert.AreSame(chunk.Map, result.Map);
        }
        
        /// <summary>
        /// UpdateAsync(Chunk)
        ///
        /// The UpdateAsync method from ChunkServices
        /// should successfully update Chunk object and
        /// return the updated Chunk object.
        /// </summary>
        [Test]
        public void Test_UpdateAsync_Chunk()
        {
            // Arrange
            var chunk = _chunkFaker.Generate();
            _repository.Setup(chunkRepo => chunkRepo.UpdateAsync(It.IsAny<Chunk>())).ReturnsAsync(
                new Chunk {
                    X = chunk.X,
                    Y = chunk.Y,
                    RowSize = chunk.RowSize,
                    Map = chunk.Map
                }
            );
            
            // Act
            _services = new ChunkServices(_repository.Object);
            var result = _services.UpdateAsync(chunk).Result;

            // Assert
            Assert.AreEqual(chunk.X, result.X);
            Assert.AreEqual(chunk.Y, result.Y);
            Assert.AreEqual(chunk.RowSize, result.RowSize);
            // arrays not supported
            // Assert.AreSame(chunk.Map, result.Map);
        }
        
        /// <summary>
        /// GetAllAsync()
        ///
        /// The GetAllAsync method from ChunkServices
        /// should return a collection of Chunks in a List object.
        /// </summary>
        [Test]
        public void Test_GetAllAsyncChunksCountEquals_Chunks()
        {
            // Arrange
            var chunks = _chunkFaker.Generate(10);
            _repository.Setup(chunkRepo => chunkRepo.GetAllAsync()).ReturnsAsync(chunks);
            
            // Act
            _services = new ChunkServices(_repository.Object);
            var result = _services.GetAllAsync().Result.ToList();

            // Assert
            Assert.AreEqual(chunks.Count, result.Count);
        }
        
        /// <summary>
        /// DeleteAllAsync()
        ///
        /// The DeleteAllAsync method from ChunkServices
        /// should return delete all chunks from collection.
        /// </summary>
        [Test]
        public void Test_DeleteAllAsyncChunks_Chunks()
        {
            // Arrange
            var chunks = _chunkFaker.Generate(10);
            _repository.Setup(chunkRepo => chunkRepo.GetAllAsync()).ReturnsAsync(chunks);
            _repository.Setup(chunkRepo => chunkRepo.DeleteAllAsync()).ReturnsAsync(() => 
            {
                chunks.Clear();
                return 1;
            });
            
            // Act
            _services = new ChunkServices(_repository.Object);
            var deleteAction = _services.DeleteAllAsync().Result;
            var getAllAction = _services.GetAllAsync().Result;

            // Assert
            Assert.AreEqual(1, deleteAction);
            Assert.AreEqual(0, getAllAction.Count());
        }
    }
}