using Player.Model;
using Player.Services;

namespace Player.ActionHandlers
{
    public interface IMoveHandler
    {
        public void SendMove(IPlayerService player);
    }
}