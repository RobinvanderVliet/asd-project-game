using Player.Services;

namespace Player.ActionHandlers
{
    public interface IMoveHandler
    {
        public void sendMove(IPlayerService player, int x, int y);
    }
}