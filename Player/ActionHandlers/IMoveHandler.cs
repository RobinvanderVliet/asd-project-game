using Player.Model;

namespace Player.ActionHandlers
{
    public interface IMoveHandler
    {
        public void SendMove(IPlayerModel player);
    }
}