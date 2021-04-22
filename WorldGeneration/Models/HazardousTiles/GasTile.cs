namespace WorldGeneration.Models.HazardousTiles
{
    public class GasTile : HazardousTile
    {
        private int Radius { get; set; }
        public GasTile(int radius = 1) 
        {
            Symbol = TileSymbol.Gas;
            IsAccessible = true;

            Radius = radius;
        }

        public override int GetDamage(int time)
        {
            return time * Radius;
        }
    }
}
