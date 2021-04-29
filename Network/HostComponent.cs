namespace Network
{
    public class HostComponent : IPacketListener
    {
        private NetworkComponent _networkComponent;
        private IPacketHandler _client;
        private SessionComponent _session;

        public HostComponent(NetworkComponent networkComponent, IPacketHandler client, SessionComponent session)
        {
            _networkComponent = networkComponent;
            _client = client;
            _session = session;
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
            
            if(packet.Header.SessionID == _session.SessionId)
            {
                bool success = _client.HandlePacket(packet);
                if (success)
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
        
        public void CreateGame()
        {
            // TODO: Implement methods.
        }
    }
}
