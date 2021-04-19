using System;
using Newtonsoft.Json;
using WebSocketSharp;

namespace Network
{
    public class WebsocketServerConnecter
    {
        static void Main(string[] args)
        {
            //Creating of a disposable WsClient instance
            using (WebSocket ws = new WebSocket("ws://localhost:8088/Echo"))
            {
                ws.OnMessage += (sender, e) =>
                {
                    ObjectPayloadDTO objectPayloadDTO2 = JsonConvert.DeserializeObject<ObjectPayloadDTO>(e.Data);
                    Console.WriteLine(objectPayloadDTO2.header.target);
                    Console.WriteLine(objectPayloadDTO2.header.originID);
                    Console.WriteLine(objectPayloadDTO2.header.sessionID);
                    Console.WriteLine(objectPayloadDTO2.header.actionType);
                    Console.WriteLine(objectPayloadDTO2.chatAction.chat);
                    Console.WriteLine(objectPayloadDTO2.chatAction.message);
                };

                PayloadHeaderDTO payloadHeaderDTO1 = new PayloadHeaderDTO();
                payloadHeaderDTO1.target = "host";
                payloadHeaderDTO1.originID = "ce4d2959-cc81-4722-8801-eba55173536";
                payloadHeaderDTO1.sessionID = "10";
                payloadHeaderDTO1.actionType = "chat";

                ChatActionDTO chatActionDTO1 = new ChatActionDTO();
                chatActionDTO1.chat = "team";
                chatActionDTO1.message = "hello world";

                ObjectPayloadDTO objectPayloadDTO1 = new ObjectPayloadDTO();
                objectPayloadDTO1.header = payloadHeaderDTO1;
                objectPayloadDTO1.chatAction = chatActionDTO1;

                string message = JsonConvert.SerializeObject(objectPayloadDTO1);

                ws.Connect();
                ws.Send(message);

                Console.ReadKey();
            }

            Receiver receiver = new Receiver();

            receiver.checkActionType("actionType", "payload");
        }
    }
}