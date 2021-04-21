using System;
using Newtonsoft.Json;
using WebSocketSharp;

namespace Network
{
    class WebsocketServerConnector
    {
        static void Main(string[] args)
        {
            WebSocketConnection webSocketConnector = new WebSocketConnection();

            PayloadHeaderDTO payloadHeaderDTO = new PayloadHeaderDTO();
            payloadHeaderDTO.Target = "host";
            payloadHeaderDTO.OriginID = "ce4d2959-cc81-4722-8801-eba55173536";
            payloadHeaderDTO.SessionID = "10";
            payloadHeaderDTO.ActionType = "chatAction";

            string testPayload = "testPayload";

            ObjectPayloadDTO objectPayloadDTO = new ObjectPayloadDTO();
            objectPayloadDTO.Header = payloadHeaderDTO;
            objectPayloadDTO.Payload = testPayload;

            string message = JsonConvert.SerializeObject(objectPayloadDTO);

            webSocketConnector.Send(message);
            Console.ReadKey();
        }
    }
}
