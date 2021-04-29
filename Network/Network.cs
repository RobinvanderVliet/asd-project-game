using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    class Network
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Are you a Client or Host?");

            String roleResult = Console.ReadLine();

            ClientComponent clientComponent = new ClientComponent();
            SessionComponent sessionComponent = new SessionComponent();
            NetworkComponent networkComponent = new NetworkComponent(clientComponent);
            HostComponent hostComponent;
            
            if (roleResult != null && roleResult.ToLower() == "host")
            {
                Console.WriteLine("Name of your session?");
                String nameResult = Console.ReadLine();
                sessionComponent.Name = nameResult;
                sessionComponent.GenerateSessionId();
                hostComponent = new HostComponent(networkComponent, clientComponent, sessionComponent);
                networkComponent.Host = hostComponent;
                
                hostComponent.CreateGame();
                
            }
            else
            {
                PacketDTO packetDTO = new PacketBuilder()
                    .SetTarget("host")
                    .SetPacketType(PacketType.GameAvailability)
                    .SetPayload("testPayload")
                    .Build();

                networkComponent.SendPacket(packetDTO);
            }
        }
        
        
    }
}
