using System;

namespace Session
{
    public interface IGameSessionHandler
    {
        public void SendGameSession(ISessionHandler sessionHandler);
    }
}