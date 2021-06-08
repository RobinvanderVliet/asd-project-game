using System.Collections.Generic;
using System.Linq;
using ActionHandling;
using Agent.Services;
using DatabaseHandler.POCO;
using DatabaseHandler.Services;
using Network;
using Network.DTO;
using Newtonsoft.Json;
using Session.DTO;
using WorldGeneration;

namespace Creature
{
    public class AgentHandler : IAgentHandler, IPacketHandler
    {
        private bool replaced;
        private Agent _agent;
        private readonly IWorldService _worldService;
        private readonly IMoveHandler _moveHandler;
        private readonly IClientController _clientController;
        private readonly IDatabaseService<AgentPOCO> _databaseService;
        private readonly IConfigurationService _configurationService;
        
        // TODO: add attack handler when that is on develop
        public AgentHandler(IWorldService worldService, IMoveHandler moveHandler, IClientController clientController, IDatabaseService<AgentPOCO> databaseService, IConfigurationService configurationService)
        {
            _worldService = worldService;
            _moveHandler = moveHandler;
            _configurationService = configurationService;
            _configurationService.CreateConfiguration("agent");
            _clientController = clientController;
            _databaseService = databaseService;
            _clientController.SubscribeToPacketType(this, PacketType.Agent);
        }

        public void Replace()
        {
            if (_agent == null) CreateAgent();
            
            replaced = !replaced;
            if (replaced)
            {
                _agent.AgentStateMachine.StartStateMachine();
            }
            else
            {
                _agent.AgentStateMachine.StopStateMachine();
            }
        }

        private void CreateAgent()
        {
            var player = _worldService.GetCurrentPlayer();
            var configuration = _configurationService.Configuration;
            _agent = new Agent(player.Name, player.XPosition, player.YPosition, player.Symbol, player.Id)
            {
                AgentData =
                {
                    MoveHandler = _moveHandler, WorldService = _worldService, Health = player.Health,
                    Inventory = player.Inventory, Stamina = player.Stamina, Team = player.Team,
                    RadiationLevel = player.RadiationLevel, VisionRange = 6, RuleSet = configuration.Settings
                }
            };
        }

        public HandlerResponseDTO HandlePacket(PacketDTO packet)
        {
            var configurationDto = JsonConvert.DeserializeObject<AgentConfigurationDTO>(packet.Payload);
            if (packet.Header.Target != "host") return new HandlerResponseDTO(SendAction.Ignore, null);
            
            var allAgents = _databaseService.GetAllAsync();
            allAgents.Wait();
            if (allAgents.Result.Any(x => x.PlayerGUID == configurationDto.PlayerId))
            {
                UpdateAgentConfiguration(configurationDto, allAgents.Result);
            }
            else
            {
                InsertAgentConfiguration(configurationDto);
            }
            return new HandlerResponseDTO(SendAction.Ignore, null);
        }

        private void InsertAgentConfiguration(AgentConfigurationDTO configurationDto)
        {
            var agentPoco = new AgentPOCO
            {
                PlayerGUID = configurationDto.PlayerId, AgentConfiguration = configurationDto.AgentConfiguration
            };
            _databaseService.CreateAsync(agentPoco);
        }

        private void UpdateAgentConfiguration(AgentConfigurationDTO agentConfigurationDto, IEnumerable<AgentPOCO> allAgentsResult)
        {
            var agentPoco = allAgentsResult.First(x => x.PlayerGUID == agentConfigurationDto.PlayerId);
            agentPoco.AgentConfiguration = agentConfigurationDto.AgentConfiguration;
            _databaseService.UpdateAsync(agentPoco);
        }
    }
}