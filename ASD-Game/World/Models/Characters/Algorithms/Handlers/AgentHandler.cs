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
        private readonly IWorldService _worldService;
        private readonly IMoveHandler _moveHandler;
        private readonly IClientController _clientController;
        private readonly IDatabaseService<AgentPOCO> _databaseService;
        private readonly IConfigurationService _configurationService;
        private readonly IAttackHandler _attackHandler;

        // string = playerId
        private Dictionary<string, WorldGeneration.Agent> _agents;

        public AgentHandler(IWorldService worldService, IMoveHandler moveHandler, IClientController clientController,
            IDatabaseService<AgentPOCO> databaseService, IConfigurationService configurationService,
            IAttackHandler attackHandler)
        {
            _worldService = worldService;
            _moveHandler = moveHandler;
            _configurationService = configurationService;
            _attackHandler = attackHandler;
            _configurationService.CreateConfiguration("agent");
            _clientController = clientController;
            _databaseService = databaseService;
            _clientController.SubscribeToPacketType(this, PacketType.Agent);
            _agents = new Dictionary<string, WorldGeneration.Agent>();
        }

        public void Replace(string playerId)
        {
            if (_worldService.GetWorld() == null) return;

            var player = _worldService.GetPlayer(playerId);
            _agents.TryGetValue(playerId, out var agent);

            var allAgents = _databaseService.GetAllAsync();
            allAgents.Wait();

            // If player in database
            if (allAgents.Result.All(x => x.PlayerGUID != player.Id)) return;

            var agentPoco = allAgents.Result.First();

            // If agent is not activated
            if (!agentPoco.Activated || agent == null)
            {
                var agentConfiguration = agentPoco.AgentConfiguration;

                // Get agent from database
                agent = CreateAgent(player, agentConfiguration);
                _agents.Add(player.Id, agent);

                // Activate agent
                agent.AgentStateMachine.StartStateMachine();

                // Update database
                agentPoco.Activated = true;
            }
            else if (!agent.AgentStateMachine.WasStarted())
            {
                // Activate agent
                agent.AgentStateMachine.StartStateMachine();

                // Update database
                agentPoco.Activated = true;
            }
            else
            {
                // Deactivate agent
                agent.AgentStateMachine.StopStateMachine();

                // Update database
                agentPoco.Activated = false;
            }

            var updateAsync = _databaseService.UpdateAsync(agentPoco);
            updateAsync.Wait();
        }

        private WorldGeneration.Agent CreateAgent(Player player, List<KeyValuePair<string, string>> agentConfiguration)
        {
            return new(player.Name, player.XPosition, player.YPosition, player.Symbol, player.Id)
            {
                AgentData =
                {
                    MoveHandler = _moveHandler, WorldService = _worldService, Health = player.Health,
                    Inventory = player.Inventory, Stamina = player.Stamina, Team = player.Team,
                    RadiationLevel = player.RadiationLevel, VisionRange = 6, RuleSet = agentConfiguration,
                    AttackHandler = _attackHandler, CharacterId = player.Id
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
                PlayerGUID = configurationDto.PlayerId, AgentConfiguration = configurationDto.AgentConfiguration,
                GameGUID = configurationDto.GameGUID, Activated = configurationDto.Activated
            };
            _databaseService.CreateAsync(agentPoco);
        }

        private void UpdateAgentConfiguration(AgentConfigurationDTO agentConfigurationDto,
            IEnumerable<AgentPOCO> allAgentsResult)
        {
            var agentPoco = allAgentsResult.First(x => x.PlayerGUID == agentConfigurationDto.PlayerId);
            agentPoco.AgentConfiguration = agentConfigurationDto.AgentConfiguration;
            _databaseService.UpdateAsync(agentPoco);
        }
    }
}