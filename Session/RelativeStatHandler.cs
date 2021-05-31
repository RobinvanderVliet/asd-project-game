using System;
using DataTransfer.DTO.Character;
using Network;
using Newtonsoft.Json;
using System.Linq;
using System.Threading;
using System.Timers;
using DatabaseHandler;
using DatabaseHandler.POCO;
using DatabaseHandler.Repository;
using DatabaseHandler.Services;
using Network.DTO;
using WorldGeneration;
using WorldGeneration.Models.HazardousTiles;
using WorldGeneration.Models.Interfaces;
using Timer = System.Timers.Timer;

namespace Session
{
    public class RelativeStatHandler : IRelativeStatHandler, IPacketHandler
    {
        // private int _health;
        // private int _stamina;
        // private int _radiationLevel;
        private Player _player;
        // TimeSpan waitTime = TimeSpan.FromMilliseconds(1000);

        private int TIMER = 1000;
        private Timer _staminaTimer;
        private Timer _radiationTimer;

        private IClientController _clientController;
        private IWorldService _worldService;

        public RelativeStatHandler(IClientController clientController, IWorldService worldService)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.RelativeStat);
            _worldService = worldService;
            _player = _worldService.getCurrentPlayer();
            // _health = _player.Health;
            // _stamina = _player.Stamina;
            // _radiationLevel = _player.RadiationLevel;
            CheckStaminaTimer();
            CheckRadiationTimer();
        }
        
        private void CheckStaminaTimer()
        {
            _staminaTimer = new Timer(TIMER);
            _staminaTimer.AutoReset = true;
            _staminaTimer.Elapsed += StaminaEvent;
            _staminaTimer.Start();
        }
        
        private void CheckRadiationTimer()
        {
            _radiationTimer = new Timer(TIMER);
            _radiationTimer.AutoReset = true;
            _radiationTimer.Elapsed += RadiationEvent;
            _radiationTimer.Start();
        }
        
        private void StaminaEvent(object sender, ElapsedEventArgs e)
        {
            // Console.WriteLine("Mijn stamina" + _player.Stamina);
            if (_player.Stamina < 100)
            {
                // Console.WriteLine("Mijn staminaaaaaaaaaaaaaaaaaaaaaaaaa" + _player.Stamina);

                var statDto = new RelativeStatDTO();
                statDto.Stamina = 1;
                SendStat(statDto);
                // _player.Stamina = _stamina;

                // Console.WriteLine("Stamina regained! S: " + (_player.Stamina));
            }
        }
        
        private void RadiationEvent(object sender, ElapsedEventArgs e)
        {
            // ITile tile = _worldService.GetTile(
            //     _worldService.getCurrentPlayer().XPosition, 
            //     _worldService.getCurrentPlayer().YPosition);
            //
            // if (tile is GasTile || true)
            // {
            //     var statDto = new RelativeStatDTO();
            //     statDto.Id = _worldService.getCurrentPlayer().Id;
            //
            //     if (_player.RadiationLevel > 0)
            //     {
            //         statDto.RadiationLevel = -1;
            //     }
            //     else if (_player.Health > 0)
            //     {
            //         statDto.Health = -1;
            //     }
            //
            //     SendStat(statDto);
            //     _player.RadiationLevel = _radiationLevel;
            //     _player.Health = _health;
            //
            //     Console.WriteLine("Radiation damage! H: " + _player.Health + " | R: " + _player.RadiationLevel);
            // }
        }

        public void SendStat(RelativeStatDTO statDTO)
        {
            statDTO.Id = _clientController.GetOriginId();
            SendStatDTO(statDTO);
        }

        private void SendStatDTO(RelativeStatDTO statDTO)
        {
            var payload = JsonConvert.SerializeObject(statDTO);
            _clientController.SendPayload(payload, PacketType.RelativeStat);
        }
        
        public HandlerResponseDTO HandlePacket(PacketDTO packet)
        {
            var relativeStatDTO = JsonConvert.DeserializeObject<RelativeStatDTO>(packet.Payload);
            bool handleInDatabase = (_clientController.IsHost() && packet.Header.Target.Equals("host")) || _clientController.IsBackupHost;

            var player = _worldService.GetPlayer(relativeStatDTO.Id);
            if(player.Stamina < Player.STAMINACAP)
            {
                player.AddStamina(relativeStatDTO.Stamina);
                Console.WriteLine(relativeStatDTO.Id + " gained stamina: " + player.Stamina);
                if (handleInDatabase)
                {
                    var dbConnection = new DbConnection();
                    var playerRepository = new Repository<PlayerPOCO>(dbConnection);

                    PlayerPOCO playerPOCO = playerRepository.GetAllAsync().Result.FirstOrDefault(poco => poco.PlayerGuid == player.Id);
                    playerPOCO.Stamina = player.Stamina;
                    playerRepository.UpdateAsync(playerPOCO);
                }
                return new HandlerResponseDTO(SendAction.SendToClients, null);
            }
            else
            {
                return new HandlerResponseDTO(SendAction.ReturnToSender, "Stamina already max");
            }
        }

    }
}