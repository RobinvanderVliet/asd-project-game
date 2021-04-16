using System;
using System.Drawing;
using System.Linq;
using LiteDB;

namespace WorldGeneration
{
    public class Class1
    {
        public Class1()
        {
            Console.WriteLine("Game is gestartAAAAAAAAAAAAAA");

            var lookingForX = 1;
            var lookingForY = 1;

            using (var db = new LiteDatabase(@"C:\Temp\ChunkDatabase.db"))
            {
                // Get a collection (or create, if doesn't exist)
                db.DropCollection("Chunks");
                var collection = db.GetCollection<Chunk>("Chunks");
            
                var grass = new TileType()
                {
                    symbol = '+',
                    color = Color.Green
                };

                var grassTile = new Tile()
                {
                    gasLevel = 0,
                    tileType = grass
                };
                Tile[,] tileMap = {{grassTile, grassTile}, {grassTile, grassTile}};
                
                // Create your new chunk instance
                var chunk = new Chunk()
                {
                    x = 1,
                    y = 2,
                    map = tileMap
                };
                var chunk2 = new Chunk()
                {
                    x = 1,
                    y = 1,
                    map = tileMap
                };
                // Insert new customer document (Id will be auto-incremented)
                collection.Insert(chunk);
                collection.Insert(chunk2);
                
                var chunkOutput = collection.FindAll();
                
                Console.WriteLine("aantal waardes: " +  collection.Count());
                Console.WriteLine("aantal waardes: " +  collection.Count());

                var results = collection.Query()
                    .Where( chunk => chunk.x.Equals(lookingForX) && chunk.y.Equals(lookingForY) ) 
                    .Select(queryOutput => new {x = queryOutput.x, y = queryOutput.y })
                    .Limit(10)
                    .ToList();
                foreach (var result in results)
                {
                    Console.WriteLine("waarde: " +  result);
                }


                // Update a document inside a collection
                //  customer.Name = "Jane Doe";

                //  col.Update(customer);

                // Index document using document Name property
                //  col.EnsureIndex(x => x.Name);

                // Use LINQ to query documents (filter, sort, transform)
                /*
                var results = col.Query()
                    .Where(x => x.Name.StartsWith("J"))
                    .OrderBy(x => x.Name)
                    .Select(x => new {x.Name, NameUpper = x.Name.ToUpper()})
                    .Limit(10)
                    .ToList();
                */
                
                // Let's create an index in phone numbers (using expression). It's a multikey index
                //    col.EnsureIndex(x => x.Phones);

                // and now we can query phones
                //    var r = col.FindOne(x => x.Phones.Contains("8888-5555"));
            }
        }
    }
}