using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using WorldGeneration.Models;

namespace WorldGeneration.DatabaseFunctions
{
    public class Database
    {
        private readonly string _databaseLocation;
        private readonly string _mapCollection;

        public Database(string databaseLocation, string mapCollection)
        {
            _databaseLocation = databaseLocation;
            _mapCollection = mapCollection;
        }

        // read function name
        public void InsertChunkIntoDatabase(Chunk chunk)
        {
            try
            {
                using var db = new LiteDatabase(_databaseLocation);
                GetMapCollection(db).Insert(chunk);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // returns Chunk from database after finding it by Chunk x and y.
        public Chunk GetChunk(int chunkXValue, int chunkYValue)
        {
            try
            {
                using var db = new LiteDatabase(_databaseLocation);
                var collection = GetMapCollection(db);
                var queryresults = collection.Query()
                    .Where(chunk => chunk.X.Equals(chunkXValue) && chunk.Y.Equals(chunkYValue))
                    .Select(queryOutput => new
                    { map = queryOutput.Map, rowSize = queryOutput.RowSize, x = queryOutput.X, y = queryOutput.Y });
                switch (queryresults.Count())
                {
                    case 0:
                        return null;
                    case > 1:
                        throw new DatabaseException("There were multiple matching chunks found where a single was expected");
                    case 1:
                        return new Chunk(queryresults.First().x, queryresults.First().y, queryresults.First().map,
                            queryresults.First().rowSize);
                    default:
                        throw new DatabaseException(
                            "Result count is negative or null");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // returns all Chunks from the database in a list. Throws a error if there are no Chunks.
        public IEnumerable<Chunk> GetAllChunks()
        {
            try
            {
                using var db = new LiteDatabase(_databaseLocation);
                var results = GetMapCollection(db).Query()
                    .Select(queryOutput => new
                    { map = queryOutput.Map, rowSize = queryOutput.RowSize, x = queryOutput.X, y = queryOutput.Y })
                    .ToList();


                switch (results.Count)
                {
                    case 0:
                        throw new DatabaseException("There were no matching chunks found");
                    case > 0:
                        return results.Select(result => new Chunk(result.x, result.y, result.map, result.rowSize))
                            .ToList();
                    default:
                        throw new DatabaseException(
                            "Extremely unexpected result from query. like, this is only here in case of a count being negative. So pretty much unreachable code.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // Drops the Chunks collection.
        public void DeleteTileMap()
        {
            try
            {
                using var db = new LiteDatabase(_databaseLocation);
                db.DropCollection(_mapCollection);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // Returns the collection connection.    
        private ILiteCollection<Chunk> GetMapCollection(ILiteDatabase db)
        {
            return db.GetCollection<Chunk>(_mapCollection);
        }
    }
}