using System;
using ASD_Game.Network.DTO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WebSocketSharp;

namespace ASD_Game.Network
{
    public class WebSocketConnection : IWebSocketConnection
    {
        private readonly WebSocket _websocket;
        private WebSocketConnectionConfig _webSocketConnectionConfig;
        private readonly IPacketListener _packetListener;

        public WebSocketConnection(IPacketListener packetListener)
        {
            LoadConfigVariables();
            _websocket = new WebSocket($"ws://{_webSocketConnectionConfig.Ip}:{_webSocketConnectionConfig.Port}/{_webSocketConnectionConfig.Path}");
            _packetListener = packetListener;
            AddBehaviorToWebsocket();

            try
            {
                _websocket.Connect();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void AddBehaviorToWebsocket()
        {
            _websocket.OnMessage += OnMessage;
        }

        private void OnMessage(object sender, MessageEventArgs e)
        {
            PacketDTO packet = JsonConvert.DeserializeObject<PacketDTO>(e.Data);
            _packetListener.ReceivePacket(packet);
        }

        public void Send(string message)
        {
            try
            {
                _websocket.Send(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        private void LoadConfigVariables()
        {
            var config = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();
            var section = config.GetSection(nameof(WebSocketConnectionConfig));
            var result = section.Get<WebSocketConnectionConfig>();
            _webSocketConnectionConfig = result;
        }
    }
}
