using ActionHandling.DTO;
using ASD_project.World.Models.Characters;

namespace ASD_project.ActionHandling
{
    public interface IRelativeStatHandler
    {
        public void CheckStaminaTimer();
        public void CheckRadiationTimer();
        public void SendStat(RelativeStatDTO stat);
        public void SetCurrentPlayer(Player player);
    }
}