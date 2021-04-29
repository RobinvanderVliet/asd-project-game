using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    class Network
    {
        static void Main(string[] args)
        {
            ClientComponent clientComponent = new ClientComponent();
            NetworkComponent networkComponent = new NetworkComponent(clientComponent);

            SessionComponent sessionComponent = new SessionComponent();

            clientComponent.StartGame(networkComponent, sessionComponent);
        }
    }
}
