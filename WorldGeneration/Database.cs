/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD project.
 
    This file is created by team: 3.
     
    Goal of this file: All data transfer with the database.
     
*/

using System;
using System.Collections.Generic;
using System.Linq;
using LiteDB;

namespace WorldGeneration
{
    public class Database
    {
        private String databaseLocation;
        private String mapCollection;
        
        public Database(String databaseLocation = "C:\\Temp\\ChunkDatabase.db", String mapCollection = "Chunks")
        {
            this.databaseLocation = databaseLocation;
            this.mapCollection = mapCollection;
        }

        //read function name
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

        //returns Chunk from database after finding it by Chunk x and y.
        public Chunk getChunk(int chunkXValue, int chunkYValue)
        {
            try
            {
                using (var db = new LiteDatabase(databaseLocation))
                {
                    var collection = this.getMapCollection(db);
                    var results = collection.Query()
                        .Where(chunk => chunk.x.Equals(chunkXValue) && chunk.y.Equals(chunkYValue))
                        .Select(queryOutput => new {queryOutput.map, queryOutput.rowSize, queryOutput.x, queryOutput.y})
                        .ToArray();

                    switch (results.Length)
                    {
                        case 0:
                            throw new DatabaseError("There were no matching chunks found");
                        case >1:
                            throw new DatabaseError("There were multiple matching chunks found. bad! this bad!");
                        case 1:
                            return new Chunk(results.First().x, results.First().y, results.First().map, results.First().rowSize);
                        default:
                            throw new DatabaseError("Extremely unexpected result from query. like, this is only here in case of a count being negative or null. So pretty much unreachable code.");
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
        
        //returns all Chunks from the database in a list. Throws a error if there are no Chunks.
        public List<Chunk> getAllChunks()
        {
            try
            {
                using (var db = new LiteDatabase(databaseLocation))
                {
                    var results = getMapCollection(db).Query()
                        .Select(queryOutput => new {queryOutput.map, queryOutput.rowSize, queryOutput.x, queryOutput.y})
                        .ToList();
                

                    switch (results.Count)
                    {
                        case 0:
                            throw new DatabaseError("There were no matching chunks found");
                        case >0:
                            return results.Select(result => new Chunk(result.x, result.y, result.map, result.rowSize)).ToList();
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
        }

        //Drops the Chunks collection.
        public void deleteTileMap()
        {
            try
            {
                using (var db = new LiteDatabase(databaseLocation))
                {
                    db.DropCollection(mapCollection);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        //Returns the collection connection.
        public ILiteCollection<Chunk> getMapCollection(LiteDatabase db)
        {
            return db.GetCollection<Chunk>(mapCollection);
        }
    }
}