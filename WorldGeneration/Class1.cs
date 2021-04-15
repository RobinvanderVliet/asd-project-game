using System;
using System.Drawing;
using LiteDB;

   Project name: ASD project.

   This file is created by team: 3.

   Goal of this file: Prototype database storage.

*/

using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration
{
    class Class1
    {
        public Class1()
        {
            Console.WriteLine("Game is gestartAAAAAAAAAAAAAA");

            using (var db = new LiteDatabase(@"C:\Temp\ChunkDatabase.db"))
            {
                // Get a collection (or create, if doesn't exist)
                var col = db.GetCollection<Chunk>("chunks");

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
                    x = 0,
                    y = 0,
                    map = tileMap
                };

                // Insert new customer document (Id will be auto-incremented)
                col.Insert(customer);

                // Update a document inside a collection
                customer.Name = "Jane Doe";

                col.Update(customer);

                // Index document using document Name property
                col.EnsureIndex(x => x.Name);

                // Use LINQ to query documents (filter, sort, transform)
                var results = col.Query()
                    .Where(x => x.Name.StartsWith("J"))
                    .OrderBy(x => x.Name)
                    .Select(x => new {x.Name, NameUpper = x.Name.ToUpper()})
                    .Limit(10)
                    .ToList();

                // Let's create an index in phone numbers (using expression). It's a multikey index
                col.EnsureIndex(x => x.Phones);

                // and now we can query phones
                var r = col.FindOne(x => x.Phones.Contains("8888-5555"));
            }
        }
    }
}