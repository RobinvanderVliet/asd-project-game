using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session
{
    public class SessionHandler : IPacketHandler, ISessionHandler
    {
        private ClientController _clientController;

        public SessionHandler(ClientController clientController)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.Session);
        }

        public void JoinSession(string sessionId)
        {

        }

        public void CreateSession(string sessionName)
        {

        }

        public void FindSessions()
        {

        }

        public bool HandlePacket(PacketDTO packet)
        {
            throw new NotImplementedException();
        }
    }
}
