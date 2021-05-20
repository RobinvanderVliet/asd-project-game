namespace WorldGeneration.Models.Interfaces
{
    public interface ITile
    {
        bool IsAccessible { get; set; }
        string Symbol { get; set; }
        int XPosition { get; set; }
        int YPosition { get; set; }
    }
}