using System;
using System.Diagnostics.CodeAnalysis;

namespace DatabaseHandler.POCO
{
    [ExcludeFromCodeCoverage]
    public class ChunkPoco
    {
        public int X;
        public int Y;
        public ITile[] Map;
        public int RowSize;
    }
}
