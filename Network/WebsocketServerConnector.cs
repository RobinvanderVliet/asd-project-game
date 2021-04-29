using System;
using Newtonsoft.Json;
using WebSocketSharp;

namespace Network
{
    class WebsocketServerConnector
    {
        static void Main(string[] args)
        {
            WebSocketConnection webSocketConnection = new WebSocketConnection();

            PacketHeaderDTO payloadHeaderDTO = new PacketHeaderDTO();
            payloadHeaderDTO.Target = "host";
            payloadHeaderDTO.OriginID = "ce4d2959-cc81-4722-8801-eba55173536";
            payloadHeaderDTO.SessionID = "10";
            payloadHeaderDTO.ActionType = ActionType.Chat;

            string testPayload = "testPayload";

            PacketDTO objectPayloadDTO = new PacketDTO();
            objectPayloadDTO.Header = payloadHeaderDTO;
            objectPayloadDTO.Payload = testPayload;

            string message = JsonConvert.SerializeObject(objectPayloadDTO);

            webSocketConnection.Send(message);
            Console.ReadKey();
        }
    }
}
