#nullable enable
namespace WorldGeneration.Database
{
    public class ChunkNotFoundException : DatabaseException
    {
        public ChunkNotFoundException(string? message) : base(message)
        {
        }
    }
}