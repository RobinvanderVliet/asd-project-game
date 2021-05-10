using System.Collections.Generic;

namespace Network
{
    public class HostController : IPacketListener
    {
        private NetworkComponent _networkComponent;
        private IPacketHandler _client;
        private Session _session;
        private List<PacketDTO> _packetQueue;

        public HostController(NetworkComponent networkComponent, IPacketHandler client, Session session)
        {
            _networkComponent = networkComponent;
            _client = client;
            _session = session;
            _networkComponent.Host = this;
            _packetQueue = new();
        }

        public void ReceivePacket(PacketDTO packet)
        {
            if (packet.Header.PacketType == PacketType.GameAvailability)
            {
                PacketDTO packetDto = new PacketBuilder()
                    .SetSessionID(_session.SessionId)
                    .SetTarget("client")
                    .SetPacketType(PacketType.GameAvailable)
                    .SetPayload(_session.Name)
                    .Build();

                _networkComponent.SendPacket(packetDto);
            }
            else if(packet.Header.SessionID == _session.SessionId)
            {
                _packetQueue.Add(packet);
                HandleQueue();
            }
        }

        private void HandleQueue()
        {
            foreach(var packet in _packetQueue)
            {
                HandlePacket(packet);
                _packetQueue.Remove(packet);
            }
        }

        private void HandlePacket(PacketDTO packet)
        {
            bool succesfullyHandledPacket = _client.HandlePacket(packet);
            if (succesfullyHandledPacket)
            {
                packet.Header.Target = "client";
                _networkComponent.SendPacket(packet);
            }
            else
            {
                //TODO: send error
            }
        }
    }
}
