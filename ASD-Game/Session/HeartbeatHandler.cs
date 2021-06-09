using Session.DTO;
using System;
using System.Collections.Generic;
using System.Timers;
using Creature;

namespace Session
{
    public class HeartbeatHandler : IHeartbeatHandler
    {
        private readonly IAgentHandler _agentHandler;
        private List<HeartbeatDTO> _players;
        TimeSpan waitTime = TimeSpan.FromMilliseconds(1000);

        private int TIMER = 1000;
        private Timer _heartbeatTimer;

        public HeartbeatHandler(IAgentHandler agentHandler)
        {
            _agentHandler = agentHandler;
            _players = new List<HeartbeatDTO>();
            _heartbeatTimer = new Timer(TIMER);
            CheckHeartbeatTimer();
        }

        public HeartbeatHandler(List<string> players)
        {
            _players = new List<HeartbeatDTO>();
            foreach (var player in players)
            {
                _players.Add(new HeartbeatDTO(player));
            }
            _heartbeatTimer = new Timer(TIMER);
            CheckHeartbeatTimer();
        }

        public void ReceiveHeartbeat(string clientId)
        {
            if (PlayerKnown(clientId))
            {
                UpdatePlayer(clientId);
                UpdateStatus();
            }
            else
            {
                _players.Add(new HeartbeatDTO(clientId));
            }
        }

        private void CheckHeartbeatTimer()
        {
            _heartbeatTimer.AutoReset = true;
            _heartbeatTimer.Elapsed += CheckHeartbeatEvent;
            _heartbeatTimer.Start();
        }

        private void CheckHeartbeatEvent(object sender, ElapsedEventArgs e)
        {
            _heartbeatTimer.Stop();
            UpdateStatus();
            _heartbeatTimer.Start();
        }

        private void CheckStatus()
        {
            var leavers = _players.FindAll(x => !x.online);
            if (leavers.Count != 0)
            {
                EnablePlayerAgent(leavers);
            }
        }

        private void EnablePlayerAgent(List<HeartbeatDTO> leavers)
        {
            foreach (HeartbeatDTO player in leavers)
            {
                _agentHandler.Replace(player.clientID);
                _players.Remove(player);
            }
        }

        private bool PlayerKnown(string clientID)
        {
            return _players.Exists(player => player.clientID == clientID);
        }

        private void UpdateStatus()
        {
            foreach (HeartbeatDTO player in _players)
            {
                if (DateTime.Now - player.time >= waitTime)
                {
                    player.online = false;
                }
                else
                {
                    player.online = true;
                }
            }
            CheckStatus();
        }

        private void UpdatePlayer(string clientID)
        {
            var player = _players.Find(x => x.clientID == clientID);
            player.time = DateTime.Now;
        }
    }
}
