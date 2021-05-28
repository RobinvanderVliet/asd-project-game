using System;
using System.Diagnostics.CodeAnalysis;
using DataTransfer.Model.World.Interfaces;

namespace DatabaseHandler.POCO
{
    [ExcludeFromCodeCoverage]
    public class ChunkPoco : IEquatable<ChunkPoco>
    {
        public int X;
        public int Y;
        public ITile[] Map;
        public int RowSize;
        
        public bool Equals(ChunkPoco other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }
            
            return Equals((ChunkPoco)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, RowSize);
        }
    }
}
