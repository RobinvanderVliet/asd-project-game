using Creature.Creature.StateMachine;
using Creature.Creature.StateMachine.CustomRuleSet;
using Creature.Creature.StateMachine.Data;
using Network;

namespace Creature
{
    public class Player : ICreature
    {
        private PlayerStateMachine _playerStateMachine;
        private ClientController _clientController;
        public ICreatureStateMachine CreatureStateMachine
        {
            get => _playerStateMachine;
        }

        public Player(PlayerData playerData, RuleSet ruleSet)
        {
            _clientController = new ClientController(new NetworkComponent());
            _playerStateMachine = new(playerData, ruleSet);
            SendChatMessenge("Agent is starting to replace player");
            _playerStateMachine.StartStateMachine();
        }

        public void ApplyDamage(double amount)
        {
            _playerStateMachine.CreatureData.Health -= amount;
        }

        public void HealAmount(double amount)
        {
            _playerStateMachine.CreatureData.Health += amount;
        }

        public void SendChatMessenge(string message)
        {
            _clientController.SendPayload(message, PacketType.Chat);
        }
    }
}