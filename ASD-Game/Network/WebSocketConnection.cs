using System;
using ASD_Game.Network.DTO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.IO;
using WebSocketSharp;

namespace ASD_Game.Network
{
    public class WebSocketConnection : IWebSocketConnection
    {
        private readonly WebSocket _websocket;
        private WebSocketConnectionConfig _webSocketConnectionConfig;
        private readonly IPacketListener _packetListener;

        private UserSettingsConfig _userSettingsConfig;

        public UserSettingsConfig UserSettingsConfig
        {
            get => _userSettingsConfig;
            set => _userSettingsConfig = value;
        }


        public WebSocketConnection(IPacketListener packetListener)
        {
            LoadConfigVariables();
            _websocket =
                new WebSocket(
                    $"ws://{_webSocketConnectionConfig.Ip}:{_webSocketConnectionConfig.Port}/{_webSocketConnectionConfig.Path}");
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
            var config = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json").Build();
            var connectionSection = config.GetSection(nameof(WebSocketConnectionConfig));
            var connectionResult = connectionSection.Get<WebSocketConnectionConfig>();
            _webSocketConnectionConfig = connectionResult;

            var userSection = config.GetSection(nameof(UserSettingsConfig));
            var userResult = userSection.Get<UserSettingsConfig>();
            _userSettingsConfig = userResult;
        }

        public void AddOrUpdateConfigVariables<T>(string key, T value)
        {
            try
            {
                string pathToRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory));
                var filePath = Path.Combine(pathToRoot, "appsettings.json");
                string json = File.ReadAllText(filePath);
                dynamic jsonObj = JsonConvert.DeserializeObject(json);

                var sectionPath = key.Split(":")[0];
                if (!string.IsNullOrEmpty(sectionPath))
                {
                    var keyPath = key.Split(":")[1];
                    jsonObj[sectionPath][keyPath] = value;
                }
                else
                {
                    jsonObj[sectionPath] = value;
                }

                string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                File.WriteAllText(filePath, output);

            }
            catch (Exception e)
            {
                Console.WriteLine("Error writing configuration variables: " + e);
            }
        }
    }
}
