using Creature.Creature.StateMachine;
using Network;

namespace Creature
{
    public class Player : ICreature
    {
        private ICreatureStateMachine _playerStateMachine;
        private IClientController _clientController;

        public ICreatureStateMachine CreatureStateMachine
        {
            get => _playerStateMachine;
        }

        public Player(ICreatureStateMachine playerStateMachine, IClientController clientController)
        {
            _clientController = clientController;
            _playerStateMachine = playerStateMachine;
        }

        public void ApplyDamage(double amount)
        {
            _playerStateMachine.CreatureData.Health -= amount;
        }

        public void HealAmount(double amount)
        {
            _playerStateMachine.CreatureData.Health += amount;
        }
        
        public void Disconnect()
        {
            SendChatMessage("Starting Agent to replace player");
            _playerStateMachine.StartStateMachine();
        }
        
        private void SendChatMessage(string message)
        {
            _clientController.SendPayload(message, PacketType.Chat);
        }
    }
}