using Newtonsoft.Json;
using Player.DTO;
using Player.Services;

namespace Player.ActionHandlers
{
    public class MoveHandler : IMoveHandler, IPacketHandler
    {
        private IClientController _clientController;

        public MoveHandler(IClientController clientController)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.Move);
        }

        public void SendMove(IPlayerService player, int x, int y)
        {
            var moveDTO = new MoveDTO(player, x, y);
            SendMoveDTO(moveDTO);

        } 
        private void SendMoveDTO(MoveDTO moveDTO)
        {
            var payload = JsonConvert.SerializeObject(moveDTO);        
            _clientController.SendPayload(payload, PacketType.Move);
        }
        
        public HandlerResponseDTO HandlePacket(PacketDTO packet)
                {
                    var moveDTO = JsonConvert.DeserializeObject<MoveDTO>(packet.Payload);
                    HandleMove(moveDTO.Message);
                            return new HandlerResponseDTO(false, null);
                }
                
       private void HandleMove(IPlayerService player, int x, int y)
               {
                   // aanroepen daadwerkelijke functie voor aanpassen x en y in wereld (dus in arraylist)
               }
        
    }
}