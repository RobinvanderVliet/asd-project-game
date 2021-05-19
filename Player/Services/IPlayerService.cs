using DataTransfer.DTO.Player;
using Player.DTO;
using Player.Model;

namespace Player.Services
{
    public interface IPlayerService
    {
        public void Attack(string direction);

        public void ExitCurrentGame();

        public void Pause();

        public void Resume();

        public void ReplaceByAgent();

        public void Say(string messageValue);

        public void Shout(string messageValue);

        public void AddHealth(int amount);

        public void RemoveHealth(int amount);

        public void AddStamina(int amount);

        public void RemoveStamina(int amount);

        public IItem GetItem(string itemName);

        public void AddInventoryItem(IItem item);

        public void RemoveInventoryItem(IItem item);

        public void EmptyInventory();

        public void AddBitcoins(int amount);

        public void RemoveBitcoins(int amount);

        public int GetAttackDamage();

        public void PickupItem();

        public void DropItem(string itemNameValue);

        public void HandleDirection(string directionValue, int stepsValue);
        
        public void ChangePositionOfAPlayer(PlayerPositionDTO playerPosition);
        
        public void CreateSession(string messageValue);
        
        public void JoinSession(string messageValue);
        
        public void RequestSessions();
    }
}