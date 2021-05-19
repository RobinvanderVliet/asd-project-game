using System;
using Network;
using Network.DTO;
using Newtonsoft.Json;
using Player.DTO;
using Player.Model;
using Player.Services;

namespace Player.ActionHandlers
{
    public class MoveHandler : IMoveHandler, IPacketHandler
    {
        private IClientController _clientController;
        private IPlayerModel _currentPlayer;

        public MoveHandler(IClientController clientController)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.Move);
        }

        public void SendMove(IPlayerModel player)
        {
            _currentPlayer = player;
            var playerDTO = new PlayerDTO(player.Name, player.XPosition, player.YPosition);
            var moveDTO = new MoveDTO(playerDTO);
            SendMoveDTO(moveDTO);
        } 
        
        private void SendMoveDTO(MoveDTO moveDTO)
        {
            var payload = JsonConvert.SerializeObject(moveDTO);
            _clientController.SendPayload(payload, PacketType.Move);
        }
        
        public HandlerResponseDTO HandlePacket(PacketDTO packet)
        {
            //Check welk actie het is:
            var moveDTO = JsonConvert.DeserializeObject<MoveDTO>(packet.Payload);
            
            //check if pakketje is host: Zo ja Controlleer of het kan zert dan in database en stuur naar alle clients een bericht
            
           
            //check if pakketje is client: Zo ja voer het uit. 
            HandleMove(moveDTO.Player);
            return new HandlerResponseDTO(SendAction.Ignore, null);
        }

        private void InsertToDatabase(MoveDTO moveDto)
        {
           //Get database
           //Check database
           //Insert Database
           //if inserted then send to all a message
           
            
            
        }
                
       private void HandleMove(PlayerDTO player)
       {

        
           
         
       }
        
    }
}