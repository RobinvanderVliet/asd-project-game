﻿using Creature.Creature.NeuralNetworking.TrainingScenario;
using System;
using System.Collections.Generic;
using Session.DTO;

namespace Session
{
    public interface ISessionHandler
    {
        public TrainingScenario trainingScenario { get; set; }
        public bool JoinSession(string sessionId, string userName);
        public bool CreateSession(string sessionName, string userName);
        public void RequestSessions();

        public void SendHeartbeat();

        public int GetSessionSeed();
        public List<string[]> GetAllClients();
    }
}