namespace WorldGeneration.Models.HazardousTiles
{
    public class GasTile : HazardousTile
    {
        public int Radius { get; set; }
        public GasTile(int radius) 
        {
            Symbol = "&";
            Accessible = true;

            this.Radius = radius;
        }

        public override int GetDamage(int time)
        {
            return time * Radius;
        }
    }
}
