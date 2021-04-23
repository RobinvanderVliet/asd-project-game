namespace WorldGeneration.Models.HazardousTiles
{
    public class GasTile : HazardousTile
    {
        public GasTile(int radius = 1)
        {
            Symbol = TileSymbol.GAS;
            IsAccessible = true;

            Radius = radius;
        }

        private int Radius { get; }

        public override int GetDamage(int time)
        {
            return time * Radius;
        }
    }
}