using Network.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    public class HeartbeatComponent
    {

        private List<HeartbeatDTO> Players;
        TimeSpan waitTime = TimeSpan.FromSeconds(1);

        public void RecieveHeartbeat(PacketDTO packet)
        {
            if(!PlayerKnown(packet.Header.SessionID))
            { 
               Players.Add(new HeartbeatDTO(packet.Header.SessionID));
            }
            
        }

        private void CheckStatus()
        {
            foreach(HeartbeatDTO player in Players)
            {
                if(player.status == 0)
                {
                    enablePlayerAgent();
                }
            }
        }

        private void enablePlayerAgent()
        {

        }

        private bool PlayerKnown(string sessionID) 
        {
            foreach(HeartbeatDTO player in Players)
            {
                if(sessionID == player.sessionID)
                {
                    return true;
                }
            }
            return false;
        }

        public void updateStatus()
        {
            foreach (HeartbeatDTO player in Players)
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
        }

    }
}
