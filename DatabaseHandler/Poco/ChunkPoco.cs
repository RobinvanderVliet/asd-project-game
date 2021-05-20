using System;
using System.Diagnostics.CodeAnalysis;
using WorldGeneration.Models.Interfaces;

namespace DatabaseHandler.Poco
{
    [ExcludeFromCodeCoverage]
    public class ChunkPoco : IEquatable<ChunkPoco>
    {
        public int X { get; set; }
        public int Y { get; set; }
        public ITile[] Map { get; set; }
        public int RowSize { get; set; }

        public bool Equals(ChunkPoco other)
        {
            if (ReferenceEquals(null, other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return X == other.X && Y == other.Y && RowSize == other.RowSize;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (obj.GetType() != GetType())
                return false;

            return Equals((ChunkPoco)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, RowSize);
        }
    }
}
