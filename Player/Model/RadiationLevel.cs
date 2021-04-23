namespace Player.Model
{
    public class RadiationLevel : IRadiationLevel
    {
        public int Level { get; set; }

        public RadiationLevel(int radiationLevel)
        {
            Level = radiationLevel;
        }
    }
}
