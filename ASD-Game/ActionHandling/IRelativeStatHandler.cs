
using ActionHandling.DTO;

namespace ActionHandling
{
    public interface IRelativeStatHandler
    {
        public void CheckStaminaTimer();
        public void CheckRadiationTimer();
        public void SendStat(RelativeStatDTO stat);
    }
}