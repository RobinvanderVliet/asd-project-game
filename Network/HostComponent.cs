namespace Network
{
    public class HostComponent : IPacketListener
    {
        private NetworkComponent _networkComponent;
        private IPacketHandler _client;
        private SessionComponent _session;

        public HostComponent(NetworkComponent networkComponent, IPacketHandler client, SessionComponent session)
        {
            this._networkComponent = networkComponent;
            this._client = client;
            this._session = session;
        }

        public void ReceivePacket(PacketDTO packet)
        {
            if(packet.Header.SessionID == _session.SessionId)
            {
                bool success = _client.HandlePacket(packet);
                if (success)
                {
                    packet.Header.Target = "client";
                    _networkComponent.SendPayload(packet);
                }
                else
                {
                    //TODO: send error
                }
            }
        }
    }
}
