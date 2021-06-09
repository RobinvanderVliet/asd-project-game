using System;
using System.Collections.Generic;
using System.Timers;
using Creature;

using ASD_Game.Session.DTO;


namespace ASD_Game.Session
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
            var leavers = _players.FindAll(x => !x.IsOnline);
            if (leavers.Count != 0)
            {
                EnablePlayerAgent(leavers);
            }
        }

        private void EnablePlayerAgent(List<HeartbeatDTO> leavers)
        {
            foreach (HeartbeatDTO player in leavers)
            {
                ReplaceAgent(player);
                _players.Remove(player);
            }
        }

        private void ReplaceAgent(HeartbeatDTO player)
        {
            _agentHandler.Replace(player.ClientID);
        }

        private bool PlayerKnown(string clientID)
        {
            return _players.Exists(player => player.ClientID == clientID);
        }

        private void UpdateStatus()
        {
            foreach (HeartbeatDTO player in _players)
            {
                if (DateTime.Now - player.Time >= waitTime)
                {
                    player.IsOnline = false;
                } else if (!player.IsOnline)
                {
                    // TODO: implement when player returns take over agent
                    //ReplaceAgent(player);
                    player.IsOnline = true;
                }
                else
                {
                    player.IsOnline = true;
                }
            }
            CheckStatus();
        }

        private void UpdatePlayer(string clientID)
        {
            var player = _players.Find(x => x.ClientID == clientID);
            player.Time = DateTime.Now;
        }
    }
}
