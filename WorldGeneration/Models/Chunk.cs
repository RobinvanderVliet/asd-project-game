using System;
using System.Diagnostics.CodeAnalysis;
using WorldGeneration.Models.Interfaces;

namespace WorldGeneration.Models
{
    [ExcludeFromCodeCoverage]
    public class Chunk : IEquatable<Chunk>
    {
        public int X { get; set; }
        public int Y { get; set; }
        public ITile[] Map { get; set; }
        public int RowSize { get; set; }
        
        public int Seed { get; set; }
        
        public Chunk()
        {
        }
        public Chunk(int x, int y, ITile[] map, int rowSize, int seed)
        {
            X = x;
            Y = y;
            Map = map;
            RowSize = rowSize;
            Seed = seed;
        }

        public bool Equals(Chunk other)
        {
            if (ReferenceEquals(null, other)) 
                return false;
            
            if (ReferenceEquals(this, other)) 
                return true;
            
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) 
                return false;
            
            if (ReferenceEquals(this, obj)) 
                return true;
            
            if (obj.GetType() != GetType()) 
                return false;
            
            return Equals((Chunk) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, RowSize);
        }
    }
}