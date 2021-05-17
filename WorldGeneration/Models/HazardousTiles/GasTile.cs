using WorldGeneration.Models.Interfaces;

namespace WorldGeneration.Models.HazardousTiles
{
    public class GasTile : IHazardousTile
    {
        
        public bool IsAccessible { get; set; }
        public string Symbol { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public GasTile(int x, int y, int radius = 1)
        {
            Symbol = TileSymbol.GAS;
            IsAccessible = true;
            X = x;
            Y = y;

            Radius = radius;
        }

        private int Radius { get; }

        public int Damage { get; set; }

        public int GetDamage(int time)
        {
            return time * Radius;
        }
    }
}