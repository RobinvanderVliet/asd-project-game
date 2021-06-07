using ASD_project.Creature.Creature.StateMachine;
using ASD_project.Network;
using ASD_project.Network.Enum;

namespace ASD_project.Creature.Creature
{
    public class Player : ICreature
    {
        private IClientController _clientController;
        private ICreatureStateMachine _playerStateMachine;

        public ICreatureStateMachine CreatureStateMachine
        {
            get => _playerStateMachine;
        }

        public Player(ICreatureStateMachine playerStateMachine, IClientController clientController)
        {
            _clientController = clientController;
            SendChatMessage("Starting Agent to replace player");
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

        private void SendChatMessage(string message)
        {
            _clientController.SendPayload(message, PacketType.Chat);
        }
    }
}
