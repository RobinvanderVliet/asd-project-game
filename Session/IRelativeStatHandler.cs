using DataTransfer.DTO.Character;

namespace Session
{
    public interface IRelativeStatHandler
    {
        public void CheckStaminaTimer();
        public void CheckRadiationTimer();
        public void SendStat(RelativeStatDTO stat);
    }
}