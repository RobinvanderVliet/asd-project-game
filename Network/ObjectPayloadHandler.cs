using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    class ObjectPayloadHandler {
        public void checkActionType(ObjectPayloadDTO objectPayloadDTO) 
        {
            switch (objectPayloadDTO.header.actionType) 
            {
                case "chatAction":
                    Console.WriteLine("Case chatAction");
                    processChatAction(objectPayloadDTO.chatAction);
                    break;
                case "moveAction":
                    Console.WriteLine("Case moveAction");
                    processMoveAction(objectPayloadDTO.moveAction);
                    break;
                case "attackAction":
                    Console.WriteLine("Case attackAction");
                    processAttackAction(objectPayloadDTO.attackAction);
                    break;
                case "joinAction":
                    Console.WriteLine("Case joinAction");
                    processJoinAction(objectPayloadDTO.joinAction);
                    break;
                case "sessionUpdateAction":
                    Console.WriteLine("Case sessionUpdateAction");
                    processSessionUpdateAction(objectPayloadDTO.sessionUpdateAction);
                    break;
                default:
                    Console.WriteLine("Not a valid actiontype");
                    break;
            }
        }

        public Boolean checkHeader(PayloadHeaderDTO payloadHeaderDTO)
        {
            Console.WriteLine("Checking session with ID: " + payloadHeaderDTO.sessionID);
            return true;
        }

        public void processChatAction(ChatActionDTO chatActionDTO) {
            //Process chat action
        }

        public void processMoveAction(MoveActionDTO moveActionDTO) {
            //Process move action
        }

        public void processAttackAction(AttackActionDTO attackActionDTO) {
            //Process attack action
        }

        public void processJoinAction(JoinActionDTO joinActionDTO) {
            //Process join action
        }

        public void processSessionUpdateAction(SessionUpdateActionDTO sessionUpdateActionDTO) {
            //Process session update action
        }
    }
}
