using Network.DTO;
using System;
using System.Collections.Generic;

namespace Network
{
    public class HeartbeatHandler : IHeartbeatHandler
    {
        private List<HeartbeatDTO> _players;
        TimeSpan waitTime = TimeSpan.FromMilliseconds(1000);

        public void ReceiveHeartbeat(PacketDTO packet)
        {
            if(!PlayerKnown(packet.Header.SessionID))
            { 
               _players.Add(new HeartbeatDTO(packet.Header.SessionID));
            }
            else
            {
                UpdatePlayer(packet.Header.SessionID);
                UpdateStatus();
            }
            
        }

        private void CheckStatus()
        {
            foreach(HeartbeatDTO player in _players)
            {
                if(player.status == 0)
                {
                    EnablePlayerAgent();
                }
            }
        }

        private void EnablePlayerAgent()
        {
            Console.WriteLine("Agent is enabled");
        }

        private bool PlayerKnown(string sessionID) 
        {
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
