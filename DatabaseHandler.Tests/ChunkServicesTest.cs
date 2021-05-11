using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseHandler.Repository;
using DatabaseHandler.Services;
using Moq;
using NUnit.Framework;
using WorldGeneration.Models;

namespace DatabaseHandler.Tests
{
    [TestFixture]
    public class ChunkServicesTest
    {
        private ChunkFaker _chunkFaker;
        private MockRepository _mockRepository;
        private Mock<IChunkRepository> _repository;
        private IChunkServices _services;
        private IList<Chunk> _chunkInMemoryDatabase; 

        [SetUp]
        public void Setup()
        {
            _mockRepository = new MockRepository(MockBehavior.Default);
            _repository = _mockRepository.Create<IChunkRepository>();
            _chunkFaker = new ChunkFaker();
            _chunkInMemoryDatabase = _chunkFaker.Generate(50);
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
            _repository.Setup(repo => repo.ReadAsync(It.IsAny<Chunk>()))
                .ReturnsAsync((Chunk chunk) => _chunkInMemoryDatabase.Single(c => c.X == chunk.X && c.Y == chunk.Y));
            _repository.Setup(chunkRepo => chunkRepo.CreateAsync(It.IsAny<Chunk>())).ReturnsAsync((Chunk item) =>
            {
                _chunkInMemoryDatabase.Add(item);
                return 1;
            });
            
            // Act
            _services = new ChunkServices(_repository.Object);
            var createChunk = _services.CreateAsync(chunk).Result;
            var result = _services.ReadAsync(chunk).Result;

            // Assert
            Assert.AreEqual(chunk.X, result.X);
            Assert.AreEqual(chunk.Y, result.Y);
            Assert.AreEqual(chunk.RowSize, result.RowSize);
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
            var rnd = new Random();
            var r = rnd.Next(_chunkInMemoryDatabase.Count);
            var chunk = _chunkInMemoryDatabase[r];
            _repository.Setup(repo => repo.ReadAsync(It.IsAny<Chunk>()))
                .ReturnsAsync((Chunk chunk) => _chunkInMemoryDatabase.Single(c => c.X == chunk.X && c.Y == chunk.Y));

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
            var rnd = new Random();
            var r = rnd.Next(_chunkInMemoryDatabase.Count);
            var chunk = _chunkInMemoryDatabase[r];
            _repository.Setup(chunkRepo => chunkRepo.UpdateAsync(It.IsAny<Chunk>())).ReturnsAsync(
                (Chunk item) =>
                {
                    var returnVal = _chunkInMemoryDatabase.Single(c => c.X == chunk.X && c.Y == chunk.Y);
                    returnVal.RowSize = item.RowSize;
                    return returnVal;
                }
            );
            
            // Act
            _services = new ChunkServices(_repository.Object);
            chunk.RowSize = 1337;
            var result = _services.UpdateAsync(chunk).Result;

            // Assert
            Assert.AreEqual(chunk.X, result.X);
            Assert.AreEqual(chunk.Y, result.Y);
            Assert.AreEqual(chunk.RowSize, result.RowSize);
            // arrays not supported
            // Assert.AreSame(chunk.Map, result.Map);
        }
        
        /// <summary>
        /// DeleteAsync(Chunk)
        ///
        /// The DeleteAsync method from ChunkServices
        /// should successfully delete Chunk object and
        /// return a boolean value.
        /// </summary>
        [Test]
        public void Test_DeleteAsync_Chunk()
        {
            // Arrange
            var rnd = new Random();
            var r = rnd.Next(_chunkInMemoryDatabase.Count);
            var chunk = _chunkInMemoryDatabase[r];
            _repository.Setup(repo => repo.ReadAsync(It.IsAny<Chunk>()))
                .ReturnsAsync((Chunk chunk) => _chunkInMemoryDatabase.Single(c => c.X == chunk.X && c.Y == chunk.Y));

            _repository.Setup(chunkRepo => chunkRepo.DeleteAsync(It.IsAny<Chunk>())).ReturnsAsync(
                (Chunk item) => _chunkInMemoryDatabase.Remove(item) ? 1 : 0);
            
            // Act
            _services = new ChunkServices(_repository.Object);
            var result = _services.DeleteAsync(chunk).Result;
            
            // Assert
            Assert.AreEqual(1, result);
            Assert.Throws<AggregateException>(() =>
            {
                var checkChunk = _services.ReadAsync(chunk).Result;
            });

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
            _repository.Setup(chunkRepo => chunkRepo.GetAllAsync()).ReturnsAsync(_chunkInMemoryDatabase);
            
            // Act
            _services = new ChunkServices(_repository.Object);
            var expected = _chunkInMemoryDatabase;
            var result = _services.GetAllAsync().Result.ToList();

            // Assert
            Assert.That(result, Has.All.Matches<Chunk>(f => expected.Any(e =>
                f.X == e.X &&
                f.Y == e.Y &&
                f.RowSize == e.RowSize)));
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
            _repository.Setup(chunkRepo => chunkRepo.GetAllAsync()).ReturnsAsync(_chunkInMemoryDatabase);
            _repository.Setup(chunkRepo => chunkRepo.DeleteAllAsync()).ReturnsAsync(() => 
            {
                _chunkInMemoryDatabase.Clear();
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