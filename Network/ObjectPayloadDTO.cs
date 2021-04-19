using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    class ObjectPayloadDTO
    {
        public PayloadHeaderDTO header { get; set; }
        public ChatActionDTO chatAction { get; set; }
        public MoveActionDTO moveAction { get; set; }
        public AttackActionDTO attackAction { get; set; }
        public JoinActionDTO joinAction { get; set; }
        public SessionUpdateActionDTO sessionUpdateAction { get; set; }
    }
}
