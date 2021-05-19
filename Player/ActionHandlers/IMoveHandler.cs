using DataTransfer.DTO.Player;
using Player.DTO;
using Player.Model;

namespace Player.ActionHandlers
{
    public interface IMoveHandler
    {
        public void SendMove(PlayerPositionDTO player);
    }
}