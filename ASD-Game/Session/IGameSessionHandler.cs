
using ASD_Game.World.Models.Characters;

namespace ASD_Game.Session
{
    public interface IGameSessionHandler
    {
        public void SendGameSession();
        public void SetStateMachine(Monster monster);
    }
}