using System.Diagnostics.Eventing.Reader;
using DataTransfer.DTO.Character;
using DataTransfer.DTO.Player;
using Network;
using Network.DTO;
using Newtonsoft.Json;
using Player.DTO;

namespace Player.ActionHandlers
{
    public class MoveHandler : IMoveHandler, IPacketHandler
    {
        private IClientController _clientController;
        private PlayerDTO _currentPlayer;

        public MoveHandler(IClientController clientController)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.Move);
        }

        public void SendMove(PlayerPositionDTO player)
        {
            //_currentPlayer = player;
            //var playerPostionDTO = new PlayerPositionDTO(player.XPosition, player.YPosition, player.Name, player.Team);
            var moveDTO = new MoveDTO(player);
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
            HandleMove(moveDTO.PlayerPosition);
            return new HandlerResponseDTO(SendAction.Ignore, null);
        }
                
       private void HandleMove(PlayerPositionDTO playerPosition)
       {
           // worldService.updateArraylistposition(player, x, y);
           
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