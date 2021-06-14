
using ASD_Game.World.Models.Characters;

namespace ASD_Game.Session
{
    public interface IGameSessionHandler
    {
        public void SendGameSession();
        public void SetBrain(SmartMonster monster);
        public void SetStateMachine(Monster monster);
    }
}