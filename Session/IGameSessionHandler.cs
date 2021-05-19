using System;

namespace Session
{
    public interface IGameSessionHandler
    {
        public Boolean InGame { get; set; }
        public void SendGameSession(string messageValue, ISessionHandler sessionHandler);
    }
}