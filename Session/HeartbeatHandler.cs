using Network.DTO;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Network
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
            if (!PlayerKnown(clientId))
            { 
               _players.Add(new HeartbeatDTO(clientId));
            }
            else
            {
                UpdatePlayer(clientId);
                UpdateStatus();
            }
            
        }

        private void CheckStatus()
        {
            List<HeartbeatDTO> leavers = new List<HeartbeatDTO>();
            foreach(HeartbeatDTO player in _players)
            {
                if(player.status == 0)
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
            foreach(HeartbeatDTO player in leavers)
            {
                _players.Remove(player);
            }
        }

        private bool PlayerKnown(string sessionID) 
        {
            if(_players == null)
            {
                return false;
            }
            foreach(HeartbeatDTO player in _players)
            {
                if(sessionID == player.sessionID)
                {
                    return true;
                }
            }
            return false;
        }

        private void UpdateStatus()
        {
            foreach (HeartbeatDTO player in _players)
            {
                if (DateTime.Now - player.time >= waitTime)
                {
                    player.status = 0;
                }
                else
                {
                    player.status = 1;
                }
            }
            CheckStatus();
        }

        private void UpdatePlayer(string sessionID)
        {
            foreach(HeartbeatDTO player in _players)
            {
                if(player.sessionID == sessionID)
                {
                    player.time = DateTime.Now;
                }
            }
        }
    }
}
