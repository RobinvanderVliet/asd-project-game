using System;
using DataTransfer.DTO.Character;
using Network;
using Newtonsoft.Json;
using System.Linq;
using System.Timers;
using DatabaseHandler;
using DatabaseHandler.POCO;
using DatabaseHandler.Repository;
using DatabaseHandler.Services;
using Network.DTO;
using WorldGeneration;
using WorldGeneration.Models.HazardousTiles;
using WorldGeneration.Models.Interfaces;

namespace Session
{
    public class RelativeStatHandler : IRelativeStatHandler, IPacketHandler
    {
        private int _health;
        private int _stamina;
        private int _radiationLevel;
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
            if (_player.Stamina >= 100)
            {
                return;
            }
            Console.WriteLine("Mijn staminaaaaaaaaaaaaaaaaaaaaaaaaa" + _player.Stamina);

            var statDto = new RelativeStatDTO();
            statDto.Id = _worldService.getCurrentPlayer().Id;
            statDto.Stamina = 1;
            SendStat(statDto);
            _player.Stamina = _stamina;

            Console.WriteLine("Stamina regained! S: " + _player.Stamina);
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

            if (_clientController.IsHost() && packet.Header.Target.Equals("host"))
            {
                var dbConnection = new DbConnection();

                var playerRepository = new Repository<PlayerPOCO>(dbConnection);
                var servicePlayer = new ServicesDb<PlayerPOCO>(playerRepository);

                var playerStats = servicePlayer.GetAllAsync().Result.Where(x =>
                    x.PlayerGuid == relativeStatDTO.Id
                );

                _health = relativeStatDTO.Health += playerStats.Select(x => x.Health).FirstOrDefault();
                _stamina = relativeStatDTO.Stamina += playerStats.Select(x => x.Stamina).FirstOrDefault();
                _radiationLevel = relativeStatDTO.RadiationLevel += playerStats.Select(x => x.RadiationLevel).FirstOrDefault();

                InsertToDatabase(relativeStatDTO);
            }
            
            HandleStat(relativeStatDTO);
            
            return new HandlerResponseDTO(SendAction.SendToClients, null);
        }
        
        private void InsertToDatabase(RelativeStatDTO statDTO)
        {
            var dbConnection = new DbConnection();

            var playerRepository = new Repository<PlayerPOCO>(dbConnection);
            var servicePlayer = new ServicesDb<PlayerPOCO>(playerRepository);

            // var destination = _mapper.Map<PlayerPOCO>(statDTO);
            var player = playerRepository.GetAllAsync().Result.FirstOrDefault(player => player.PlayerGuid == statDTO.Id);

            player.Health = statDTO.Health;
            player.Stamina = statDTO.Stamina;
            player.RadiationLevel = statDTO.RadiationLevel;
            playerRepository.UpdateAsync(player);
            
            // if (servicePlayer.UpdateAsync(destination).Result == 1)
            // {
            //     //TODO: check if successful or not
            // }
        }

        private void HandleStat(RelativeStatDTO statDTO)
        {
            var dto = _worldService.getCurrentPlayer();
            dto.Health += statDTO.Health;
            dto.Stamina += statDTO.Stamina;
            dto.RadiationLevel += statDTO.RadiationLevel;
            
            _worldService.UpdatePlayer(dto);
        }
    }
}