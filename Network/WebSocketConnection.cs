using Newtonsoft.Json;
using System;
using WebSocketSharp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace Network
{
    public class WebSocketConnection
    {
        private WebSocket _websocket;
        private WebSocketConnectionConfig _webSocketConnectionConfig;
        public WebSocketConnection()
        {
            LoadConfigVariables();
            _websocket = new WebSocket($"ws://{_webSocketConnectionConfig.Ip}:{_webSocketConnectionConfig.Port}/{_webSocketConnectionConfig.Path}");
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

        public WebSocketConnection(WebSocket webSocket)
        {
            this._websocket = webSocket;
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
            _websocket.OnOpen += Websocket_OnOpen;
            _websocket.OnError += Websocket_OnError;
            _websocket.OnClose += Websocket_OnClose;
        }

        private void Websocket_OnClose(object sender, CloseEventArgs e)
        {
            Console.WriteLine("connection close");
        }

        private void Websocket_OnError(object sender, WebSocketSharp.ErrorEventArgs e)
        {
            Console.WriteLine("error event");
        }

        private void Websocket_OnOpen(object sender, EventArgs e)
        {
            Console.WriteLine("connection open");
        }

        private void OnMessage(object sender, MessageEventArgs e)
        {
            ObjectPayloadDTO objectPayloadDTO = JsonConvert.DeserializeObject<ObjectPayloadDTO>(e.Data);
            Console.WriteLine("Received: " + objectPayloadDTO.Header.Target);
            Console.WriteLine("Received: " + objectPayloadDTO.Payload);

            if (ObjectPayloadHandler.CheckHeader(objectPayloadDTO.Header))
            {
                ObjectPayloadHandler.CheckActionType(objectPayloadDTO);
            }
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
            _webSocketConnectionConfig = section.Get<WebSocketConnectionConfig>();
        }
    }
}
