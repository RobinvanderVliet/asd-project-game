using Newtonsoft.Json;
using System.Linq;
using System.Timers;
using ASD_Game.ActionHandling.DTO;
using ASD_Game.DatabaseHandler.POCO;
using ASD_Game.DatabaseHandler.Services;
using ASD_Game.Messages;
using ASD_Game.Network;
using ASD_Game.Network.DTO;
using ASD_Game.Network.Enum;
using ASD_Game.World.Models.Characters;
using ASD_Game.World.Models.HazardousTiles;
using ASD_Game.World.Services;
using Timer = System.Timers.Timer;

namespace ASD_Game.ActionHandling
{
    public class RelativeStatHandler : IRelativeStatHandler, IPacketHandler
    {
        private Player _player;

        private const int STAMINA_TIMER = 1000;
        private const int RADIATION_TIMER = 1000;

        private Timer _staminaTimer;
        private Timer _radiationTimer;

        private readonly IClientController _clientController;
        private readonly IWorldService _worldService;
        private readonly IDatabaseService<PlayerPOCO> _playerDatabaseService;
        private readonly IMessageService _messageService;

        public RelativeStatHandler(IClientController clientController, IWorldService worldService, IDatabaseService<PlayerPOCO> playerDatabaseService, IMessageService messageService)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.RelativeStat);
            _worldService = worldService;
            _playerDatabaseService = playerDatabaseService;
            _messageService = messageService;
        }

        public void CheckStaminaTimer()
        {
            _staminaTimer = new Timer(STAMINA_TIMER);
            _staminaTimer.AutoReset = true;
            _staminaTimer.Elapsed += StaminaEvent;
            _staminaTimer.Start();
        }

        public void CheckRadiationTimer()
        {
            _radiationTimer = new Timer(RADIATION_TIMER);
            _radiationTimer.AutoReset = true;
            _radiationTimer.Elapsed += RadiationEvent;
            _radiationTimer.Start();
        }

        private void StaminaEvent(object sender, ElapsedEventArgs e)
        {
            if (_worldService.IsDead(_worldService.GetCurrentPlayer()))
            {
                _staminaTimer.Stop();
                return;
            }

            if (_player.Stamina < 100)
            {
                var statDto = new RelativeStatDTO();
                statDto.Stamina = 1;
                SendStat(statDto);
            }
        }

        private void RadiationEvent(object sender, ElapsedEventArgs e)
        {
            if (_worldService.IsDead(_worldService.GetCurrentPlayer()))
            {
                _messageService.AddMessage("You died");
                _worldService.DisplayWorld();
                _radiationTimer.Stop();
                return;
            }

            var tile = _worldService.GetTile(
                _worldService.GetCurrentPlayer().XPosition,
                _worldService.GetCurrentPlayer().YPosition);

            if (tile is GasTile)
            {
                var statDto = new RelativeStatDTO();

                if (_player.RadiationLevel > 0)
                {
                    statDto.RadiationLevel = -1;
                }
                else if (_player.Health > 0)
                {
                    statDto.Health = -1;
                }
                SendStat(statDto);
            }
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
            bool handleInDatabase = (_clientController.IsHost() && packet.Header.Target.Equals("host")) ||
                                    _clientController.IsBackupHost;

            var player = _worldService.GetPlayer(relativeStatDTO.Id);
            if (player.Stamina < Player.STAMINA_MAX && relativeStatDTO.Stamina != 0)
            {
                player.AddStamina(relativeStatDTO.Stamina);
                if (relativeStatDTO.Id == _clientController.GetOriginId())
                {
                    _worldService.DisplayStats();
                }

                InsertToDatabase(relativeStatDTO, handleInDatabase, player);
                return new HandlerResponseDTO(SendAction.SendToClients, null);
            }

            if (player.RadiationLevel > 0 && relativeStatDTO.RadiationLevel != 0)
            {
                player.AddRadiationLevel(relativeStatDTO.RadiationLevel);
                if (relativeStatDTO.Id == _clientController.GetOriginId())
                {
                    _worldService.DisplayStats();
                }

                InsertToDatabase(relativeStatDTO, handleInDatabase, player);
                return new HandlerResponseDTO(SendAction.SendToClients, null);
            }

            if (player.Health > 0 && relativeStatDTO.Health != 0)
            {
                player.AddHealth(relativeStatDTO.Health);
                if (relativeStatDTO.Id == _clientController.GetOriginId())
                {
                    _worldService.DisplayStats();
                }

                InsertToDatabase(relativeStatDTO, handleInDatabase, player);
                return new HandlerResponseDTO(SendAction.SendToClients, null);
            }
            else
            {
                _worldService.DisplayWorld();
            }

            return new HandlerResponseDTO(SendAction.ReturnToSender, null);
        }

        private void InsertToDatabase(RelativeStatDTO relativeStatDTO, bool handleInDatabase, Player player)
        {
            if (handleInDatabase)
            {
                PlayerPOCO playerPOCO = _playerDatabaseService.GetAllAsync().Result.FirstOrDefault(poco =>
                    poco.PlayerGUID == player.Id && poco.GameGUID == _clientController.SessionId);
                if (relativeStatDTO.Stamina != 0)
                {
                    playerPOCO.Stamina = player.Stamina;
                }
                else if (relativeStatDTO.RadiationLevel != 0)
                {
                    playerPOCO.RadiationLevel = player.RadiationLevel;
                }
                else if (relativeStatDTO.Health != 0)
                {
                    playerPOCO.Health = player.Health;
                }
                _playerDatabaseService.UpdateAsync(playerPOCO);
            }
        }

        public void SetCurrentPlayer(Player player)
        {
            _player = _worldService.GetCurrentPlayer();
        }
    }
}