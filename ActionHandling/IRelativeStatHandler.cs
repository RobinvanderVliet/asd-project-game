using DataTransfer.DTO.Character;

namespace ActionHandling
{
    public interface IRelativeStatHandler
    {
        public void SendStat(RelativeStatDTO stat);
        public int GetHealth();
        public int GetStamina();
        public int GetRadiationLevel();
    }
}