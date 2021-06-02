using WorldGeneration.Models.Interfaces;

namespace WorldGeneration
{
    public interface IWorldService
    {
        public void UpdateCharacterPosition(string userId, int newXPosition, int newYPosition);
        public void AddPlayerToWorld(Player player, bool isCurrentPlayer);
        public void DisplayWorld();
        public void DeleteMap();
        public void GenerateWorld(int seed);
        public Player getCurrentPlayer();
        public Player GetPlayer(string id);
        public ITile GetTile(int x, int y);
        public void LoadArea(int playerX, int playerY, int viewDistance);
    }
}