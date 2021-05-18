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
            var moveDTO = JsonConvert.DeserializeObject<MoveDTO>(packet.Payload);
            HandleMove(moveDTO.Player);
            return new HandlerResponseDTO(SendAction.Ignore, null);
        }
                
       private void HandleMove(PlayerDTO player)
       {
           if (player.PlayerName.Equals(_currentPlayer.Name))
           {
               
           }
           // aanroepen daadwerkelijke functie voor aanpassen x en y in wereld (dus in arraylist)
           //_player.ChangePositionOfAPlayer(player);
           // als host dan in globale db aanpassen voor die speler (hostcontoller (HandlePacket))
           
           // als speler waarvan positie gewijzigd is dan in eigen db aanpassen
           //if (player.PlayerName == _player.//playerName
              //                                          )
           //{
               // ja: dan aanpassen in mijn (_player) db
               //_player.setData(int x, int y);
           //}
       }
        
    }
}