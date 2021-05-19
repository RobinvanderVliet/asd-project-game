using System;
using Network;
using Network.DTO;
using Newtonsoft.Json;
using Player.DTO;
using Player.Services;

namespace Player.ActionHandlers
{
    public class MoveHandler : IMoveHandler, IPacketHandler
    {
        private IClientController _clientController;
        private IPlayerService _currentPlayer;

        public MoveHandler(IClientController clientController)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.Move);
        }

        public void SendMove(IPlayerService player)
        {
            _currentPlayer = player;
            var playerDTO = new PlayerDTO(_currentPlayer.GetName(), _currentPlayer.GetX(), _currentPlayer.GetY());
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
            if (packet.Header.Target == "host")
            {
                //handlemove
            }
            else if (packet.Header.Target == "client")
            {
                // Adjust playerService Arraylist.
                //moet op id vergeleken worden
                if (moveDTO.Player.PlayerName == _currentPlayer.GetName())
                {
                    CreateAsync(moveDTO.Player);
                }
                
                //moet aangepast worden
                _currentPlayer.ChangePositionOfAPlayer(moveDTO.Player);
            }

            // HandleMove(moveDTO.Player); 
            // return new HandlerResponseDTO(SendAction.Ignore, null);
            return null;
        }
           
       //  private HandlerResponseDTO HandleMove(PlayerDTO player)
       // {
       //     if (player.PlayerName.Equals(_currentPlayer.GetName()))
       //     {
       //       _currentPlayer.SetX(player.X);
       //       _currentPlayer.SetY(player.Y);
       //       // Make changes in database
       //     }
       //
       //     bool moveIsPossible = false;
       //     // check if the move is possible
       //     if(moveIsPossible){}
       //     else{}
       //     
       //     _currentPlayer.ChangePositionOfAPlayer(player);
       //
       //     return null;
       // }
    }
}