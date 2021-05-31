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
            Console.WriteLine("f3");
            if (_clientController.IsHost() && packet.Header.Target.Equals("host"))
            {
                Console.WriteLine("f2");
                var dbConnection = new DbConnection();

                var playerRepository = new Repository<PlayerPOCO>(dbConnection);
                var servicePlayer = new ServicesDb<PlayerPOCO>(playerRepository);
                
                // Thread.Sleep(1000);
                
                var playerStats = servicePlayer.GetAllAsync().Result.Where(x =>
                    x.PlayerGuid == relativeStatDTO.Id
                );
                
                var incomingPlayerStamina = playerStats.Select(x => x.Stamina).FirstOrDefault();

                if (incomingPlayerStamina < 100)
                {
                    var incomingPlayername = playerStats.Select(x => x.PlayerName).FirstOrDefault();
                    // var incomingPlayerHealth = playerStats.Select(x => x.Health).FirstOrDefault();
                    var incomingPlayerXPosition = playerStats.Select(x => x.XPosition).FirstOrDefault();
                    var incomingPlayerYPosition = playerStats.Select(x => x.YPosition).FirstOrDefault();
                    var incomingPlayerId = playerStats.Select(x => x.PlayerGuid).FirstOrDefault();
                    string incomingPlayerSymbol;

                    if (relativeStatDTO.Id.Equals(_clientController.GetOriginId()))
                    {
                        incomingPlayerSymbol = "#";
                    }
                    else
                    {
                        incomingPlayerSymbol = "E";
                    }
                    Console.WriteLine("f");
                    Player incomingPlayer = new Player(incomingPlayername, incomingPlayerXPosition, incomingPlayerYPosition, incomingPlayerSymbol, incomingPlayerId);
                
                    incomingPlayer.Health = relativeStatDTO.Health + playerStats.Select(x => x.Health).FirstOrDefault();
                    incomingPlayer.Stamina = relativeStatDTO.Stamina + incomingPlayerStamina;
                    incomingPlayer.RadiationLevel = relativeStatDTO.RadiationLevel + playerStats.Select(x => x.RadiationLevel).FirstOrDefault();
                    
                    InsertToDatabase(incomingPlayer);
                    HandleStat(incomingPlayer);
                    
                    Console.WriteLine(relativeStatDTO.Id + " gained stamina: " + incomingPlayer.Stamina);
                }
            }
            else if (relativeStatDTO.Id.Equals(_clientController.GetOriginId()))
            {
                // Console.WriteLine("Binnengekregen stamina in client " + relativeStatDTO.Stamina);
                // Console.WriteLine("Oude stamina " + _player.Stamina);
                _player.Health += relativeStatDTO.Health;
                _player.Stamina += relativeStatDTO.Stamina;
                // Console.WriteLine("Actuele stamina " + _player.Stamina);
                _player.RadiationLevel += relativeStatDTO.RadiationLevel;
                
                //new
                HandleStat(_player);
            }

            return new HandlerResponseDTO(SendAction.SendToClients, null);
        }
        
        private void InsertToDatabase(Player player)
        {
            var dbConnection = new DbConnection();

            var playerRepository = new Repository<PlayerPOCO>(dbConnection);
            var servicePlayer = new ServicesDb<PlayerPOCO>(playerRepository);

            // var destination = _mapper.Map<PlayerPOCO>(statDTO);
            var dbplayer = playerRepository.GetAllAsync().Result.FirstOrDefault(player1 => player1.PlayerGuid == player.Id);
            dbplayer.Health = player.Health;
            dbplayer.Stamina = player.Stamina;
            dbplayer.RadiationLevel = player.RadiationLevel;
            // Console.WriteLine("InsertDatabase stamina: " + dbplayer.Stamina);
            playerRepository.UpdateAsync(dbplayer);
            
            // if (servicePlayer.UpdateAsync(destination).Result == 1)
            // {
            //     //TODO: check if successful or not
            // }
        }

        private void HandleStat(Player player)
        {
            _worldService.UpdateCharacterStamina(player.Stamina);
            if (player.Id.Equals(_clientController.GetOriginId()))
            {
                _player = player;
            }
        }
    }
}