using System;
using DataTransfer.DTO.Character;
using Network;
using Newtonsoft.Json;
using System.Linq;
using AutoMapper;
using DatabaseHandler;
using DatabaseHandler.POCO;
using DatabaseHandler.Repository;
using DatabaseHandler.Services;
using Network.DTO;
using Player.Model;
using Player.Services;
using WorldGeneration;

namespace Player.ActionHandlers
{
    public class RelativeStatHandler : IRelativeStatHandler, IPacketHandler
    {
        private int _health;
        private int _stamina;
        private int _radiationLevel;

        private IMapper _mapper;
        private IClientController _clientController;
        private IWorldService _worldService;

        public RelativeStatHandler(IMapper mapper, IClientController clientController, IWorldService worldService)
        {
            _mapper = mapper;
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.RelativeStat);
            _worldService = worldService;
        }

        public void SendStat(RelativeStatDTO statDTO)
        {
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
                    x.GameGuid == relativeStatDTO.GameGuid &&
                    x.PlayerGuid == relativeStatDTO.PlayerGuid
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

            var destination = _mapper.Map<PlayerPOCO>(statDTO);

            if (servicePlayer.UpdateAsync(destination).Result == 1)
            {
                //TODO: check if successful or not
            }
        }

        private void HandleStat(RelativeStatDTO statDTO)
        {
            var dto = _worldService.getCurrentCharacterPositions();
            dto.Health += statDTO.Health;
            dto.Stamina += statDTO.Stamina;
            dto.RadiationLevel += statDTO.RadiationLevel;
            
            _worldService.UpdateCharacterPosition(dto);
        }

        public int GetHealth()
        {
            return _health;
        }
        
        public int GetStamina()
        {
            return _stamina;
        }

        public int GetRadiationLevel()
        {
            return _radiationLevel;
        }
    }
}