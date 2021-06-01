using System.Diagnostics.CodeAnalysis;
using AutoBogus;
using WorldGeneration.Models;

namespace DatabaseHandler.Tests
{
    [ExcludeFromCodeCoverage]
    public sealed class ChunkFaker : AutoFaker<Chunk>
    {
        public ChunkFaker()
        {
            RuleFor(chunk => chunk.X, f => f.Random.Int(0, 10000));
            RuleFor(chunk => chunk.Y, f => f.Random.Int(0, 10000));
            RuleFor(chunk => chunk.RowSize, f => f.Random.Int(0, 500));
        }
    }
}