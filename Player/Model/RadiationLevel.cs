namespace Player.Model
{
    public class RadiationLevel : IRadiationLevel
    {
        private int _level;
        public int Level { get => _level; set => _level = value; }

        public RadiationLevel(int radiationLevel)
        {
            _level = radiationLevel;
        }
    }
}
