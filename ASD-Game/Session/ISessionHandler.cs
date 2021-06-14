using System.Collections.Generic;
using ASD_Game.World.Models.Characters.Algorithms.NeuralNetworking.TrainingScenario;

namespace ASD_Game.Session
{
    public interface ISessionHandler
    {
        public TrainingScenario TrainingScenario { get; set; }

        public bool JoinSession(string sessionId, string userName);
        public bool CreateSession(string sessionName, string userName);
        public void RequestSessions();
        public int GetSessionSeed();
        public List<string[]> GetAllClients();
    }
}