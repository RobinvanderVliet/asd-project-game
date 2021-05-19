using Creature.Creature.StateMachine;
using Network;

namespace Creature
{
    public class Player : ICreature
    {
        private ClientController _clientController;
        private ICreatureStateMachine _playerStateMachine;

        public ICreatureStateMachine CreatureStateMachine
        {
            get => _playerStateMachine;
        }

        public Player(ICreatureStateMachine playerStateMachine)
        {
            _clientController = new ClientController(new NetworkComponent());
            SendChatMessenge("Starting Agent to replace player");
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
            _playerStateMachine.StartStateMachine();
        }
        
        private void SendChatMessenge(string message)
        {
            _clientController.SendPayload(message, PacketType.Chat);
        }
    }
}