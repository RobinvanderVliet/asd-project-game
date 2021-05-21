using System;
using Session.DTO;
using System.Collections.Generic;
using System.Threading;

namespace Session
{
    public class HeartbeatHandler : IHeartbeatHandler
    {
        private List<HeartbeatDTO> _players;
        TimeSpan waitTime = TimeSpan.FromMilliseconds(1000);

        public HeartbeatHandler()
        {
            _players = new List<HeartbeatDTO>();
            Timer timer = new Timer((e) =>
            {
                UpdateStatus();
            }, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(2000));
        }

        public HeartbeatHandler(List<HeartbeatDTO> _playerlist)
        {
            _players = _playerlist;
            Timer timer = new Timer((e) =>
            {
                UpdateStatus();
            }, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(2000));
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

        private void CheckStatus()
        {
            List<HeartbeatDTO> leavers = new List<HeartbeatDTO>();
            foreach (HeartbeatDTO player in _players)
            {
                if (player.status)
                {
                    leavers.Add(player);
                }
            }
            if (leavers.Count != 0)
            {
                EnablePlayerAgent(leavers);
            }
        }

        private void EnablePlayerAgent(List<HeartbeatDTO> leavers)
        {
            Console.WriteLine("Agents are enabled");
            foreach (HeartbeatDTO player in leavers)
            {
                _players.Remove(player);
            }
        }

        private bool PlayerKnown(string sessionID)
        {
            return _players.Exists(player => player.sessionID == sessionID);
        }

        private void UpdateStatus()
        {
            foreach (HeartbeatDTO player in _players)
            {
                if (DateTime.Now - player.time >= waitTime)
                {
                    player.status = false;
                }
                else
                {
                    player.status = true;
                }
            }
            CheckStatus();
        }

        private void UpdatePlayer(string sessionID)
        {
            foreach (HeartbeatDTO player in _players)
            {
                if (player.sessionID == sessionID)
                {
                    player.time = DateTime.Now;
                }
            }
        }
    }
}
