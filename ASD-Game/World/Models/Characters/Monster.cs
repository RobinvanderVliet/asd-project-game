

using WorldGeneration.StateMachine;

namespace WorldGeneration
{
    public class Monster : Character
    {
        private ICharacterStateMachine _monsterStateMachine;

        public ICharacterStateMachine CharacterStateMachine
        {
            get => _monsterStateMachine;
        }

        public Monster(string name, int xPosition, int yPosition, string symbol, ICharacterStateMachine monsterStateMachine) : base(name, xPosition, yPosition, symbol, monsterStateMachine)
        {
            _monsterStateMachine = monsterStateMachine;
            _monsterStateMachine.StartStateMachine();
        }

        public void ApplyDamage(double amount)
        {
            _monsterStateMachine.CharacterData.Health -= amount;
        }

        public void HealAmount(double amount)
        {
            _monsterStateMachine.CharacterData.Health += amount;
        }
    }
}
