using DataTransfer.POCO.World.Interfaces;

namespace DataTransfer.POCO.World.HazardousTiles
{
    public class GasTile : IHazardousTile
    {
        
        public bool IsAccessible { get; set; }
        public string Symbol { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public GasTile(int x, int y, int radius = 1)
        {
            Symbol = TileSymbol.GAS;
            IsAccessible = true;
            XPosition = x;
            YPosition = y;

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