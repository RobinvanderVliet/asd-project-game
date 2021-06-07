using System.Diagnostics.CodeAnalysis;
using ASD_project.World.Models.Interfaces;

namespace ASD_project.World.Models
{
    [ExcludeFromCodeCoverage]
    public class Chunk
    {
        public int X { get; set; }
        public int Y { get; set; }
        public ITile[] Map { get; set; }
        public int RowSize { get; set; }
        public int Seed { get; set; }

    }
}