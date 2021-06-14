using System;
using System.Collections.Generic;
using System.Timers;
using ASD_Game.Session.DTO;
using ASD_Game.Messages;
using System.Threading;
using System.Diagnostics;

namespace ASD_Game.Session
{
    public class HeartbeatHandler : IHeartbeatHandler
    {
        private List<HeartbeatDTO> _players;
        TimeSpan waitTime = TimeSpan.FromMilliseconds(1000);

        private int TIMER = 1000;
        private Thread _checkHeartbeatThread;
        private bool _runThread;
        private IMessageService _messageService;

        public HeartbeatHandler(IMessageService messageService)
        {
            _players = new List<HeartbeatDTO>();
            _messageService = messageService;
            StartHeartbeatThread();
        }

        private void StartHeartbeatThread()
        {
            _runThread = true;
            _checkHeartbeatThread = new Thread(() =>
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                while (_runThread)
                {
                    if (stopwatch.ElapsedMilliseconds >= TIMER)
                    {
                        CheckHeartbeatEvent();
                        stopwatch.Restart();
                    }
                }

            })
            { Priority = ThreadPriority.Highest, IsBackground = true };
            _checkHeartbeatThread.Start();
        }

        public HeartbeatHandler(List<string> players, IMessageService messageService)
        {
            _players = new List<HeartbeatDTO>();
            foreach (var player in players)
            {
                _players.Add(new HeartbeatDTO(player));
            }
            _messageService = messageService;
            StartHeartbeatThread();
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

        private void CheckHeartbeatEvent()
        {
            UpdateStatus();
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
            _messageService.AddMessage("Agents are enabled");
            foreach (HeartbeatDTO player in leavers)
            {
                _players.Remove(player);
            }
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
