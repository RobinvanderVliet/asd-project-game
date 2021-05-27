using DataTransfer.DTO.Character;

namespace Player.ActionHandlers
{
    public interface IMoveHandler
    {
        public void SendMove(MapCharacterDTO player);
        public int GetStamina();
    }
}