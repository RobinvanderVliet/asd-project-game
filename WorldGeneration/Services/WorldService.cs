namespace WorldGeneration
{
    public class WorldService : IWorldService
    {
        private World _world { get; set; }


        public void UpdateCharacterPosition(string userId, int newXPosition, int newYPosition)
        {
            _world.UpdateCharacterPosition(userId, newXPosition, newYPosition);
        }

        public void AddPlayerToWorld(Player player, bool isCurrentPlayer)
        {
            _world.AddPlayerToWorld(player, isCurrentPlayer);
        }

        public void DisplayWorld()
        {
            _world.DisplayWorld();
        }
        
        public void DeleteMap()
        {
            _world.DeleteMap();
        }

        public void GenerateWorld(int seed)
        {
            _world = new World(seed, 6, new MapFactory());
        }

        public Player getCurrentPlayer()
        {
            return _world.CurrentPlayer;
        }

        public World GetWorld()
        {
            return _world;
        }
    }
}