using System;
using System.Diagnostics.Eventing.Reader;
using DataTransfer.DTO.Character;
using DataTransfer.DTO.Player;
using Network;
using Network.DTO;
using Newtonsoft.Json;
using Player.DTO;
using Player.Services;
using WorldGeneration;

namespace Player.ActionHandlers
{
    public class MoveHandler : IMoveHandler, IPacketHandler
    {
        private IClientController _clientController;
        private PlayerDTO _currentPlayer;
        private WorldService _worldService;

        public MoveHandler(IClientController clientController)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.Move);
        }
        
        public void SendMove(MapCharacterDTO player, WorldService worldService)
        {
            _worldService = worldService;
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
            //Check welk actie het is:
            var moveDTO = JsonConvert.DeserializeObject<MoveDTO>(packet.Payload);
            if (packet.Header.Target == "host")
            {
                //handlemove
            }
            else if (packet.Header.Target == "client")
            {
                //adjust playerlist
                
                //moet op id vergeleken worden
                // if (moveDTO.PlayerPosition. == _currentPlayer.)
                // {
                    //adjust actual player
                    // _currentPlayer.ChangePositionOfAPlayer(moveDTO.Player);
                    
                    //change database
                    // CreateAsync(moveDTO.Player);
                // }
            }

            HandleMove(moveDTO.PlayerPosition);
            return new HandlerResponseDTO(SendAction.Ignore, null);
            // return null;
        }

         private HandlerResponseDTO HandleMove(MapCharacterDTO playerPosition)
        {
            // if (player.PlayerName.Equals(_currentPlayer.GetName()))
            // {
                _worldService.UpdateCharacterPosition(playerPosition);

                // _currentPlayer.SetX(player.X);
                // _currentPlayer.SetY(player.Y);
                // Make changes in database
            // }

            // _currentPlayer.ChangePositionOfAPlayer(player);
        }
        
        private void InsertToDatabase(MoveDTO moveDto)
        {
           //Get database
           //Check database
           //Insert Database
           //if inserted then send to all a message
        }
        
            return null;
        }
    }
}