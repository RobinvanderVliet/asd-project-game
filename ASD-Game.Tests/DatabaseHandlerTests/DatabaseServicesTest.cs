using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ASD_Game.DatabaseHandler.Repository;
using ASD_Game.DatabaseHandler.Services;
using ASD_Game.World.Models;
using Moq;
using NUnit.Framework;

namespace ASD_Game.Tests.DatabaseHandlerTests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class DatabaseServicesTest
    {
        private ChunkFaker _chunkFaker;
        private MockRepository _mockRepository;
        private Mock<IRepository<Chunk>> _repository;
        private IDatabaseService<Chunk> _services;
        private IList<Chunk> _chunkInMemoryDatabase;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new MockRepository(MockBehavior.Default);
            _repository = _mockRepository.Create<IRepository<Chunk>>();
            _chunkFaker = new ChunkFaker();
            _chunkInMemoryDatabase = _chunkFaker.Generate(50);

            _repository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(() => _chunkInMemoryDatabase);
        }

        [ExcludeFromCodeCoverage]
        private Chunk GetRandomChunk()
        {
            var rnd = new Random();
            var r = rnd.Next(_chunkInMemoryDatabase.Count);
            return _chunkInMemoryDatabase[r];
        }

        /// <summary>
        /// CreateAsync(Chunk)
        ///
        /// The CreateAsync method from ChunkServices
        /// should successfully insert a record.
        /// </summary>
        [Test]
        public void Test_CreateAsync_CreateAChunk()
        {
            // Arrange
            var chunk = _chunkFaker.Generate();
            _repository.Setup(chunkRepo => chunkRepo.CreateAsync(It.IsAny<Chunk>())).ReturnsAsync((Chunk item) =>
            {
                _chunkInMemoryDatabase.Add(item);
                return "succeeded";
            });
            _services = new DatabaseService<Chunk>(_repository.Object);

            // Act
            var createChunk = _services.CreateAsync(chunk).Result;
            var result = _services.GetAllAsync().Result.FirstOrDefault(c => c.X == chunk.X && c.Y == chunk.Y);

            // Assert
            Assert.AreEqual(chunk.X, result.X);
            Assert.AreEqual(chunk.Y, result.Y);
            Assert.AreEqual(chunk.RowSize, result.RowSize);
        }

        /// <summary>
        /// CreateAsync(Chunk)
        ///
        /// The CreateAsync method from ChunkServices
        /// should throw exception because Chunk with
        /// X and Y already exist.
        /// </summary>
        [Test]
        public void Test_CreateAsync_ReturnsExceptionBecauseChunkAlreadyExist()
        {
            // Arrange
            var chunk = _chunkFaker.Generate();
            _repository.Setup(chunkRepo => chunkRepo.CreateAsync(It.IsAny<Chunk>())).ReturnsAsync((Chunk item) =>
            {
                if (_chunkInMemoryDatabase.Contains(item))
                {
                    throw new InvalidOperationException();
                }
                _chunkInMemoryDatabase.Add(item);
                return "succeeded";
            });
            _services = new DatabaseService<Chunk>(_repository.Object);

            // Act
            var createChunk = _services.CreateAsync(chunk).Result;

            // Assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                var errorChunk = _services.CreateAsync(chunk).Result;
            });
        }

        /// <summary>
        /// ReadAsync(Chunk)
        ///
        /// The ReadAsync method from ChunkServices
        /// should successfully return a Chunk object.
        /// </summary>
        [Test]
        public void Test_ReadAsync_GetsAChunk()
        {
            // Arrange
            var chunk = GetRandomChunk();
            _services = new DatabaseService<Chunk>(_repository.Object);

            // Act
            var result = _services.GetAllAsync().Result.FirstOrDefault(c => c.X == chunk.X && c.Y == chunk.Y);

            // Assert
            Assert.AreEqual(chunk.X, result.X);
            Assert.AreEqual(chunk.Y, result.Y);
            Assert.AreEqual(chunk.RowSize, result.RowSize);
            // arrays not supported
            // Assert.AreSame(chunk.Map, result.Map);
        }

        /// <summary>
        /// ReadAsync(Chunk)
        ///
        /// The ReadAsync method from ChunkServices
        /// should return null because the
        /// requested Chunk does not exist.
        /// </summary>
        [Test]
        public void Test_ReadAsync_ReturnsNullBecauseItDoesntExist()
        {
            // Arrange
            var chunk = new Chunk
            {
                X = 1337,
                Y = 1337
            };
            _services = new DatabaseService<Chunk>(_repository.Object);
            var result = _services.GetAllAsync().Result.FirstOrDefault(c => c.X == chunk.X && c.Y == chunk.Y);

            // Act & Assert
            Assert.AreEqual(null, result);
        }

        /// <summary>
        /// UpdateAsync(Chunk)
        ///
        /// The UpdateAsync method from ChunkServices
        /// should successfully update Chunk object and
        /// return the updated Chunk object.
        /// </summary>
        [Test]
        public void Test_UpdateAsync_UpdatesAChunk()
        {
            // Arrange
            var chunk = GetRandomChunk();
            _repository.Setup(chunkRepo => chunkRepo.UpdateAsync(It.IsAny<Chunk>())).ReturnsAsync(
                (Chunk item) =>
                {
                    var returnVal = _chunkInMemoryDatabase.Single(c => c.Equals(item));
                    returnVal.RowSize = item.RowSize;
                    return 1;
                }
            );
            _services = new DatabaseService<Chunk>(_repository.Object);

            // Act
            chunk.RowSize = 1337;
            var updateChunk = _services.UpdateAsync(chunk).Result;
            var result = _services.GetAllAsync().Result.FirstOrDefault(c => c.X == chunk.X && c.Y == chunk.Y);

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
        /// should throw an exception because the
        /// requested Chunk does not exist.
        /// </summary>
        [Test]
        public void Test_UpdateAsync_ReturnsExceptionBecauseItDoesntExist()
        {
            // Arrange
            _repository.Setup(chunkRepo => chunkRepo.UpdateAsync(It.IsAny<Chunk>())).ReturnsAsync(
                (Chunk item) =>
                {
                    var returnVal = _chunkInMemoryDatabase.Single(c => c.X == item.X && c.Y == item.Y);
                    returnVal.RowSize = item.RowSize;
                    throw new InvalidOperationException();
                }
            );
            _services = new DatabaseService<Chunk>(_repository.Object);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                var result = _services.UpdateAsync(new Chunk
                {
                    X = 1337,
                    Y = 1337
                }).Result;
            });
        }

        /// <summary>
        /// DeleteAsync(Chunk)
        ///
        /// The DeleteAsync method from ChunkServices
        /// should successfully delete Chunk object and
        /// return a boolean value.
        /// </summary>
        [Test]
        public void Test_DeleteAsync_DeletesAChunk()
        {
            // Arrange
            var chunk = GetRandomChunk();
            _repository.Setup(chunkRepo => chunkRepo.DeleteAsync(It.IsAny<Chunk>())).ReturnsAsync(
                (Chunk item) => _chunkInMemoryDatabase.Remove(item) ? 1 : 0);
            _services = new DatabaseService<Chunk>(_repository.Object);

            // Act
            var result = _services.DeleteAsync(chunk).Result;
            var checkChunk = _services.GetAllAsync().Result.FirstOrDefault(c => c.X == chunk.X && c.Y == chunk.Y);

            // Assert
            Assert.AreEqual(1, result);
            Assert.AreEqual(null, checkChunk);
        }

        /// <summary>
        /// DeleteAsync(Chunk)
        ///
        /// The DeleteAsync method from ChunkServices
        /// should throw an exception because the
        /// requested Chunk does not exist.
        /// </summary>
        [Test]
        public void Test_DeleteAsync_ReturnsExceptionBecauseItDoesntExist()
        {
            // Arrange
            _repository.Setup(chunkRepo => chunkRepo.DeleteAsync(It.IsAny<Chunk>())).ReturnsAsync(
                (Chunk item) => _chunkInMemoryDatabase.Remove(item) ? 1 : throw new InvalidOperationException());
            _services = new DatabaseService<Chunk>(_repository.Object);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                var result = _services.DeleteAsync(new Chunk
                {
                    X = 1337,
                    Y = 1337
                }).Result;
            });
        }

        /// <summary>
        /// GetAllAsync()
        ///
        /// The GetAllAsync method from ChunkServices
        /// should return a collection of Chunks in a List object.
        /// </summary>
        [Test]
        public void Test_GetAllAsync_GetsAllChunks()
        {
            // Arrange
            _repository.Setup(chunkRepo => chunkRepo.GetAllAsync()).ReturnsAsync(_chunkInMemoryDatabase);
            _services = new DatabaseService<Chunk>(_repository.Object);

            // Act
            var expected = _chunkInMemoryDatabase;
            var result = _services.GetAllAsync().Result;

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
        public void Test_DeleteAllAsync_DeletesAllChunks()
        {
            // Arrange
            _repository.Setup(chunkRepo => chunkRepo.GetAllAsync()).ReturnsAsync(_chunkInMemoryDatabase);
            _repository.Setup(chunkRepo => chunkRepo.DeleteAllAsync()).ReturnsAsync(() =>
            {
                _chunkInMemoryDatabase.Clear();
                return 1;
            });
            _services = new DatabaseService<Chunk>(_repository.Object);

            // Act
            var deleteAction = _services.DeleteAllAsync().Result;
            var getAllAction = _services.GetAllAsync().Result;

            // Assert
            Assert.AreEqual(1, deleteAction);
            Assert.AreEqual(0, getAllAction.Count());
        }
    }
}