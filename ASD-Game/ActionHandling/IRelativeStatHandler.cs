using ASD_Game.ActionHandling.DTO;
using ASD_Game.World.Models.Characters;

namespace ASD_Game.ActionHandling
{
    public interface IRelativeStatHandler
    {
        public void CheckStaminaTimer();
        public void CheckRadiationTimer();
        public void SendStat(RelativeStatDTO stat);
        public void SetCurrentPlayer(Player player);
    }
}