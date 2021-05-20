using AutoBogus;
using DataTransfer.POCO.World;

namespace DatabaseHandler.Tests
{
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