using System.Runtime.Intrinsics.X86;

namespace WorldGeneration.Models.Interfaces
{
    public interface ITile
    {
        bool IsAccessible { get; set; }
        string Symbol { get; set; }
        int X { get; set; }
        int Y { get; set; }
    }
}