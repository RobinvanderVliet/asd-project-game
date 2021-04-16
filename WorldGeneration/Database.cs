using System;
using System.Collections.Generic;
using LiteDB;

namespace WorldGeneration
{
    public class Database
    {
        private String databaseLocation;
        private String mapCollection;


        public Database()
        {
            databaseLocation = "C:\\Temp\\ChunkDatabase.db";
            mapCollection = "Chunks";
        }

        public void insertChunkIntoDatabase(Chunk chunk)
        {
            try
            {
                using (var db = new LiteDatabase(databaseLocation))
                {
                    getMapCollection(db).Insert(chunk);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public Chunk getChunk(int lookingForX, int lookingForY)
        {
            try
            {
                using (var db = new LiteDatabase(databaseLocation))
                {
                    var results = getMapCollection(db).Query()
                        .Where(chunk => chunk.x.Equals(lookingForX) && chunk.y.Equals(lookingForY))
                        .Select(queryOutput => new Chunk(queryOutput.x, queryOutput.y, queryOutput.map))
                        .ToList();

                    switch (results.Count)
                    {
                        case 0:
                            throw new DatabaseError("There were no matching chunks found");
                        case >1:
                            throw new DatabaseError("There were multiple matching chunks found. bad! this bad!");
                        case 1:
                            foreach (var result in results)
                            {
                                Console.WriteLine("waarde: " + result);
                                return result;
                            }
                            break;
                        default:
                            throw new DatabaseError("Extremely unexpected result from query. like, this is only here in case of a count being negative. So pretty much unreachable code.");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return null;
        }
        
        public List<Chunk> getAllChunks()
        {
            try
            {
                using (var db = new LiteDatabase(databaseLocation))
                {
                    var results = getMapCollection(db).Query()
                        .Select(queryOutput => new Chunk(queryOutput.x, queryOutput.y, queryOutput.map))
                        .ToList();

                    switch (results.Count)
                    {
                        case 0:
                            throw new DatabaseError("There were no matching chunks found");
                        case >0:
                            return results;
                        default:
                            throw new DatabaseError("Extremely unexpected result from query. like, this is only here in case of a count being negative. So pretty much unreachable code.");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return null;
        }

        public ILiteCollection<Chunk> getMapCollection(LiteDatabase db)
        {
            return db.GetCollection<Chunk>(mapCollection);
        }
    }
}