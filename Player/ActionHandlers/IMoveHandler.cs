using Player.Services;

namespace Player.ActionHandlers
{
    public interface IMoveHandler
    {
        public void SendMove(IPlayerService player, int x, int y);
    }
}