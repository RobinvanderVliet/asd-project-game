using DataTransfer.DTO.Character;

namespace Player.ActionHandlers
{
    public interface IMoveHandler
    {
        public void SendMove(MapCharacterDTO player);
        public void HandleDirection(string directionValue, int stepsValue);
    }
}