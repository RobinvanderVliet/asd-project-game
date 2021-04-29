using System;

namespace Network
{
    public static class ObjectPayloadHandler {
        public static void CheckActionType(PacketDTO objectPayloadDTO) 
        {
            switch (objectPayloadDTO.Header.ActionType)
            {
                case "chatAction":
                    Console.WriteLine("Case chatAction");
                    //Send payload to chatActionComponent
                    break;
                case "moveAction":
                    Console.WriteLine("Case moveAction");
                    //Send payload to moveActionComponent
                    break;
                case "attackAction":
                    Console.WriteLine("Case attackAction");
                    //Send payload to attackActionComponent
                    break;
                case "joinAction":
                    Console.WriteLine("Case joinAction");
                    //Send payload to joinActionComponent
                    break;
                case "sessionUpdateAction":
                    Console.WriteLine("Case sessionUpdateAction");
                    //Send payload to sessionUpdateActionComponent
                    break;
                default:
                    Console.WriteLine("Not a valid actiontype");
                    break;
            }
        }

        public static Boolean CheckHeader(PacketHeaderDTO payloadHeaderDTO)
        {
            // returns true if header suggests payload is meant for this client, otherwise returns false
            Console.WriteLine("Checking session with ID: " + payloadHeaderDTO.SessionID);
            return true;
        }
    }
}
