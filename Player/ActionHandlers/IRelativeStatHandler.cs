using DataTransfer.DTO.Character;

namespace Player.ActionHandlers
{
    public interface IRelativeStatHandler
    {
        public void SendStat(RelativeStatDTO stat);
        public int GetHealth();
        public int GetStamina();
        public int GetRadiationLevel();
    }
}